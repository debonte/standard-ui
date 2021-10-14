﻿// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using SkiaSharp;
using static Microsoft.StandardUI.FactoryStatics;
using Microsoft.StandardUI.Shapes;
using Microsoft.StandardUI.Media;
using DefaultValueAttribute = Microsoft.StandardUI.DefaultValueAttribute;

namespace Microcharts
{
    public interface IChart : IControl
    {
        IEnumerable<ChartEntry> Entries { get; set; }

        Color BackgroundColor { get; set; }

        Color LabelColor { get; set; }
    }

    /// <summary>
    /// A chart.
    /// </summary>
    public abstract class ChartImplementation : StandardControlImplementation, INotifyPropertyChanged
    {
        private float animationProgress;
        private double margin = 20, labelTextSize = 16;
        private SKTypeface typeface;
        private double? internalMinValue, internalMaxValue;
        private bool isAnimated = true, isAnimating = false;
        private TimeSpan animationDuration = TimeSpan.FromSeconds(1.5f);
        private Task invalidationPlanification;
        private CancellationTokenSource animationCancellation;

        protected IChart Control { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.Chart"/> class.
        /// </summary>
        public ChartImplementation(IChart control) : base((IStandardControlEnvironmentPeer)control)
        {
            Control = control;

            PropertyChanged += OnPropertyChanged;

            // Animation doesn't currently work, so disable it by default
            IsAnimated = false;
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the chart is invalidated.
        /// </summary>
        public event EventHandler Invalidated;

        #region Properties

        /// <summary>
        /// Get or set value determining if the labels are in a unicode language (Chinese, Arabic, Hebrew, etc.)
        /// </summary>
        public bool UnicodeMode { get; set; }

        /// <summary>
        /// Used to help SkiaSharp.HarfBuzz correctly render unicode languages.
        /// use UnicodeLanguage class to set this value.
        /// </summary>
        public char UnicodeLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Microcharts.Chart"/> is animated when entries change.
        /// </summary>
        /// <value><c>true</c> if is animated; otherwise, <c>false</c>.</value>
        public bool IsAnimated
        {
            get => isAnimated;
            set
            {
                if (Set(ref isAnimated, value))
                {
                    if (!value)
                    {
                        AnimationProgress = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Microcharts.Chart"/> is currently animating.
        /// </summary>
        /// <value><c>true</c> if is animating; otherwise, <c>false</c>.</value>
        public bool IsAnimating
        {
            get => isAnimating;
            private set => Set(ref isAnimating, value);
        }

        /// <summary>
        /// Gets or sets the duration of the animation.
        /// </summary>
        /// <value>The duration of the animation.</value>
        public TimeSpan AnimationDuration
        {
            get => animationDuration;
            set => Set(ref animationDuration, value);
        }

        /// <summary>
        /// Gets or sets the global margin.
        /// </summary>
        /// <value>The margin.</value>
        public double Margin
        {
            get => margin;
            set => Set(ref margin, value);
        }

        /// <summary>
        /// Gets or sets the animation progress.
        /// </summary>
        /// <value>The animation progress.</value>
        public float AnimationProgress
        {
            get => animationProgress;
            set
            {
                value = Math.Min(1, Math.Max(value, 0));
                Set(ref animationProgress, value);
            }
        }

        /// <summary>
        /// Gets or sets the text size of the labels.
        /// </summary>
        /// <value>The size of the label text.</value>
        public double LabelTextSize
        {
            get => labelTextSize;
            set => Set(ref labelTextSize, value);
        }

        public SKTypeface Typeface
        {
            get => typeface;
            set => Set(ref typeface, value);
        }

        public Color BackgroundColor => Control.BackgroundColor;

        public Color LabelColor => Control.LabelColor;

        public IEnumerable<ChartEntry> Entries => Control.Entries;

        /// <summary>
        /// Gets or sets the minimum value from entries. If not defined, it will be the minimum between zero and the
        /// minimal entry value.
        /// </summary>
        /// <value>The minimum value.</value>
        public double MinValue
        {
            get
            {
                if (!Entries.Any())
                {
                    return 0;
                }

                if (InternalMinValue == null)
                {
                    return Math.Min(0, Entries.Min(x => x.Value));
                }

                return Math.Min(InternalMinValue.Value, Entries.Min(x => x.Value));
            }

            set => InternalMinValue = value;
        }

        /// <summary>
        /// Gets or sets the maximum value from entries. If not defined, it will be the maximum between zero and the
        /// maximum entry value.
        /// </summary>
        /// <value>The minimum value.</value>
        public double MaxValue
        {
            get
            {
                if (!Entries.Any())
                {
                    return 0;
                }

                if (InternalMaxValue == null)
                {
                    return Math.Max(0, Entries.Max(x => x.Value));
                }

                return Math.Max(InternalMaxValue.Value, Entries.Max(x => x.Value));
            }

            set => InternalMaxValue = value;
        }

        /// <summary>
        /// Gets or sets a value whether debug rectangles should be drawn.
        /// </summary>
        internal bool DrawDebugRectangles { get; private set; }

        /// <summary>
        /// Gets or sets the internal minimum value (that can be null).
        /// </summary>
        /// <value>The internal minimum value.</value>
        protected double? InternalMinValue
        {
            get => internalMinValue;
            set
            {
                if (Set(ref internalMinValue, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                }
            }
        }

        /// <summary>
        /// Gets or sets the internal max value (that can be null).
        /// </summary>
        /// <value>The internal max value.</value>
        protected double? InternalMaxValue
        {
            get => internalMaxValue;
            set
            {
                if (Set(ref internalMaxValue, value))
                {
                    RaisePropertyChanged(nameof(MaxValue));
                }
            }
        }

        /// <summary>
        /// Gets the drawable chart area (is set <see cref="BuildCaptionElements"/>).
        /// This is the total chart size minus the area allocated by caption elements.
        /// </summary>
        protected Rect DrawableChartArea { get; private set; }

        #endregion

        public override IUIElement Build()
        {
            int width = (int)Control.Width;
            int height = (int)Control.Height;

            ICanvas canvas = Canvas() .Width(width) .Height(height);

            DrawableChartArea = new Rect(0, 0, width, height);
            BuildContent(canvas, width, height);

            return canvas;
        }

        /// <summary>
        /// Draws the chart content.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public abstract void BuildContent(ICanvas canvas, int width, int height);

        /// <summary>
        /// Draws caption elements on the right or left side of the chart.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="entries">The entries.</param>
        /// <param name="isLeft">If set to <c>true</c> is left.</param>
        /// <param name="isGraphCentered">Should the chart in the center always?</param>
        protected void BuildCaptionElements(ICanvas canvas, int width, int height, List<ChartEntry> entries, bool isLeft, bool isGraphCentered)
        {
            var totalMargin = 2 * Margin;
            var availableHeight = height - (2 * totalMargin);
            var x = isLeft ? Margin : (width - Margin - LabelTextSize);
            var ySpace = (availableHeight - LabelTextSize) / ((entries.Count <= 1) ? 1 : entries.Count - 1);

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries.ElementAt(i);
                var y = totalMargin + (i * ySpace);
                if (entries.Count <= 1)
                {
                    y += (availableHeight - LabelTextSize) / 2;
                }

                var hasLabel = !string.IsNullOrEmpty(entry.Label);
                var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

                if (hasLabel || hasValueLabel)
                {
                    var captionMargin = LabelTextSize * 0.60f;
                    var captionX = isLeft ? Margin : width - Margin - LabelTextSize;
                    var valueColor = entry.Color.WithA((byte)(entry.Color.A * AnimationProgress));
                    var lblColor = entry.TextColor.WithA((byte)(entry.TextColor.A * AnimationProgress));
                    var rect = new Rect(captionX, y, LabelTextSize, LabelTextSize);

                    var rectangle = Rectangle()
                        .Fill(SolidColorBrush().Color(valueColor))
                        .Width(rect.Width)
                        .Height(rect.Height);

                    canvas.Add(rect.X, rect.Y, rectangle);

                    if (isLeft)
                    {
                        captionX += LabelTextSize + captionMargin;
                    }
                    else
                    {
                        captionX -= captionMargin;
                    }

                    canvas.BuildCaptionLabels(entry.Label, lblColor, UnicodeMode, UnicodeLanguage, entry.ValueLabel, valueColor, LabelTextSize,
                        new Point(captionX, y + (LabelTextSize / 2)), isLeft ? TextAlignment.Left : TextAlignment.Right, Typeface, out var labelBounds);
                    labelBounds.Union(rect);

#if LATER
                    if (DrawDebugRectangles)
                    {
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = entry.Color,
                            IsStroke = true
                        })
                        {
                            canvas.DrawRect(labelBounds, paint);
                        }
                    }
#endif

                    if (isLeft)
                    {
                        DrawableChartArea = new Rect(Math.Max(DrawableChartArea.Left, labelBounds.Right), 0, DrawableChartArea.Right, DrawableChartArea.Bottom);
                    }
                    else
                    {   // Draws the chart centered for right labelmode only
                        var left = isGraphCentered == true ? Math.Abs(width - DrawableChartArea.Right) : 0;
                        DrawableChartArea = new Rect(left, 0, Math.Min(DrawableChartArea.Right, labelBounds.Left), DrawableChartArea.Bottom);
                    }

                    if (entries.Count == 0 && isGraphCentered)
                    {
                        // Draws the chart centered if there isn't any left values
                        DrawableChartArea = new Rect(Math.Abs(width - DrawableChartArea.Right), 0, DrawableChartArea.Right, DrawableChartArea.Bottom);
                    }
                }
            }
        }

        /// <summary>
        /// Invoked whenever a property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AnimationProgress):
                    Invalidate();
                    break;
                case nameof(LabelTextSize):
                case nameof(MaxValue):
                case nameof(MinValue):
                case nameof(BackgroundColor):
                    PlanifyInvalidate();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Invalidate the chart.
        /// </summary>
        protected void Invalidate() => Invalidated?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Planifies the invalidation.
        /// </summary>
        protected async void PlanifyInvalidate()
        {
            if (invalidationPlanification != null)
            {
                await invalidationPlanification;
            }
            else
            {
                invalidationPlanification = Task.Delay(200);
                await invalidationPlanification;
                Invalidate();
                invalidationPlanification = null;
            }
        }

        /// <summary>
        /// Adds a weak event handler to observe invalidate changes.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <param name="onInvalidate">Callback when chart is invalidated.</param>
        /// <typeparam name="TTarget">The target subsriber type.</typeparam>
        public InvalidatedWeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target, Action<TTarget> onInvalidate)
            where TTarget : class
        {
            var weakHandler = new InvalidatedWeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subsribe();
            return weakHandler;
        }

        /// <summary>
        /// Animates the view.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="entrance">If set to <c>true</c> entrance.</param>
        /// <param name="token">Token.</param>
        public async Task AnimateAsync(bool entrance, CancellationToken token = default(CancellationToken))
        {
            var watch = new Stopwatch();

            var start = entrance ? 0 : 1;
            var end = entrance ? 1 : 0;
            var range = end - start;

            AnimationProgress = start;
            IsAnimating = true;

            watch.Start();

            var source = new TaskCompletionSource<bool>();
            var timer = Timer.Create();

            timer.Start(TimeSpan.FromSeconds(1.0 / 30), () =>
            {
                if (token.IsCancellationRequested)
                {
                    source.SetCanceled();
                    return false;
                }

                var progress = (float)(watch.Elapsed.TotalSeconds / animationDuration.TotalSeconds);
                progress = entrance ? Ease.In(progress) : Ease.Out(progress);
                AnimationProgress = start + (progress * (end - start));

                var shouldContinue = (entrance && AnimationProgress < 1) || (!entrance && AnimationProgress > 0);

                if (!shouldContinue)
                {
                    source.SetResult(true);
                }

                return shouldContinue;
            });

            await source.Task;

            watch.Stop();
            IsAnimating = false;
        }

#if false
        private async void UpdateEntries(IEnumerable<ChartEntry> value)
        {
            try
            {
                if (animationCancellation != null)
                {
                    animationCancellation.Cancel();
                }

                var cancellation = new CancellationTokenSource();
                animationCancellation = cancellation;

                if (!cancellation.Token.IsCancellationRequested && entries != null && IsAnimated)
                {
                    await AnimateAsync(false, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 0;
                }

                if (Set(ref entries, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                if (!cancellation.Token.IsCancellationRequested && entries != null && IsAnimated)
                {
                    await AnimateAsync(true, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 1;
                }
            }
            catch
            {
                if (Set(ref entries, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                Invalidate();
            }
            finally
            {
                animationCancellation = null;
            }
        }
#endif

        /// <summary>
        /// Raises the property change.
        /// </summary>
        /// <param name="property">The property name.</param>
        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Set the specified field and raise a property change if new value is different.
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="field">The field reference.</param>
        /// <param name="value">The new value.</param>
        /// <param name="property">The property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool Set<T>(ref T field, T value, [CallerMemberName]string property = null)
        {
            if (!EqualityComparer<T>.Equals(field, property))
            {
                field = value;
                RaisePropertyChanged(property);
                return true;
            }

            return false;
        }
    }
}
