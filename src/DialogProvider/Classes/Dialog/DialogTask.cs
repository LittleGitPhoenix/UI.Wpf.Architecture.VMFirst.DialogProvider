#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// A custom <see cref="Task"/> that will be returned by most every method of the <see cref="DialogManager"/>. Can be used to interact with the dialog.
	/// </summary>
	public class DialogTask : Task<DialogResult>
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		/// <summary> Callback function used to close the dialog. </summary>
		/// <remarks> This is called by <see cref="CloseDialog"/>. </remarks>
		private readonly Action<DialogResult> _closeCallback;

		#endregion

		#region Properties

		/// <summary> A <see cref="DialogTask"/> that is already completed and has a result of <see cref="Killed"/>. </summary>
		public static DialogTask Killed { get; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Static constructor
		/// </summary>
		static DialogTask()
		{
			// Create a special 'Killed' dialog task by creating and running a normal dialog task.
			using (var cts = new CancellationTokenSource())
			{
				Killed = new DialogTask(cts.Token);
				Killed.Start();
			}
		}
		
		/// <summary>
		/// Constructor for <see cref="Killed"/>.
		/// </summary>
		/// <param name="cancellationToken"> The <see cref="CancellationToken"/> for the <see cref="Killed"/>-<see cref="DialogTask"/>. </param>
		private DialogTask(CancellationToken cancellationToken) : base(() => DialogResult.Killed, cancellationToken, TaskCreationOptions.None) { }

		/// <inheritdoc />
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="function"> The delegate that represents the code to execute in the task. When the function has completed, the task's System.Threading.Tasks.Task`1.Result property will be set to return the result value of the function. </param>
		/// <param name="closeCallback"> Callback function used to close the dialog. </param>
		/// <param name="cancellationToken"> <see cref="CancellationToken"/> used to stop this task and return to the caller. </param>
		internal DialogTask(Func<DialogResult> function, Action<DialogResult> closeCallback, CancellationToken cancellationToken)
			: base
			(
				function: function,
				cancellationToken: cancellationToken,
				creationOptions: TaskCreationOptions.LongRunning
			)
		{
			// Save parameters.
			_closeCallback = closeCallback;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Closes the dialog related to this instance.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of the dialog. </param>
		public void CloseDialog(DialogResult dialogResult)
		{
			_closeCallback.Invoke(dialogResult);
		}

		#endregion
	}
}