using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI
{
    public static class StandardUIEnvironment
    {
        private static IStandardUIEnvironment _standardUIEnvironment;

        public static void Init(IStandardUIEnvironment environment)
        {
            _standardUIEnvironment = environment;
        }

        public static IStandardUIEnvironment Environment => _standardUIEnvironment!;

        public static IUIElementFactory UIElementFactory => _standardUIEnvironment.UIElementFactory;

        public static IMediaFactory MediaFactory => _standardUIEnvironment.MediaFactory;
    }
}
