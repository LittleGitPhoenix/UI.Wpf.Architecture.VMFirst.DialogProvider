﻿#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Threading;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Interface for dialog managers that are responsible for showing dialogs.
	/// </summary>
	public interface IDialogManager
	{
		#region Initialization

		/// <summary>  Flag if the instance is fully initialized. </summary>
		bool IsInitialized { get; }

		/// <summary>
		/// Initializes the current instance with the <paramref name="view"/> needed to display a dialog.
		/// </summary>
		/// <param name="view"> The <see cref="System.Windows.FrameworkElement"/> that will be used to display a dialog. </param>
		/// <returns> The same instance of <see cref="IDialogManager"/> that was just initialized. This is useful for chaining. </returns>
		IDialogManager Initialize(System.Windows.FrameworkElement view);

		#endregion

		#region Show

		/// <summary>
		/// Shows a message dialog.
		/// </summary>
		/// <param name="viewModel"> The <see cref="MessageDialogModel"/> of the dialog. </param>
		/// <param name="buttonConfigurations"> A collection of custom <see cref="ButtonConfiguration"/> used to display buttons. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public DialogTask ShowMessage
		(
			MessageDialogViewModel viewModel,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation,
			DialogDisplayBehavior displayBehavior,
			DialogOptions dialogOptions,
			CancellationToken cancellationToken = default
		);

		/// <summary>
		/// Shows dialog with multiple exceptions.
		/// </summary>
		/// <param name="exceptions"> A collection of <see cref="Exception"/>s for the dialog. </param>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		DialogTask ShowExceptions
		(
			ICollection<Exception> exceptions,
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		);

		/// <summary>
		/// Shows a content dialog.
		/// </summary>
		/// <param name="viewModel"> The content (view model) of the dialog. </param>
		/// <param name="buttonConfigurations"> A collection of custom <see cref="ButtonConfiguration"/> used to display buttons. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.HideTransparencyToggle"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		DialogTask ShowContent
		(
			object viewModel,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.HideTransparencyToggle,
			CancellationToken cancellationToken = default
		);

		#endregion
		
		#region Close

		/// <summary>
		/// Removes the top most dialog in all <see cref="DialogDisplayLocation"/>s and shows the next one or closes the overlay.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of the revoked dialogs. </param>
		void Revoke(DialogResult dialogResult);

		/// <summary>
		/// Removes the top most dialog of the <paramref name="displayLocation"/> and shows the next one or closes the overlay.
		/// </summary>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog to revoke. </param>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of the revoked dialog. </param>
		void Revoke(DialogDisplayLocation displayLocation, DialogResult dialogResult);

		/// <summary>
		/// Closes all currently shown dialogs in all <see cref="DialogDisplayLocation"/>s.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of the topmost dialog. All other will have <see cref="DialogResult.Killed"/>. </param>
		void Close(DialogResult dialogResult);

		/// <summary>
		/// Closes all dialogs currently shown in the <paramref name="displayLocation"/>.
		/// </summary>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialogs to close. </param>
		/// /// <param name="dialogResult"> The <see cref="DialogResult"/> of the topmost dialog. All other will have <see cref="DialogResult.Killed"/>. </param>
		void Close(DialogDisplayLocation displayLocation, DialogResult dialogResult);

		#endregion
	}
}