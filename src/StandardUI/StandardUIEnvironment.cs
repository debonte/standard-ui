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

        public static IVisualEnvironment VisualEnvironment => Instance.VisualEnvironment;

        public static IStandardUIFactory Factory => Instance.Factory;
    }
}
