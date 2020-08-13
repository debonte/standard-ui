using Microsoft.StandardUI.Converters;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.Wpf.Converters
{
	public class SizeTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new SizeWpf(SizeConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
