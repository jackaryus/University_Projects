// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// A clickable button
    /// </summary>
    public class ButtonComponent : Component
    {
        public Color IdleColor { get; set; }
        public Color HoverColor { get; set; }
        public Color DownColor { get; set; }
        Action OnClick { get; set; }
        Action OnHover { get; set; }
        Action OnUnhover { get; set; }
        Action OnRelease { get; set; }

        public string Text { get; set; }
        public Color TextColor { get; set; }
        LabelComponent label;

        /// <summary>
        /// A clickable button
        /// </summary>
        /// <param name="texture">The texture to render for the button (Defaults to a solid rectangle if null)</param>
        /// <param name="text">The text to render on this button</param>
        /// <param name="textFont">The font to render the text in</param>
        /// <param name="textColor">The color to render the text in</param>
        /// <param name="textScale">The text scale</param>
        /// <param name="idleColor">The default tint of the button</param>
        /// <param name="hoverColor">The tint of the button when the mouse is over it</param>
        /// <param name="downColor">The tint of the button when clicked</param>
        /// <param name="onClick">An event to run when clicked</param>
        /// <param name="onHover">An event to run the mouse moves over the button</param>
        /// <param name="onUnhover">An event to run when the mouse moves off the butotn</param>
        /// <param name="onRelease">An event to run when the mouse button is released</param>
        public ButtonComponent(string texture, string text, string textFont, Color textColor, float textScale, Color idleColor, Color hoverColor, Color downColor, Action onClick = null, Action onHover = null, Action onUnhover = null, Action onRelease = null)
        {
            IdleColor = idleColor;
            HoverColor = hoverColor;
            DownColor = downColor;
            OnClick = onClick;
            OnHover = onHover;
            OnUnhover = onUnhover;
            OnRelease = onRelease;

            SetTexture(texture);

            Text = text;
            TextColor = textColor;

            label = new LabelComponent(text, textFont, textColor, textScale, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            RegisterComponent(label);
        }

        /// <summary>
        /// A clickable button
        /// </summary>
        /// <param name="text">The text to render on this button</param>
        /// <param name="textFont">The font to render the text in</param>
        /// <param name="textColor">The color to render the text in</param>
        /// <param name="textScale">The text scale</param>
        /// <param name="idleColor">The default tint of the button</param>
        /// <param name="hoverColor">The tint of the button when the mouse is over it</param>
        /// <param name="downColor">The tint of the button when clicked</param>
        /// <param name="onClick">An event to run when clicked</param>
        /// <param name="onHover">An event to run the mouse moves over the button</param>
        /// <param name="onUnhover">An event to run when the mouse moves off the butotn</param>
        /// <param name="onRelease">An event to run when the mouse button is released</param>
        public ButtonComponent(string text, string textFont, Color textColor, float textScale, Color idleColor, Color hoverColor, Color downColor, Action onClick = null, Action onHover = null, Action onUnhover = null, Action onRelease = null)
            : this(null, text, textFont, textColor, textScale, idleColor, hoverColor, downColor, onClick, onHover, onUnhover, onRelease)
        {
        }

        /// <summary>
        /// A clickable button
        /// </summary>
        /// <param name="texture">The texture to render for the button (Defaults to a solid rectangle if null)</param>
        /// <param name="idleColor">The default tint of the button</param>
        /// <param name="hoverColor">The tint of the button when the mouse is over it</param>
        /// <param name="downColor">The tint of the button when clicked</param>
        /// <param name="onClick">An event to run when clicked</param>
        /// <param name="onHover">An event to run the mouse moves over the button</param>
        /// <param name="onUnhover">An event to run when the mouse moves off the butotn</param>
        /// <param name="onRelease">An event to run when the mouse button is released</param>
        public ButtonComponent(string texture, Color idleColor, Color hoverColor, Color downColor, Action onClick = null, Action onHover = null, Action onUnhover = null, Action onRelease = null)
            : this(texture, "", "generic", Color.White, 0.0f, idleColor, hoverColor, downColor, onClick, onHover, onUnhover, onRelease)
        {
        }

        public override void MouseOver(MouseEvent e)
        {
            if (IsVisible && OnHover != null)
            {
                OnHover.Invoke();
                e.Handled = true;
            }
        }

        public override void MouseOut(MouseEvent e)
        {
            if (IsVisible && OnUnhover != null)
            {
                OnUnhover.Invoke();
                e.Handled = true;
            }
        }

        public override void MouseDown(MouseEvent e)
        {
            if (IsVisible && OnClick != null)
            {
                OnClick.Invoke();
                e.Handled = true;
            }
        }

        public override void MouseUp(MouseEvent e)
        {
            if (IsVisible && OnRelease != null)
            {
                OnRelease.Invoke();
                e.Handled = true;
            }
        }

        protected override void OnUpdate(float delta)
        {
            label
                .SetText(Text)
                .SetTint(TextColor)
                .SetRenderSpace(RenderSpace)
                .SetPosition(new Vector2(Bounds.Center.X, Bounds.Center.Y))
                .SetZIndex(ZIndex + 1)
                .SetVisible(IsVisible);             
        }

        protected override void OnRender(float delta)
        {
            Draw(Texture != null ? GraphicsManager.GetTexture(Texture) : Pixel, Bounds, null, MouseState == EMouseState.Down ? DownColor : (MouseState == EMouseState.Over ? HoverColor : IdleColor) * Alpha);
        }
    }
}
