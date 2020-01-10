using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Specialized exception class that is used when the <see cref="IViewProvider"/> fails.
	/// </summary>
	[Serializable]
	public class ViewProviderException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewProviderException"/> class.
		/// </summary>
		public ViewProviderException() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewProviderException"/> class with a specified error message.
		/// </summary>
		/// <param name="message"> The error message that explains the reason for the exception. </param>
		public ViewProviderException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewProviderException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message"> The error message that explains the reason for the exception. </param>
		/// <param name="inner"> The exception that is the cause of the current exception, or a <c>NULL</c> reference if no inner exception is specified. </param>
		public ViewProviderException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// This protected constructor is used for deserialization.
		/// </summary>
		protected ViewProviderException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Used to perform a custom serialization.
		/// </summary>
		/// <param name="info"> The <see cref="SerializationInfo"/>. </param>
		/// <param name="context"> The <see cref="StreamingContext"/>. </param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); }
	}
}