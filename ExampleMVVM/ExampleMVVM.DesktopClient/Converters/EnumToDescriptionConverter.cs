using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ExampleMVVM.DesktopClient.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum enumValue))
            {
                return DependencyProperty.UnsetValue;
            }

            return GetDescription(enumValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private string GetDescription(Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var fieldInfo = enumType.GetField(enumValue.ToString());

            return !(Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute))
                is DescriptionAttribute descriptionAttribute)
                ? enumValue.ToString()
                : descriptionAttribute.Description;
        }
    }
}
