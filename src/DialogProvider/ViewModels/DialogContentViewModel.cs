using System;
using Phoenix.UI.Wpf.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	/// <summary>
	/// Base class for view models that can be displayed within a dialog.
	/// </summary>
	public abstract class DialogContentViewModel : ICloseableViewModel
	{
		/// <inheritdoc />
		public Action<DialogResult> RequestClose { get; set; }

		/// <summary> Contains different options for the dialog. </summary>
		public DialogOptions DialogOptions { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dialogOptions"> Contains different options for the dialog. </param>
		protected DialogContentViewModel(DialogOptions dialogOptions)
		{
			this.DialogOptions = dialogOptions;
		}
	}
}