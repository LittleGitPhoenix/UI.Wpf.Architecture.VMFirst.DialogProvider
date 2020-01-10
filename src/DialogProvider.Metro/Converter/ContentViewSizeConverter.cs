using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Phoenix.UI.Wpf.DialogProvider.Metro.Converter
{
	internal class ContentViewSizeConverter : System.Windows.Markup.MarkupExtension, IValueConverter
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		/// <summary>
		/// Converts a <paramref name="value"/>.
		/// </summary>
		/// <param name="value"> The value produced by the binding source. </param>
		/// <param name="targetType"> The type of the binding target property. </param>
		/// <param name="parameter"> The converter parameter to use. </param>
		/// <param name="culture"> The culture to use in the converter. </param>
		/// <returns> The converted <paramref name="value"/>. </returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			// Parse the dimension.
			if (!(value is double actual)) return System.Windows.DependencyProperty.UnsetValue;

			// Parse the factor.
			if (parameter is null || !double.TryParse(parameter.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, culture, out var factor)) return actual;

			// Multiply.
			//Debug.WriteLine($"New value: {actual * factor}");
			return actual * factor;
		}

		/// <summary>
		/// Converts a <paramref name="value"/> back.
		/// </summary>
		/// <param name="value"> The value that is produced by the binding target. </param>
		/// <param name="targetType"> The type to convert to. </param>
		/// <param name="parameter"> The converter parameter to use. </param>
		/// <param name="culture"> The culture to use in the converter. </param>
		/// <returns> The back converted <paramref name="value"/>. </returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new InvalidOperationException($"{this.GetType().Name} can only be used one way.");
		}
	}
}