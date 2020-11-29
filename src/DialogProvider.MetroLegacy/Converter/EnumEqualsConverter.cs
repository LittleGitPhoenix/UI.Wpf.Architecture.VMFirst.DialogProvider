#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Converter
{
	/// <summary>
	/// Checks if the bound <see cref="Enum"/> value matches the <see cref="Enum"/> value of the <c>ConverterParameter</c>.
	/// </summary>
	/// <remarks> Works for both normal and flags enumerations. </remarks>
	internal class EnumEqualsConverter : SourceValueConverter<EnumEqualsConverter>
	{
		/// <inheritdoc />
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return EnumEqualsConverter.CheckEnumValuesEquality(value, parameter);
		}

		internal static bool CheckEnumValuesEquality(object value, object parameter)
		{
			// Check if the passed value is an enumeration.
			if (!(value is Enum valueEnum)) throw new NotSupportedException($"The passed value '{value?.GetType() ?? null}' must be of type '{typeof(Enum)}'.");

			// Check if the parameter is an enumeration.
			if (!(parameter is Enum parameterEnum)) throw new NotSupportedException($"The passed parameter '{parameter?.GetType() ?? null}' must be of type '{typeof(Enum)}'.");

			// Get the types of both enumerations.
			Type valueType = valueEnum.GetType();
			Type parameterType = parameterEnum.GetType();

			// Check if the enumeration is a flags enumeration.
			/*
			 * Testing Enum.Equals against Enum.HasFlag for normal enumeration shows, that Enum.HasFlag works there too.
			 * But as the MSDN documentation states:
			 * The HasFlag method is designed to be used with enumeration types that are marked with the FlagsAttribute attribute
			 * and can be used to determine whether multiple bit fields are set.
			 * For enumeration types that are not marked with the FlagsAttribute attribute, call either the Equals method or the CompareTo method.
			 */
			if (valueType.IsDefined(attributeType: typeof(FlagsAttribute), inherit: false))
			{
				try
				{
					// Check if the flag is set.
					return valueEnum.HasFlag(parameterEnum);
				}
				catch (ArgumentException)
				{
					if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
					{
						// This sometimes happens in the designer, so use the crude method.
						var actual = System.Convert.ToInt64(valueEnum);
						var target = System.Convert.ToInt64(parameterEnum);

						return (actual & target) != 0;
					}
					else
					{
						throw new System.Data.DataException($"The enumerations passed as value and parameter must be of the same type, but value is '{valueType}' and parameter is '{parameterType}'.");
					}
				}
			}
			else
			{
				try
				{
					// Check if the value matches.
					return valueEnum.CompareTo(parameterEnum) == 0;
				}
				catch (ArgumentException)
				{
					if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
					{
						// This sometimes happens in the designer, so use the crude method.
						var actual = System.Convert.ToInt64(valueEnum);
						var target = System.Convert.ToInt64(parameterEnum);

						return actual == target;
					}
					else
					{
						throw new System.Data.DataException($"The enumerations passed as value and parameter must be of the same type, but value is '{valueType}' and parameter is '{parameterType}'.");
					}
				}
			}
		}
	}

	/// <summary>
	/// Checks if the bound <see cref="Enum"/> value does not match the <c>ConverterParameter</c>.
	/// </summary>
	internal class EnumNotEqualsConverter : SourceValueConverter<EnumNotEqualsConverter>
	{
		/// <inheritdoc />
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !EnumEqualsConverter.CheckEnumValuesEquality(value, parameter);
		}
	}

	/// <summary>
	/// Checks if the bound <see cref="Enum"/> value matches the <c>ConverterParameter</c> and converts it into configurable <see cref="System.Windows.Visibility"/>.
	/// </summary>
	internal class EnumEqualsToVisibilityConverter : SourceValueToVisibilityConverter<EnumEqualsToVisibilityConverter>
	{
		/// <inheritdoc />
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return EnumEqualsConverter.CheckEnumValuesEquality(value, parameter) ? base.TrueVisibility : base.FalseVisibility;
		}
	}
}