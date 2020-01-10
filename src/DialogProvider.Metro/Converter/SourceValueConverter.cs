using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Phoenix.UI.Wpf.DialogProvider.Metro.Converter
{
	/// <summary>
	/// Base class for custom converters.
	/// </summary>
	/// <example>
	/// 
	/// Reference the assembly:
	/// -----------------------
	/// xmlns:converter="clr-namespace:Phoenix.Base.UI.Wpf.Converters;assembly=Phoenix.Base.Wpf"
	/// 
	/// Use converter in trigger:
	/// -------------------------
	/// <DataTrigger Binding="{Binding [BINDING_PROPERTY], Converter={x:Static converters:IsGreaterThanConverter.Instance}, ConverterParameter=0}" Value="true">
	///		<Setter Property = "[PROPERTY]" Value="[VALUE]"/>
	/// </DataTrigger>
	/// 
	/// </example>
	internal abstract class SourceValueConverter<T> : MarkupExtension, IValueConverter
		where T : SourceValueConverter<T>, new()
	{
		public static readonly IValueConverter Instance = new T();

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
		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		/// <summary>
		/// Converts a <paramref name="value"/> back.
		/// </summary>
		/// <param name="value"> The value that is produced by the binding target. </param>
		/// <param name="targetType"> The type to convert to. </param>
		/// <param name="parameter"> The converter parameter to use. </param>
		/// <param name="culture"> The culture to use in the converter. </param>
		/// <returns> The back converted <paramref name="value"/>. </returns>
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException($"{this.GetType().Name} can only be used one way.");
		}
	}

	/// <summary>
	/// Checks if the bound <c>value</c> for a condition and converts it into configurable <see cref="Visibility"/>.
	/// </summary>
	internal abstract class SourceValueToVisibilityConverter<T> : SourceValueConverter<T>
		where T : SourceValueToVisibilityConverter<T>, new()
	{
		/// <summary> Evaluates <c>TRUE</c> to <see cref="Visibility.Collapsed"/> and <c>FALSE</c> to <see cref="Visibility.Visible"/>. </summary>
		public static readonly IValueConverter InverseInstance = new T() { TrueVisibility = Visibility.Collapsed, FalseVisibility = Visibility.Visible };

		public virtual Visibility TrueVisibility { get; set; } = Visibility.Visible;

		public virtual Visibility FalseVisibility { get; set; } = Visibility.Collapsed;
	}

	/// <summary>
	/// Checks if the bound <c>value</c> for a condition and converts it into configurable <see cref="Visibility"/>.
	/// </summary>
	internal abstract class InverseSourceValueToVisibilityConverter<T> : SourceValueToVisibilityConverter<T>
		where T : SourceValueToVisibilityConverter<T>, new()
	{
		/// <summary> Evaluates <c>TRUE</c> to <see cref="Visibility.Visible"/> and <c>FALSE</c> to <see cref="Visibility.Collapsed"/>. </summary>
		public new static readonly IValueConverter InverseInstance = new T() { TrueVisibility = Visibility.Visible, FalseVisibility = Visibility.Collapsed };

		public override Visibility TrueVisibility { get; set; } = Visibility.Collapsed;

		public override Visibility FalseVisibility { get; set; } = Visibility.Visible;
	}
}