#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Factory class for <see cref="DialogTask"/>s.
	/// </summary>
	/// <remarks> Used as a wrapper for the <see cref="DialogTask"/>s internal function. </remarks>
	internal class DialogTaskFactory
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> The <see cref="DialogResult"/> of the dialog. </summary>
		internal DialogResult Result { get; set; }

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		public DialogTaskFactory() { }

		#endregion

		#region Methods
		
		/// <summary>
		/// Creates a new <see cref="DialogTask"/>.
		/// </summary>
		/// <param name="closeCallback"> Callback function used to close the dialog. </param>
		/// <param name="cancellationToken"> <see cref="CancellationToken"/> used to stop this task and return to the caller. </param>
		/// <returns> A new <see cref="DialogTask"/> instance. </returns>
		internal DialogTask Create(Action<DialogResult> closeCallback, CancellationToken cancellationToken)
		{
			return new DialogTask(() => this.Wait(cancellationToken), closeCallback, cancellationToken);
		}

		/// <summary>
		/// Endless waiting function that must be canceled using the <paramref name="cancellationToken"/>.
		/// </summary>
		/// <param name="cancellationToken"> The <see cref="CancellationToken"/> used to cancel waiting. </param>
		/// <returns> A <see cref="DialogResult"/>. </returns>
		private DialogResult Wait(CancellationToken cancellationToken)
		{
			try
			{
				Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken).Wait(cancellationToken);
			}
			catch
			{
				/*ignore*/
			}

			return this.Result;
		}

		#endregion
	}
}