#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Defines which default buttons the dialog will show. Will be converted to <see cref="ButtonConfiguration"/>s
	/// </summary>
	[Flags]
	public enum DialogButtons
	{
		/// <summary> No buttons should be shown. </summary>
		/// <remarks> To apply this, no other flags have to be set. </remarks>
		None = 0,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.YesButtonConfiguration"/>. </summary>
		Yes = 1 << 0,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.NoButtonConfiguration"/>. </summary>
		No = 1 << 1,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.OkButtonConfiguration"/>. </summary>
		Ok = 1 << 2,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.CancelButtonConfiguration"/>. </summary>
		Cancel = 1 << 3,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.SaveButtonConfiguration"/>. </summary>
		Save = 1 << 4,
		/// <summary> Represent the <see cref="DefaultButtonConfigurations.CloseButtonConfiguration"/>. </summary>
		Close = 1 << 5,
	}
}