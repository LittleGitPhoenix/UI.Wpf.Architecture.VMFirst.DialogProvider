#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.DialogProvider.Models
{
	/// <summary>
	/// Model for <see cref="ExceptionDialogViewModel"/>.
	/// </summary>
	public class ExceptionDialogModel : List<InnerExceptionDialogModel>
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> Name of the first level exception. </summary>
		public string ExceptionName => this.First().ExceptionName;

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="exception"> The <see cref="Exception"/> that will be parsed and displayed. </param>
		public ExceptionDialogModel(Exception exception)
		{
			//this.ExceptionName = exception.GetType().Name;
			this.AddRange(ExceptionDialogModel.ParseException(exception));
		}

		#endregion

		#region Methods

		private static IEnumerable<InnerExceptionDialogModel> ParseException(Exception exception, byte level = 0)
		{
			if (exception == null) yield break;
			
			if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 1)
			{
				var flattenedException = aggregateException.Flatten();
				if (flattenedException.InnerExceptions.Count == 1)
				{
					// Directly use the topmost exception.
					yield return new InnerExceptionDialogModel(flattenedException.InnerExceptions.First());
				}
				else
				{
					// Create one exception detail object for each inner exception.
					foreach (var ex in flattenedException.InnerExceptions)
					{
						yield return new InnerExceptionDialogModel(ex);
					}
				}
			}
			else
			{
				// Directly use the topmost exception.
				yield return new InnerExceptionDialogModel(exception);

				// Parse inner exceptions if needed and only up to 10 nested levels.
				if (level < 10 && exception.InnerException != null)
				{
					foreach (var innerExceptionDialogModel in ExceptionDialogModel.ParseException(exception.InnerException, (byte)(level + 1)))
					{
						yield return innerExceptionDialogModel;
					}
				};
			}
		}

		#endregion
	}
}