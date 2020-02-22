#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models
{
	/// <summary>
	/// Readable representation of an <see cref="Exception"/>.
	/// </summary>
	public class InnerExceptionDialogModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> <see cref="Exception"/> name. </summary>
		public string ExceptionName { get; }

		/// <summary> <see cref="Exception"/> message. </summary>
		public string ExceptionMessage { get; }

		/// <summary> <see cref="StackTrace"/> of the <see cref="Exception"/>. Can be <c>Null</c>. </summary>
		public List<string> StackInformation { get; }

		#endregion
		
		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="exception"></param>
		public InnerExceptionDialogModel(Exception exception)
		{
			this.ExceptionName = exception.GetType().Name;
			this.ExceptionMessage = exception.Message;
			this.StackInformation = InnerExceptionDialogModel.ParseStackTrace(exception);
		}

		#endregion

		#region Methods

		private static List<string> ParseStackTrace(Exception exception)
		{
			List<string> stackEntries = new List<string>();

			// Get the stack trace.
			StackTrace stackTrace = new StackTrace(exception, true);
			if (stackTrace.FrameCount > 0)
			{
				StackFrame[] stackFrames = stackTrace.GetFrames();
				// ReSharper disable once PossibleNullReferenceException
				foreach (StackFrame stackFrame in stackFrames)
				{
					string fileName = stackFrame.GetFileName();
					string methodName = stackFrame.GetMethod()?.Name;

					// If no useful information is available, than skip this frame.
					if (String.IsNullOrWhiteSpace(fileName) && String.IsNullOrWhiteSpace(methodName)) continue;

					if (String.IsNullOrWhiteSpace(fileName)) fileName = "[UNKNOWN FILE]";
					if (String.IsNullOrWhiteSpace(methodName)) methodName = "[UNKNOWN METHOD]";

					int lineNumber = stackFrame.GetFileLineNumber();
					int columnNumber = stackFrame.GetFileColumnNumber();

					stackEntries.Add($"{fileName} :: {methodName} - Line: {lineNumber}, Column: {columnNumber}");
				}
			}

			return stackEntries;
		}

		#endregion
	}
}