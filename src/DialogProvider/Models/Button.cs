using System;
using Phoenix.UI.Wpf.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.DialogProvider.Models
{
	/// <summary>
	/// Model for a dialog button.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class Button
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> Flag signaling if the button is enabled or not. </summary>
		public bool IsEnabled { get; set; }
		private void OnIsEnabledChanged()
		{
			// Forward changes of the enabled property to the command.
			if (this.Command != null) this.Command.Executable = this.IsEnabled;
		}

		/// <summary> The buttons caption. </summary>
		public string Caption { get; set; }
		
		/// <summary> The command that will be executed when the button is pressed. </summary>
		public CommandHandler Command { get; }

		/// <summary> The <see cref="DialogButtonBehavior"/> of the button. </summary>
		public DialogButtonBehavior ButtonBehavior { get; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="buttonConfiguration"> The <see cref="ButtonConfiguration"/> of this button. </param>
		/// <param name="closeCallback"> The callback method used to close the dialog this button is being displayed in. </param>
		internal Button
		(
			ButtonConfiguration buttonConfiguration,
			Action<DialogResult> closeCallback
		)
		{
			// Save parameters.
			this.IsEnabled = buttonConfiguration.IsEnabled;
			this.IsEnabled = true;
			this.Caption = buttonConfiguration.Caption;
			this.ButtonBehavior = buttonConfiguration.ButtonBehavior;

			// Initialize fields.
			this.Command = new CommandHandler
			(
				() =>
				{
					var dialogResult = buttonConfiguration.Callback.Invoke();
					if (dialogResult != DialogResult.None) closeCallback?.Invoke(dialogResult);
				},
				this.IsEnabled
			);

			// Keep the enabled state up to date.
			buttonConfiguration.EnabledChanged += (sender, args) => this.IsEnabled = buttonConfiguration.IsEnabled;
		}

		#endregion

		#region Methods
		#endregion
	}
}