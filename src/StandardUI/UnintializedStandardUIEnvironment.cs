using System.StandardUI.Media;
using System;

namespace System.StandardUI
{
    public class UnintializedStandardUIEnvironment : IStandardUIEnvironment
    {
        public static UnintializedStandardUIEnvironment Instance = new UnintializedStandardUIEnvironment();

        public IVisualEnvironment VisualEnvironment => throw new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");

        public IStandardUIFactory Factory => throw new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");
    }
}
