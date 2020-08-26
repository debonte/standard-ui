using System.StandardUI.Media;
using System;

namespace System.StandardUI
{
    public class UnintializedStandardUIEnvironment : IStandardUIEnvironment
    {
        public static UnintializedStandardUIEnvironment Instance = new UnintializedStandardUIEnvironment();

        public IVisualEnvironment VisualEnvironment => throw new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");

        public IUIElementFactory UIElementFactory => throw new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");

        public IMediaFactory MediaFactory => throw new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");
    }
}
