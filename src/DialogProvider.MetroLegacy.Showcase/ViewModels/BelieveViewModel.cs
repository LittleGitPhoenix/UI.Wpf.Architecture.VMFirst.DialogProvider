using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.ViewModels
{
	class BelieveViewModel
	{
		internal Task<DialogResult> OnAccept()
		{
			Trace.WriteLine($"Atta boy.");
			return Task.FromResult(DialogResult.Yes);
		}

		internal Task<DialogResult> OnDecline()
		{
			Trace.WriteLine($"but you have to believe.");
			return Task.FromResult(DialogResult.None);
		}
	}
}