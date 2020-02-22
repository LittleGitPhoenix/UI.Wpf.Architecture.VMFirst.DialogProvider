#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections;
using System.Globalization;
using System.Windows;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Converter
{
	/// <summary> 
	/// Checks if the bound <see cref="ICollection"/> contains elements. If the bound property is <c>NULL</c> or it could not be cast then <c>FALSE</c> will be returned.
	/// </summary>
	[System.Windows.Data.ValueConversion(sourceType: typeof(ICollection), targetType: typeof(bool))]
	internal class HasElementsConverter : SourceValueConverter<HasElementsConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is ICollection collection)) return false;
			return collection.Count > 0;
		}
	}

	/// <summary>
	/// Checks if the bound <see cref="ICollection"/> contains elements and converts it into configurable <see cref="Visibility"/>.
	/// </summary>
	[System.Windows.Data.ValueConversion(sourceType: typeof(ICollection), targetType: typeof(Visibility))]
	internal class HasElementsToVisibilityConverter : SourceValueToVisibilityConverter<HasElementsToVisibilityConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is ICollection collection)) return base.FalseVisibility;
			return collection.Count > 0 ? this.TrueVisibility : base.FalseVisibility;
		}
	}
}