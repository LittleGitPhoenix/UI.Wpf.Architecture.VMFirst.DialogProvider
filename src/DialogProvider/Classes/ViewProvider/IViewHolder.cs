#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Collections.Generic;
using System.Reflection;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Interface used to bind view assemblies to their view models counterparts.
	/// </summary>
	public interface IViewHolder
	{
		/// <summary>
		/// Collection of <see cref="Assembly"/>s that this assembly provides views for.
		/// </summary>
		/// <example>
		/// <para> Return the assembly of one type from the supported view model assemblies: </para>
		/// <para> public ICollection{Assembly} ViewModelAssemblies { get; } =  new List{Assembly}() { typeof(DetailsViewModel).Assembly } </para>
		/// </example>
		ICollection<Assembly> ViewModelAssemblies { get; }
	}
}