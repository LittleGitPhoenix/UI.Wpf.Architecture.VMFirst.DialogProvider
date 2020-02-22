#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Diverse options for the dialog itself.
	/// </summary>
	/// <remarks> Those settings may not be fully supported by all view assemblies. </remarks>
	[Flags]
	public enum DialogOptions
	{
		/// <summary> No special options. </summary>
		None = 0,
		
		/// <summary> Hides the transparency toggle in the dialog view. </summary>
		HideTransparencyToggle = 1 << 0,
		
		/// <summary> Hides the call stack trace of error dialogs. </summary>
		/// <remarks> This takes precedence over <see cref="AutoExpandStacktrace"/> </remarks>
		HideStacktrace = 1 << 1,
		
		/// <summary> Automatically expands the call stack trace of error dialogs. </summary>
		AutoExpandStacktrace = 1 << 2,
	}
}