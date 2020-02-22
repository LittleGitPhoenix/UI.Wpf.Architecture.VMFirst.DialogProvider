#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Converter
{
	/// <summary>
	/// Checks if the bound property is <c>NULL</c>.
	/// </summary>
	[System.Windows.Data.ValueConversion(sourceType: typeof(object), targetType: typeof(bool))]
	internal class IsNullConverter : SourceValueConverter<IsNullConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value == null);
		}
	}

	/// <summary>
	/// Checks if the bound <see cref="Object"/> is <c>NULL</c> and converts it into configurable <see cref="Visibility"/>.
	/// </summary>
	[System.Windows.Data.ValueConversion(sourceType: typeof(object), targetType: typeof(Visibility))]
	internal class IsNullToVisibilityConverter : InverseSourceValueToVisibilityConverter<IsNullToVisibilityConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is null ? this.TrueVisibility : this.FalseVisibility;
		}
	}
}