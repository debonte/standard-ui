namespace Microsoft.StandardUI
{
    public interface IStandardUIEnvironment
    {
        [DefaultValue(double.NaN)]
        double Width { get; }

        [DefaultValue(double.NaN)]
        double Height { get; }
    }
}
