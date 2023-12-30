using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rhizine.WPF.Converters;

/// <summary>
/// Converts a boolean value to and from Visibility. 
/// Can be reversed with the Reverse property or a parameter.
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether the conversion should be reversed.
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Converts a boolean value to a Visibility value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional parameter to override the Reverse property.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A Visibility value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = value is bool b && b;
        bool isReverse = Reverse ^ (parameter as bool? ?? false);

        return (isVisible ^ isReverse) ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a Visibility value back to a boolean value.
    /// </summary>
    /// <param name="value">The Visibility value to convert.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional parameter to override the Reverse property.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A boolean value.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is Visibility visibility))
            return DependencyProperty.UnsetValue;

        bool isReverse = Reverse ^ (parameter as bool? ?? false);
        return (visibility == Visibility.Visible) ^ isReverse;
    }
}
