#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// A special <see cref="Adorner"/> that can display any <see cref="Visual"/>-<see cref="Content"/>.
	/// </summary>
	/// <remarks> https://stackoverflow.com/questions/9998691/wpf-adorner-with-controls-inside/10034274#10034274 </remarks>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class ContentAdorner : Adorner
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private readonly VisualCollection _visuals;

		private readonly ContentPresenter _contentPresenter;

		#endregion

		#region Properties

		/// <inheritdoc />
		[PropertyChanged.DoNotNotify]
		protected override int VisualChildrenCount => _visuals.Count;

		/// <summary> The content of this adorner. </summary>
		public Visual Content
		{
			get => _contentPresenter.Content as Visual;
			set => _contentPresenter.Content = value;
		}

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="adornedElement"> The <see cref="UIElement"/> that will be adorned. </param>
		private ContentAdorner(UIElement adornedElement)
			: base(adornedElement)
		{
			// Initialize fields.
			_visuals = new VisualCollection(this);
			_contentPresenter = new ContentPresenter();
			_visuals.Add(_contentPresenter);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="adornedElement"> The <see cref="UIElement"/> that will be adorned. </param>
		/// <param name="content"> The content of the adorner. </param>
		public ContentAdorner(UIElement adornedElement, Visual content)
			: this(adornedElement)
		{
			// Save parameters.
			this.Content = content;
		}

		#endregion

		#region Methods

		/// <inheritdoc />
		protected override Size MeasureOverride(Size constraint)
		{
			// Measure the size of the content presenter. This is mandatory as the call to 'Measure' will be recursive throughout all child elements.
			_contentPresenter.Measure(constraint);
			
			// Do not return the desired size (which is set via the above call to 'Measure') of the content presenter.
			//return _contentPresenter.DesiredSize;
			
			// Return the maximum available size of the adorned element instead, so that the adorner overlays it.
			return base.AdornedElement.RenderSize;
		}

		/// <inheritdoc />
		protected override Size ArrangeOverride(Size finalSize)
		{
			_contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
			return _contentPresenter.RenderSize;
		}

		/// <inheritdoc />
		protected override Visual GetVisualChild(int index)
		{
			return _visuals[index];
		}

		#endregion
	}
}