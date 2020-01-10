#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phoenix.UI.Wpf.DialogProvider.Metro.Classes
{
    /// <summary>
    /// Special <see cref="ResourceDictionary"/> for design mode purposes.
    /// </summary>
    internal class DesignTimeResourceDictionary : ResourceDictionary
    {
        /// <summary> Flag signaling if the application is currently in design mode. </summary>
        private static readonly bool IsInDesignMode = (bool) System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        /// <summary> Special <see cref="Source"/> property whoose value is only applied set in desing mode. </summary>
        public new Uri Source
        {
            get => base.Source;
            set
            {
                // Only set the real source if in design mode.
                if (DesignTimeResourceDictionary.IsInDesignMode) base.Source = value;
            } 
        }
    }
}