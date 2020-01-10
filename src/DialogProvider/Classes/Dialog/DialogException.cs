#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Specialized exception class that is used if initializing showing a dialog fails.
	/// </summary>
	[Serializable]
	public class DialogException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DialogException"/> class.
		/// </summary>
		public DialogException() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="DialogException"/> class with a specified error message.
		/// </summary>
		/// <param name="message"> The error message that explains the reason for the exception. </param>
		public DialogException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="DialogException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message"> The error message that explains the reason for the exception. </param>
		/// <param name="inner"> The exception that is the cause of the current exception, or a <c>NULL</c> reference if no inner exception is specified. </param>
		public DialogException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// This protected constructor is used for deserialization.
		/// </summary>
		protected DialogException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Used to perform a custom serialization.
		/// </summary>
		/// <param name="info"> The <see cref="SerializationInfo"/>. </param>
		/// <param name="context"> The <see cref="StreamingContext"/>. </param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); }
	}
}