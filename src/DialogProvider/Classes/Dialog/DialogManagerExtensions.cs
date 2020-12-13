#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Contains extension methods for showing dialogs.
	/// </summary>
	public static class DialogManagerExtensions
	{
		#region Messages

		/// <summary>
		/// Shows a message dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="contentViewModel"> The inner content (view model) of the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowMessage
		(
			this IDialogManager dialogManager,
			string title = null,
			string message = null,
			object contentViewModel = null,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowMessage(new MessageDialogModel(identifier: null, title: title, message: message, contentViewModel: contentViewModel), buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a message dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModel"> The <see cref="MessageDialogModel"/> of the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowMessage
		(
			this IDialogManager dialogManager,
			MessageDialogModel messageModel,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowMessage(new[] { messageModel }, buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a message dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModels"> A collection of <see cref="MessageDialogModel"/>s for the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowMessage
		(
			this IDialogManager dialogManager,
			ICollection<MessageDialogModel> messageModels,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowMessage(messageModels, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a message dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModels"> A collection of <see cref="MessageDialogModel"/>s for the dialog. </param>
		/// <param name="buttonConfigurations"> A collection of custom <see cref="ButtonConfiguration"/> used to display buttons. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowMessage
		(
			this IDialogManager dialogManager,
			ICollection<MessageDialogModel> messageModels,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowMessage(new MessageDialogViewModel(messageModels), buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		#endregion

		#region Warnings

		/// <summary>
		/// Shows a warning dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="contentViewModel"> The inner content (view model) of the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowWarning
		(
			this IDialogManager dialogManager,
			string title = null,
			string message = null,
			object contentViewModel = null,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowWarning(new MessageDialogModel(identifier: null, title: title, message: message, contentViewModel: contentViewModel), buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a warning dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModel"> The <see cref="MessageDialogModel"/> of the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowWarning
		(
			this IDialogManager dialogManager,
			MessageDialogModel messageModel,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowWarning(new[] { messageModel }, buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a warning dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModels"> A collection of <see cref="MessageDialogModel"/>s for the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.Ok"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowWarning
		(
			this IDialogManager dialogManager,
			ICollection<MessageDialogModel> messageModels,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowWarning(messageModels, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows a warning dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="messageModels"> A collection of <see cref="MessageDialogModel"/>s for the dialog. </param>
		/// <param name="buttonConfigurations"> A collection of custom <see cref="ButtonConfiguration"/> used to display buttons. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowWarning
		(
			this IDialogManager dialogManager,
			ICollection<MessageDialogModel> messageModels,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowMessage(new WarningDialogViewModel(messageModels), buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		#endregion

		#region Exceptions

		/// <summary>
		/// Shows an exception dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowException
		(
			this IDialogManager dialogManager,
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowExceptions(new Exception[0], title, message, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <summary>
		/// Shows an exception dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="exception"> The <see cref="Exception"/> of the dialog. If this is an <see cref="AggregateException"/>, then the <see cref="AggregateException.InnerExceptions"/> collection will be used. </param>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.None"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowException
		(
			this IDialogManager dialogManager,
			Exception exception,
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowExceptions(exception is AggregateException aggregateException ? aggregateException.InnerExceptions.ToArray() : new[] { exception }, title, message, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		#endregion

		#region Content

		/// <summary>
		/// Shows a content dialog.
		/// </summary>
		/// <param name="dialogManager"> The extended <see cref="IDialogManager"/>. </param>
		/// <param name="viewModel"> The content (view model) of the dialog. </param>
		/// <param name="buttons"> The <see cref="DialogButtons"/> to display. Default is <see cref="DialogButtons.None"/>. </param>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of the dialog. Default is <see cref="DialogDisplayLocation.Window"/>. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> of the dialog. Default is <see cref="DialogDisplayBehavior.Show"/>. </param>
		/// <param name="dialogOptions"> The <see cref="DialogOptions"/> of the dialog. Default is <see cref="DialogOptions.HideTransparencyToggle"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> An awaitable <see cref="DialogTask"/>. </returns>
		public static DialogTask ShowContent
		(
			this IDialogManager dialogManager,
			object viewModel,
			DialogButtons buttons = DialogButtons.None,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.HideTransparencyToggle,
			CancellationToken cancellationToken = default
		)
			=> dialogManager.ShowContent(viewModel, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);
		
		#endregion
	}
}