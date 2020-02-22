#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels
{
	/// <summary>
	/// Base class for view models that can be displayed within a dialog.
	/// </summary>
	public abstract class DialogContentViewModel : ICloseableDialogContentViewModel, IOptionsAwareDialogContentViewModel
	{
		/// <inheritdoc />
		public Action<DialogResult> RequestClose { get; internal set; }

		/// <inheritdoc />
		public DialogOptions DialogOptions { get; internal set; }
		
		/// <summary>
		/// Constructor
		/// </summary>
		protected DialogContentViewModel() { }
	}
}