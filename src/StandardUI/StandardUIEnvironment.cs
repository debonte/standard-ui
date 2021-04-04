using System;

namespace Microsoft.StandardUI
{
    public static class StandardUIEnvironment
    {
        public static IStandardUIEnvironment Instance { get; private set; } = UnintializedStandardUIEnvironment.Instance;

        public static void Init(IStandardUIEnvironment environment)
        {
            if (!object.ReferenceEquals(Instance, UnintializedStandardUIEnvironment.Instance))
                throw new InvalidOperationException($"StandardUIEnviornment.Init already called. Current environment is of type {Instance.GetType()}");

            Instance = environment;

            Factory = environment.Factory;
        }

        public static IVisualEnvironment VisualEnvironment => Instance.VisualEnvironment;

        /// <summary>
        /// Cache the factory here as that's slightly more efficient than always fetching it on demand.
        /// </summary>
        public static IStandardUIFactory Factory { get; private set; } = new UnintializedStandardUIFactory();
    }
}
