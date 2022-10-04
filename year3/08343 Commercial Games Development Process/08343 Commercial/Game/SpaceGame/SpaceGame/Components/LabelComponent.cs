// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// A text label
    /// </summary>
    public class LabelComponent : Component
    {
        public string Text { get; set; }
        public string Font { get; set; }
        public float FontScale { get; set; }
        public ETextHorizontalAlign TextHAlign { get; set; }
        public ETextVerticalAlign TextVAlign { get; set; }

        /// <summary>
        /// A text label
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="font">The font to render the text in</param>
        /// <param name="tint">The color of the text</param>
        /// <param name="fontScale">The scale of the text</param>
        /// <param name="hAlign">How to horizontally align the text</param>
        /// <param name="vAlign">How to vertically align the text</param>
        public LabelComponent(string text, string font, Color tint, float fontScale = 1.0f, ETextHorizontalAlign hAlign = ETextHorizontalAlign.Left, ETextVerticalAlign vAlign = ETextVerticalAlign.Top)
        {
            Text = text;
            Font = font;
            SetTint(tint);
            FontScale = fontScale;
            TextHAlign = hAlign;
            TextVAlign = vAlign;
        }

        protected override void OnRender(float delta)
        {
            DrawString(Font, Text, Position + new Vector2(1.0f, 1.0f) * FontScale, Color.Black * Alpha, FontScale, TextHAlign, TextVAlign);
            DrawString(Font, Text, Position, Tint * Alpha, FontScale, TextHAlign, TextVAlign);
        }

        public LabelComponent SetText(string newText)
        {
            Text = newText;
            return this;
        }
    }
}
