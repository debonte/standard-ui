using System.StandardUI.Converters;
using System.ComponentModel;
using System.Globalization;

namespace System.StandardUI.Wpf.Converters
{
	public class PointsTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new PointsWpf(PointsConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
