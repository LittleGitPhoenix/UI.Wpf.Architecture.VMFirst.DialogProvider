#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels
{
	/// <summary>
	/// Interface for dialog container view models.
	/// </summary>
	public interface IDialogContainerViewModel
	{
		/// <summary> Should the transparency toggle be shown in the view (if it implements such functionality at all). </summary>
		bool ShowTransparencyToggle { get; set; }

		/// <summary> The <see cref="Visual"/> that will be displayed inside the dialog container. This could be a message- or exception view or something completely custom. </summary>
		Visual ContentView { get; set; }

		/// <summary> The <see cref="Button"/>s that the dialog will show. </summary>
		ICollection<Button> Buttons { get; set; }

		/// <summary>
		/// Handler method for the <see cref="UIElement.PreviewKeyUpEvent"/>.
		/// </summary>
		/// <param name="sender"> The <see cref="UIElement"/> that raised the event. </param>
		/// <param name="args"> The corresponding <see cref="System.Windows.Input.KeyEventArgs"/> </param>
		void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs args);
	}
}