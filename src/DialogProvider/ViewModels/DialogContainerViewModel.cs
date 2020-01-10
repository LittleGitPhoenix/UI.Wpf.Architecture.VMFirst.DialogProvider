#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Phoenix.UI.Wpf.DialogProvider.Classes;
using Phoenix.UI.Wpf.DialogProvider.Models;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	internal class DialogContainerViewModel : IDialogContainerViewModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <inheritdoc />
		public bool ShowTransparencyToggle { get; set; }

		/// <inheritdoc />
		public Visual ContentView { get; set; }
		
		/// <inheritdoc />
		public ICollection<Button> Buttons { get; set; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public DialogContainerViewModel()
		{
			// Save parameters.

			// Initialize fields.
			this.ShowTransparencyToggle = true;
		}

		#endregion

		#region Methods
		
		/// <inheritdoc />
		public void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs args)
		{
			CommandHandler command = null;

			switch (args.Key)
			{
				case System.Windows.Input.Key.Escape:
				{
					command = this.GetCommandForBehavior(DialogButtonBehavior.Cancel);
					break;
				}
				case System.Windows.Input.Key.Enter:
				{
					command = this.GetCommandForBehavior(DialogButtonBehavior.Enter);
					break;
				}
			}

			if (command != null)
			{
				args.Handled = true;
				command.Execute(null);
			}
		}

		/// <summary>
		/// Gets the <see cref="CommandHandler"/> for the enabled button with the <paramref name="buttonBehavior"/>.
		/// </summary>
		/// <param name="buttonBehavior"> The <see cref="DialogButtonBehavior"/> the <see cref="Button"/> must match. </param>
		/// <returns> The matching <see cref="CommandHandler"/> or <c>Null</c>. </returns>
		private CommandHandler GetCommandForBehavior(DialogButtonBehavior buttonBehavior)
		{
			var matchingButtons = this.Buttons
				.Where(button => button.IsEnabled && button.ButtonBehavior == buttonBehavior)
				.ToArray()
				;

			if (matchingButtons.Length == 0)
			{
				System.Diagnostics.Debug.WriteLine($"{this.GetType().Name.ToUpper()}:: No button matches the behavior '{buttonBehavior}'.");
				return null;
			}
			else if (matchingButtons.Length > 1)
			{
				System.Diagnostics.Trace.WriteLine($"{this.GetType().Name.ToUpper()}:: More then one button matches the behavior '{buttonBehavior}'. Therefore nothing will be executed.");
				return null;
			}
			else
			{
				return matchingButtons.First().Command;
			}
		}

		#endregion
	}
}