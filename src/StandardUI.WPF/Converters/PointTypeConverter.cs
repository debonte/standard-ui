using Microsoft.StandardUI.Converters;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.Wpf.Converters
{
	public class PointTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new PointWpf(PointConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
