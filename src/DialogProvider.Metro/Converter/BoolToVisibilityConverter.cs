#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Converter
{
	/// <summary>
	/// Checks if the bound <see cref="String"/> is null or whitespace and converts it into configurable <see cref="Visibility"/>.
	/// </summary>
	[System.Windows.Data.ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility))]
	internal class BoolToVisibilityConverter : SourceValueToVisibilityConverter<BoolToVisibilityConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var boolean = false;
			try
			{
				boolean = System.Convert.ToBoolean(value);
			}
			catch { /*ignore*/ }

			return boolean ? this.TrueVisibility : this.FalseVisibility;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
			{
				switch (visibility)
				{
					case Visibility.Visible: return true;
					default: return false;
				}
			}
			return false;
		}
	}
}