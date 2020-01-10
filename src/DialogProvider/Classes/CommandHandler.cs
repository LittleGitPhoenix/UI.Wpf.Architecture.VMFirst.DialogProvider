using System;
using System.Windows.Input;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// <see cref="ICommand"/> implementation for fixed <see cref="Action"/>s.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class CommandHandler : ICommand
	{
		#region Delegates / Events

		/// <inheritdoc />
		public event EventHandler CanExecuteChanged;
		/// <summary>
		/// Should be called when changes occur that affect whether or not the command should execute.
		/// </summary>
		protected virtual void OnCanExecuteChanged()
		{
			this.CanExecuteChanged?.Invoke(this, new EventArgs());
		}

		#endregion

		#region Constants
		#endregion

		#region Fields

		/// <summary> The action that will be invoked when the command is executed. </summary>
		private readonly Action _action;
		
		#endregion

		#region Properties
		
		/// <summary> Flag the signals if the command can be executed. </summary>
		public bool Executable { get; set; }
		private void OnExecutableChanged()
		{
			if (System.Windows.Application.Current.Dispatcher.CheckAccess())
			{
				this.OnCanExecuteChanged();
			}
			else
			{
				System.Windows.Application.Current.Dispatcher.Invoke(this.OnCanExecuteChanged);
			}
		}

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="action"> The action that will be invoked when the command is executed. </param>
		/// <param name="executable"> Flag the signals if the command can be executed. </param>
		public CommandHandler(Action action, bool executable)
		{
			_action = action;
			this.Executable = executable;
		}

		#endregion

		#region Methods

		/// <inheritdoc />
		public void Execute(object parameter)
		{
			_action?.Invoke();
		}

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			return this.Executable;
		}
		
		#endregion
	}
}