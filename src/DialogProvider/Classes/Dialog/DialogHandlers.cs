#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Collections.Generic;
using System.Linq;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Collection of <see cref="DialogHandler"/>s.
	/// </summary>
	class DialogHandlers : List<DialogHandler>
	{
		public DialogHandlers(IEnumerable<DialogHandler> dialogRegistrations) : base(dialogRegistrations) { }

		public DialogHandler GetDialogHandler(DialogDisplayLocation displayLocation)
		{
			return this.FirstOrDefault(dialogRegistration => dialogRegistration.DisplayLocation == displayLocation);
		}
	}
}