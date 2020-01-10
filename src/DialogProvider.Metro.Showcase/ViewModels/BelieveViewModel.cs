using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.DialogProvider.Showcase.ViewModels
{
	class BelieveViewModel
	{
		internal DialogResult OnAccept()
		{
			Trace.WriteLine($"Atta boy.");
			return DialogResult.Yes;
		}

		internal DialogResult OnDecline()
		{
			Trace.WriteLine($"but you have to believe.");
			return DialogResult.None;
		}
	}
}