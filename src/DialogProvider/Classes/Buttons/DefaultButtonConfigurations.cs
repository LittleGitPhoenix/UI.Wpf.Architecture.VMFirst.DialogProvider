#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Collections.Generic;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Resources.Globalization;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Contains predefined <see cref="ButtonConfiguration"/>s for common use cases.
	/// </summary>
	public static class DefaultButtonConfigurations
	{
		/// <summary>
		/// Represent a <c>Ok</c> button.
		/// </summary>
		public static ButtonConfiguration OkButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.Ok,
			dialogResult: DialogResult.Yes,
			buttonBehavior: DialogButtonBehavior.Enter,
			callback: null
		);

		/// <summary>
		/// Represent a <c>Cancel</c> button.
		/// </summary>
		public static ButtonConfiguration CancelButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.Cancel,
			dialogResult: DialogResult.No,
			buttonBehavior: DialogButtonBehavior.Cancel,
			callback: null
		);

		/// <summary>
		/// Represent a <c>Yes</c> button.
		/// </summary>
		public static ButtonConfiguration YesButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.Yes,
			dialogResult: DialogResult.Yes,
			buttonBehavior: DialogButtonBehavior.None,
			callback: null
		);

		/// <summary>
		/// Represent a <c>No</c> button.
		/// </summary>
		public static ButtonConfiguration NoButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.No,
			dialogResult: DialogResult.No,
			buttonBehavior: DialogButtonBehavior.None,
			callback: null
		);
		
		/// <summary>
		/// Represent a <c>Save</c> button.
		/// </summary>
		public static ButtonConfiguration SaveButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.Save,
			dialogResult: DialogResult.Yes,
			buttonBehavior: DialogButtonBehavior.Enter,
			callback: null
		);

		/// <summary>
		/// Represent a <c>Close</c> button.
		/// </summary>
		public static ButtonConfiguration CloseButtonConfiguration { get; } = new ButtonConfiguration
		(
			caption: g11n.Close,
			dialogResult: DialogResult.No,
			buttonBehavior: DialogButtonBehavior.Cancel,
			callback: null
		);

		/// <summary>
		/// Gets the <see cref="ButtonConfiguration"/>s for the specified <paramref name="buttons"/>.
		/// </summary>
		/// <param name="buttons"> The <see cref="DialogButtons"/> that should be displayed. </param>
		/// <returns> A collection of <see cref="ButtonConfiguration"/>. </returns>
		/// <remarks> To have no buttons the only flag has to be <see cref="DialogButtons.None"/>. If other flags are set, then the respective buttons will be shown. </remarks>
		internal static IEnumerable<ButtonConfiguration> GetConfiguration(DialogButtons buttons)
		{
			if (buttons.Equals(DialogButtons.None)) yield break;

			if (buttons.HasFlag(DialogButtons.Ok)) yield return DefaultButtonConfigurations.OkButtonConfiguration;
			if (buttons.HasFlag(DialogButtons.Cancel)) yield return DefaultButtonConfigurations.CancelButtonConfiguration;
			if (buttons.HasFlag(DialogButtons.Yes)) yield return DefaultButtonConfigurations.YesButtonConfiguration;
			if (buttons.HasFlag(DialogButtons.No)) yield return DefaultButtonConfigurations.NoButtonConfiguration;
			if (buttons.HasFlag(DialogButtons.Save)) yield return DefaultButtonConfigurations.SaveButtonConfiguration;
			if (buttons.HasFlag(DialogButtons.Close)) yield return DefaultButtonConfigurations.CloseButtonConfiguration;
		}
	}
}