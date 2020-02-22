#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Configuration of a <see cref="Button"/>.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class ButtonConfiguration
	{
		#region Delegates / Events

		internal event EventHandler EnabledChanged;
		private void OnEnabledChanged()
		{
			this.EnabledChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> Flag if this button is enabled. Default is <c>True</c>. </summary>
		public bool IsEnabled
		{
			get => _isEnabled;
			set
			{
				if (value == _isEnabled) return;
				_isEnabled = value;
				this.OnEnabledChanged();
			}
		}
		private bool _isEnabled;

		/// <summary> The <see cref="DialogButtonBehavior"/> of the button. </summary>
		public DialogButtonBehavior ButtonBehavior { get; }

		/// <summary> The buttons caption. </summary>
		internal string Caption { get; }

		/// <summary> Asynchronous callback that is invoke when the button is pressed. The return value determines, if the dialog will be closed afterwards, where <c>True</c> will close the dialog and <c>False</c> will not. </summary>
		internal Func<Task<DialogResult>> Callback { get;  }
		
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> this button has. If this is <see cref="Classes.DialogResult.None"/>, then the dialog will not be closed. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogResult dialogResult
		)
			: this
			(
				caption: caption,
				dialogResult: dialogResult,
				buttonBehavior: DialogButtonBehavior.None
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> this button has. If this is <see cref="DialogResult.None"/>, then the dialog will not be closed. </param>
		/// <param name="buttonBehavior"> The <see cref="DialogButtonBehavior"/> of the button. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogResult dialogResult,
			DialogButtonBehavior buttonBehavior
		)
			: this
			(
				caption: caption,
				dialogResult: dialogResult,
				buttonBehavior: buttonBehavior,
				callback: null
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> this button has. If this is <see cref="DialogResult.None"/>, then the dialog will not be closed. </param>
		/// <param name="callback"> Optional callback that is invoked when the button is pressed. The dialog will be closed afterwards with the specified <paramref name="dialogResult"/>. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogResult dialogResult,
			Action callback
		)
			: this
			(
				caption: caption,
				dialogResult: dialogResult,
				buttonBehavior: DialogButtonBehavior.None,
				callback: callback
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> this button has. If this is <see cref="DialogResult.None"/>, then the dialog will not be closed. </param>
		/// <param name="buttonBehavior"> The <see cref="DialogButtonBehavior"/> of the button. </param>
		/// <param name="callback"> Optional callback that is invoked when the button is pressed. The dialog will be closed afterwards with the specified <paramref name="dialogResult"/>. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogResult dialogResult,
			DialogButtonBehavior buttonBehavior,
			Action callback
		)
			: this
			(
				caption,
				buttonBehavior,
				() =>
				{
					return Task.Run
					(
						() =>
						{
							callback?.Invoke();
							return Task.FromResult(dialogResult);
						}
					);
				}
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="callback"> Optional callback that is invoked when the button is pressed. The returned <see cref="DialogResult"/> determines, if the dialog will be closed afterwards. <see cref="DialogResult.None"/> means the dialog stays open, everything else closes it. </param>
		public ButtonConfiguration
		(
			string caption,
			Func<DialogResult> callback
		)
			: this
			(
				caption: caption,
				buttonBehavior: DialogButtonBehavior.None,
				callback: () => Task.Run(callback.Invoke)
			)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="callback"> Optional asynchronous callback that is invoked when the button is pressed. The returned <see cref="DialogResult"/> determines, if the dialog will be closed afterwards. <see cref="DialogResult.None"/> means the dialog stays open, everything else closes it. </param>
		public ButtonConfiguration
		(
			string caption,
			Func<Task<DialogResult>> callback
		)
			: this
			(
				caption: caption,
				buttonBehavior: DialogButtonBehavior.None,
				callback: callback
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="buttonBehavior"> The <see cref="DialogButtonBehavior"/> of the button. </param>
		/// <param name="callback"> Optional callback that is invoked when the button is pressed. The returned <see cref="DialogResult"/> determines, if the dialog will be closed afterwards. <see cref="DialogResult.None"/> means the dialog stays open, everything else closes it. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogButtonBehavior buttonBehavior,
			Func<DialogResult> callback
		)
			: this
			(
				caption: caption,
				buttonBehavior: buttonBehavior,
				callback: () => Task.Run(callback.Invoke)
			)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="caption"> The buttons caption. </param>
		/// <param name="buttonBehavior"> The <see cref="DialogButtonBehavior"/> of the button. </param>
		/// <param name="callback"> Optional asynchronous callback that is invoked when the button is pressed. The returned <see cref="DialogResult"/> determines, if the dialog will be closed afterwards. <see cref="DialogResult.None"/> means the dialog stays open, everything else closes it. </param>
		public ButtonConfiguration
		(
			string caption,
			DialogButtonBehavior buttonBehavior,
			Func<Task<DialogResult>> callback
		)
		{
			// Save parameters.
			this.IsEnabled = true;
			this.Caption = caption;
			this.Callback = callback ?? throw new ArgumentNullException(nameof(callback));
			this.ButtonBehavior = buttonBehavior;
		}

		#endregion

		#region Methods
		#endregion
	}
}