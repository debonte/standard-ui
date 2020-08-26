using System.StandardUI.Media;
using System;

namespace System.StandardUI
{
    public static class StandardUIEnvironment
    {
        public static IStandardUIEnvironment Instance { get; private set; } = UnintializedStandardUIEnvironment.Instance;

        public static void Init(IStandardUIEnvironment environment)
        {
            if (!object.ReferenceEquals(Instance, UnintializedStandardUIEnvironment.Instance))
                throw new InvalidOperationException($"StandardUIEnviornment.Init already called. Current environment is of type {Instance.GetType()}");

            Instance = environment;
        }

        public static IUIElementFactory UIElementFactory => Instance.UIElementFactory;

        public static IMediaFactory MediaFactory => Instance.MediaFactory;

        public static IVisualEnvironment VisualEnvironment => Instance.VisualEnvironment;
    }
}
