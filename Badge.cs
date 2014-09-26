
using System;
using Xamarin.Forms;

namespace Mono.MKNumberBadgeView
{
    /// <summary>
    /// A 'Badge' (generally red circle with a number).
    /// </summary>
    public class Badge : View
    {
        /// <summary>
        /// The content property.
        /// </summary>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create<Badge, int>(p => p.Value, 0, propertyChanged: null);

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }
    }
}
