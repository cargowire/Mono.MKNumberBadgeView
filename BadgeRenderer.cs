
using System;
using System.ComponentModel;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Badge), typeof(BadgeRenderer))]

namespace Mono.MKNumberBadgeView
{
    /// <summary>
    /// Badge renderer.
    /// </summary>
    public class BadgeRenderer : ViewRenderer<Badge, BadgeView>
    {
        private BadgeView badgeView;

        /// <summary>
        /// Responds to the element changing.
        /// </summary>
        /// <param name="args">The args</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Badge> args)
        {
            base.OnElementChanged(args);

            if (args.OldElement != null || this.Element == null)
            {
                return;
            }

            this.badgeView = this.badgeView ?? new BadgeView(
                new RectangleF(
                    (float)this.Element.X,
                    (float)this.Element.Y,
                    (float)this.Element.Width,
                    (float)this.Element.Height));

            this.badgeView.Value = this.Element.Value;

            this.SetNativeControl(this.badgeView);
        }

        /// <summary>
        /// Responds to properties changing on the element.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="args">The args</param>
        protected override void OnElementPropertyChanged(
            object sender,
            PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
            if (this.Element == null || this.Control == null)
            {
                return;
            }

            if (args.PropertyName == "Value")
            {
                this.Control.Value = this.Element.Value;
            }
        }
    }
}
