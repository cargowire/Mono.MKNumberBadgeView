using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Mono.MKNumberBadgeView
{
    /// <summary>
    /// A Badge view.
    /// </summary>
    public class BadgeView : UIView
    {
        private int val;
        private bool hideWhenZero;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeView"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public BadgeView(RectangleF frame)
            : base(frame)
        {
            this.InitState();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeView"/> class.
        /// </summary>
        /// <param name="coder">The coder.</param>
        public BadgeView(NSCoder coder)
            : base(coder)
        {
            this.InitState();
        }

        /// <summary>
        /// Gets or sets the Text format for the badge, defaults to just the number
        /// </summary>
        /// <value>The text format.</value>
        public string TextFormat { get; set; }

        /// <summary>
        /// Gets or sets the adjustment offset for the text in the badge
        /// </summary>
        /// <value>The adjust offset.</value>
        public PointF AdjustOffset { get; set; }

        /// <summary>
        /// Gets or sets the current value displayed in the badge. Updating the value will update the view's display.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get
            {
                return this.val;
            }

            set
            {
                if (this.val != value)
                {
                    this.val = value;
                    this.Hidden = this.HideWhenZero && this.val == 0;
                    var size = this.BadgeSize;
                    this.Frame = new RectangleF(this.Frame.X, this.Frame.Y, size.Width, size.Height);
                    this.SetNeedsDisplay();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the badge view draws a dhadow or not.
        /// </summary>
        /// <value><c>true</c> if shadow; otherwise, <c>false</c>.</value>
        public bool Shadow { get; set; }

        /// <summary>
        /// Gets or sets the offset for the shadow, if there is one.
        /// </summary>
        /// <value>The shadow offset.</value>
        public SizeF ShadowOffset { get; set; }

        /// <summary>
        /// Gets or sets the base color for the shadow, if there is one.
        /// </summary>
        /// <value>The color of the shadow.</value>
        public UIColor ShadowColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the badge view should be drawn with a shine.
        /// </summary>
        /// <value><c>true</c> if shine; otherwise, <c>false</c>.</value>
        public bool Shine { get; set; }

        /// <summary>
        /// Gets or sets the font to be used for drawing the numbers. NOTE: not all fonts are created equal for this purpose.
        /// Only "system fonts" should be used.
        /// </summary>
        /// <value>The font.</value>
        public UIFont Font { get; set; }

        /// <summary>
        /// Gets or sets the color used for the background of the badge.
        /// </summary>
        /// <value>The color of the fill.</value>
        public UIColor FillColor { get; set; }

        /// <summary>
        /// Gets or sets the color to be used for drawing the stroke around the badge.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public UIColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the width for the stroke around the badge.
        /// </summary>
        /// <value>The width of the stroke.</value>
        public float StrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets The color to be used for drawing the badge's numbers.
        /// </summary>
        /// <value>The color of the text.</value>
        public UIColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets how the badge image hould be aligned horizontally in the view.
        /// </summary>
        /// <value>The alignment.</value>
        public UITextAlignment Alignment { get; set; }

        /// <summary>
        /// Gets the visual size of the badge for the current value. Not the same hing as the size of the view's bounds.
        /// The badge view bounds should be wider than space needed to draw the badge.
        /// </summary>
        /// <value>The size of the badge.</value>
        public SizeF BadgeSize
        {
            get
            {
                var numberString = this.Value.ToString(this.TextFormat);

                SizeF numberSize = new NSString(numberString).StringSize(this.Font);

                using (CGPath badgePath = this.NewBadgePathForTextSize(numberSize))
                {
                    RectangleF badgeRect = badgePath.PathBoundingBox;

                    badgeRect.X = 0;
                    badgeRect.Y = 0;
                    badgeRect.Size = new SizeF((float)Math.Ceiling(badgeRect.Size.Width), (float)Math.Ceiling(badgeRect.Size.Height));

                    return badgeRect.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of pixels between the number inside the badge and the stroke around the badge. This value
        /// is approximate, as the font geometry might effectively slightly increase or decrease the apparent pad.
        /// </summary>
        /// <value>The pad.</value>
        public int Pad { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this badge hides when the value reaches zero.
        /// </summary>
        /// <value><c>true</c> if hide when zero; otherwise, <c>false</c>.</value>
        public bool HideWhenZero
        {
            get
            {
                return this.hideWhenZero;
            }

            set
            {
                if (this.hideWhenZero != value)
                {
                    this.hideWhenZero = value;
                    this.Hidden = this.hideWhenZero && this.val == 0;
                    this.SetNeedsDisplay();
                }
            }
        }

        /// <summary>
        /// Gets the size that best fits this view.
        /// </summary>
        /// <param name="size">Current size.</param>
        /// <returns>The size.</returns>
        public override SizeF SizeThatFits(SizeF size)
        {
            return this.BadgeSize;
        }

        /// <summary>
        /// Draw the view into the specified rect.
        /// </summary>
        /// <param name="rect">The Rect.</param>
        public override void Draw(RectangleF rect)
        {
            RectangleF viewBounds = this.Bounds;

            CGContext curContext = UIGraphics.GetCurrentContext();

            var numberString = this.Value.ToString(this.TextFormat);

            SizeF numberSize = new NSString(numberString).StringSize(this.Font);

            using (CGPath badgePath = this.NewBadgePathForTextSize(numberSize))
            {
                RectangleF badgeRect = badgePath.PathBoundingBox;

                badgeRect.X = 0;
                badgeRect.Y = 0;
                badgeRect.Size = new SizeF((float)Math.Ceiling(badgeRect.Size.Width), (float)Math.Ceiling(badgeRect.Size.Height));

                curContext.SaveState();

                curContext.SetLineWidth(this.StrokeWidth);
                curContext.SetStrokeColorWithColor(this.StrokeColor.CGColor);
                curContext.SetFillColorWithColor(this.FillColor.CGColor);

                // Line stroke straddles the path, so we need to account for the outer portion
                badgeRect.Size = new SizeF(badgeRect.Size.Width + (float)Math.Ceiling(this.StrokeWidth / 2), badgeRect.Size.Height + (float)Math.Ceiling(this.StrokeWidth / 2));

                PointF ctm = new PointF(0f, 0f);

                switch (this.Alignment)
                {
                case UITextAlignment.Justified:
                case UITextAlignment.Natural:
                case UITextAlignment.Center:
                    ctm = new PointF((float)Math.Round((viewBounds.Size.Width - badgeRect.Size.Width) / 2), (float)Math.Round((viewBounds.Size.Height - badgeRect.Size.Height) / 2));
                    break;
                case UITextAlignment.Left:
                    ctm = new PointF(0.0f, (float)Math.Round((viewBounds.Size.Height - badgeRect.Size.Height) / 2));
                    break;
                case UITextAlignment.Right:
                    ctm = new PointF(viewBounds.Size.Width - badgeRect.Size.Width, (float)Math.Round(viewBounds.Size.Height - badgeRect.Size.Height) / 2);
                    break;
                }

                curContext.TranslateCTM(ctm.X, ctm.Y);

                if (this.Shadow)
                {
                    curContext.SaveState();

                    SizeF blurSize = this.ShadowOffset;

                    curContext.SetShadowWithColor(blurSize, 4, this.ShadowColor.CGColor);

                    curContext.BeginPath();
                    curContext.AddPath(badgePath);
                    curContext.ClosePath();

                    curContext.DrawPath(CGPathDrawingMode.FillStroke);
                    curContext.RestoreState();
                }

                curContext.BeginPath();
                curContext.AddPath(badgePath);
                curContext.ClosePath();
                curContext.DrawPath(CGPathDrawingMode.FillStroke);

                if (this.Shine)
                {
                    curContext.BeginPath();
                    curContext.AddPath(badgePath);
                    curContext.ClosePath();
                    curContext.Clip();

                    using (CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB())
                    {
                        float[] shinyColorGradient = new float[8] { 1f, 1f, 1f, 0.8f, 1f, 1f, 1f, 0f };
                        float[] shinyLocationGradient = new float[2] { 0f, 1f };

                        using (CGGradient gradient = new CGGradient(colorSpace, shinyColorGradient, shinyLocationGradient))
                        {
                            curContext.SaveState();
                            curContext.BeginPath();
                            curContext.MoveTo(0.0f, 0.0f);

                            float shineStartY = badgeRect.Size.Height * 0.25f;
                            float shineStopY = shineStartY + (badgeRect.Size.Height * 0.4f);

                            curContext.AddLineToPoint(0, shineStartY);
                            curContext.AddCurveToPoint(
                                0,
                                shineStopY,
                                badgeRect.Size.Width,
                                shineStopY,
                                badgeRect.Size.Width,
                                shineStartY);
                            curContext.AddLineToPoint(badgeRect.Size.Width, 0);
                            curContext.ClosePath();
                            curContext.Clip();
                            curContext.DrawLinearGradient(
                                gradient,
                                new PointF(badgeRect.Size.Width / 2.0f, 0.0f),
                                new PointF(badgeRect.Size.Width / 2.0f, shineStopY),
                                CGGradientDrawingOptions.DrawsBeforeStartLocation);
                            curContext.RestoreState();
                        }
                    }
                }

                curContext.RestoreState();

                curContext.SaveState();
                curContext.SetFillColorWithColor(this.TextColor.CGColor);

                PointF textPt = new PointF(
                    ctm.X + ((badgeRect.Size.Width - numberSize.Width) / 2) + this.AdjustOffset.X,
                    ctm.Y + ((badgeRect.Size.Height - numberSize.Height) / 2) + this.AdjustOffset.Y);

                new NSString(numberString).DrawString(textPt, this.Font);

                curContext.RestoreState();
            }
        }

        private void InitState()
        {
            var iosVersion = new Version(UIDevice.CurrentDevice.SystemVersion);

            this.Opaque = false;
            this.Pad = 2;
            this.Font = UIFont.BoldSystemFontOfSize(16);
            this.Shadow = iosVersion.Major < 7;
            this.ShadowOffset = new SizeF(0, 3);
            this.ShadowColor = UIColor.Black.ColorWithAlpha(0.5f);
            this.Shine = iosVersion.Major < 7;
            this.Alignment = UITextAlignment.Right;
            this.FillColor = UIColor.Red;
            this.StrokeColor = iosVersion.Major < 7 ? UIColor.White : UIColor.Clear;
            this.StrokeWidth = iosVersion.Major < 7 ? 2.0f : 0.0f;
            this.TextColor = UIColor.White;
            this.HideWhenZero = true;
            this.AdjustOffset = new PointF(0, 0);
            this.TextFormat = "d";

            this.BackgroundColor = UIColor.Clear;
        }

        private CGPath NewBadgePathForTextSize(SizeF size)
        {
            float arcRadius = (float)Math.Ceiling((size.Height + this.Pad) / 2.0f);

            float badgeWidthAdjustment = size.Width - (size.Height / 2.0f);
            float badgeWidth = 2.0f * arcRadius;

            if (badgeWidthAdjustment > 0.0)
            {
                badgeWidth += badgeWidthAdjustment;
            }

            CGPath badgePath = new CGPath();

            var m_pi_2 = (float)(Math.PI / 2);

            badgePath.MoveToPoint(arcRadius, 0.0f);
            badgePath.AddArc(arcRadius, arcRadius, arcRadius, 3.0f * m_pi_2, m_pi_2, true);
            badgePath.AddLineToPoint(badgeWidth - arcRadius, 2.0f * arcRadius);
            badgePath.AddArc(badgeWidth - arcRadius, arcRadius, arcRadius, m_pi_2, 3.0f * m_pi_2, true);
            badgePath.AddLineToPoint(arcRadius, 0.0f);

            return badgePath;
        }
    }
}
