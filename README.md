# .NET Standard UI

.NET Standard UI enables building UI, especially controls, that runs across multiple UI frameworks - WPF, UWP, WinUI, Xamarin.Forms, .NET MAUI, Uno, Avalonia, and more. A .NET Standard UI control is a single .NET Standard assembly, that works on all supported UI frameworks.

.NET Standard UI is similar in some ways to [XAML Standard](https://github.com/microsoft/xaml-standard), but this is a binary standard, not just aligned naming conventions. A binary standard is much more useful, at it allows writing shared code. The standard objects can be used
from XAML or from code.

The standard includes APIs needed to create controls that use drawn UI - shapes APIs (for the drawing), visual states, layout (e.g. Grid and StackPanel), core input event handling, and core accessibility support. It may grow in the future, but that's the initial scope.

API names most closely match UWP/WinUI, but should be familiar to WPF and Xamarin.Forms develepers as they are pretty similar.

# Architecture and APIs

The API is interface based. For instance, an ellipse is `Microsoft.StandardUI.Shapes.IEllipse`. Users of the API always use the interface.

In terms of implementation, UI platforms can implement the interface directly OR it can be implemented by a wrapper object (which typically lives in this repo). Having both options available provides maximum flexibility.

For new UI platforms, like WinUI3 and .NET MAUI, ideally they have their native
`Ellipse` object implement `IEllipse` directly. That helps enforce API naming consistency and is slightly more efficient.

Or the interface can be implemented via a wrapper, which requires no changes to the underlying UI platform at all. That's a good choice for platforms like WPF.

The API interfaces are all defined [here](src/StandardUI). Implementations for the different UI frameworks are created through a mix of [code generation](src/StandardUI.CodeGenerator) from those interfaces and hand coding.

This project is an evolution of my [XGraphics](https://github.com/BretJohnson/XGraphics) project, taking it beyond just shapes.

# Current APIs

### Shapes and Drawing

_Shapes:_
[IShape](StandardUI/Shapes/IShape.cs),
[IEllipse](StandardUI/Shapes/IEllipse.cs),
[ILine](StandardUI/Shapes/ILine.cs),
[IPath](StandardUI/Shapes/IPath.cs),
[IPolygon](StandardUI/Shapes/IPolygon.cs),
[IPolyline](StandardUI/Shapes/IPolyline.cs),
[IRectangle](StandardUI/Shapes/IRectangle.cs)

_Geometries:_
[IGeometry](StandardUI/Media/IGeometry.cs),
[IArcSegement](StandardUI/Media/IArcSegement.cs),
[IBezierSegment](StandardUI/Media/IBezierSegment.cs),
[ILineSegment](StandardUI/Media/ILineSegment.cs),
[IPathFigure](StandardUI/Media/IPathFigure.cs),
[IPathGeometry](StandardUI/Media/IPathGeometry.cs),
[IPathSegment](StandardUI/Media/IPathSegment.cs),
[IPolyBezierSegment](StandardUI/Media/IPolyBezierSegment.cs)
[IPolyQuadraticBezierSegment](StandardUI/Media/IPolyQuadraticBezierSegment.cs)
[IQuadraticBezierSegment](StandardUI/Media/IQuadraticBezierSegment.cs)

_Transforms:_
[ITransform](StandardUI/Media/ITransform.cs),
[IRotateTransform](StandardUI/Media/IRotateTransform.cs),
[IScaleTransform](StandardUI/Media/IScaleTransform.cs),
[ITransformGroup](StandardUI/Media/ITransformGroup.cs),
[ITranslateTransform](StandardUI/Media/ITranslateTransform.cs)

_Brushes and Strokes:_
[BrushMappingMode](StandardUI/Media/BrushMappingMode.cs),
[FillMode](StandardUI/Media/FillMode.cs),
[GradientStreamMethod](StandardUI/Media/GradientStreamMethod.cs),
[IGradientBrush](StandardUI/Media/IGradientBrush.cs),
[ILinearGradientBrush](StandardUI/Media/ILinearGradientBrush.cs),
[IRadialGradientBrush](StandardUI/Media/IRadialGradientBrush.cs),
[ISolidColorBrush](StandardUI/Media/ISolidColorBrush.cs),
[PenLineCap](StandardUI/Media/PenLineCap.cs),
[PenLineJoin](StandardUI/Media/PenLineJoin.cs),
[SweepDirection](StandardUI/Media/SweepDirection.cs)

All of these APIs are nearly identical to UWP, WPF, and Xamarin.Forms 4.8 (which added shape and brush support).

Shapes are [IUIElements](StandardUI/IUIElement.cs) that can be used as children to build the visual representation of a control, often as part of a control template. That's the same model used by UWP/WPF/Forms.

Geometries, transforms, and brushes all help support the drawing.

### Layout

[IPanel](StandardUI/Controls/IPanel.cs),
[IStackPanel](StandardUI/Controls/IStackPanel.cs),
[IGrid](StandardUI/Controls/IGrid.cs),
[ICanvas](StandardUI/Controls/ICanvas.cs)

### Text

[ITextBlock](StandardUI/Controls/ITextBlock.cs),
[FontStyle](StandardUI/Text/FontStyle.cs),
[FontWeight](StandardUI/Text/FontWeight.cs),
[FontWeights](StandardUI/Text/FontWeights.cs)

Text output is another core piece of drawn UI.

### Control hierarchy

[IUIElement](StandardUI/IUIElement.cs),
[IUIElementCollection](StandardUI/Controls/IUIElementCollection.cs),
[IControl](StandardUI/Controls/IControl.cs),
[IUserControl](StandardUI/Controls/IUserControl.cs)


