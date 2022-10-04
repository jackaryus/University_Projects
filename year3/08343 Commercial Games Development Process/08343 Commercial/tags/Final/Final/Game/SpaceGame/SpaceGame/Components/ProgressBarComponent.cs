// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// A progress bar
    /// </summary>
    public class ProgressBarComponent : Component
    {
        private float ActualValue = 0.0f;
        private float ActualMax = 1.0f;
        public float Value { get; set; }
        public float MaxValue { get; set; }

        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public float InterpolateSpeed { get; set; }
        public int Padding { get; set; }

        private bool RenderCentered = true;

        /// <summary>
        /// A progress bar
        /// </summary>
        /// <param name="value">The initial value of the progress bar</param>
        /// <param name="maxValue">The maximum value of the progress bar</param>
        /// <param name="foregroundColor">The colour of the bar itself</param>
        /// <param name="backgroundColor">The colour of the progress bar background</param>
        /// <param name="interpolateSpeed">How fast the bar updates to its actual value</param>
        /// <param name="padding">Padding of the bar from the edges of the element</param>
        public ProgressBarComponent(float value, float maxValue, Color foregroundColor, Color backgroundColor, float interpolateSpeed = 0.2f, int padding = 1)
        {
            ActualValue = Value = value;
            ActualMax = MaxValue = maxValue;

            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            InterpolateSpeed = interpolateSpeed;
            Padding = padding;
        }

        protected override void OnUpdate(float delta)
        {
            if (ActualValue != Value)
                ActualValue = UtilityManager.Lerp(ActualValue, Value, Math.Max(0.0f, Math.Min(1.0f, InterpolateSpeed)));
            if(ActualMax != MaxValue)
                ActualMax = UtilityManager.Lerp(ActualMax, MaxValue, Math.Max(0.0f, Math.Min(1.0f, InterpolateSpeed)));
        }

        protected override void OnRender(float delta)
        {
            RectangleF fillRegion = new RectangleF(Position.X + Padding, Position.Y + Padding, ActualValue / ActualMax * (Scale.X - Padding * 2), Scale.Y - Padding * 2);

            Draw(Pixel, Bounds, null, BackgroundColor * Alpha, 0.0f, RenderCentered);
            Draw(Pixel, fillRegion.ToRectangle(), null, ForegroundColor * Alpha, 0.0f, RenderCentered);
        }

        /// <summary>
        /// Sets the value of the progress bar
        /// </summary>
        /// <param name="newValue">The new value of the progress bar</param>
        /// <param name="bInstant">If true, doesn't interpolate to the new value</param>
        /// <returns>This progress bar component</returns>
        public ProgressBarComponent SetValue(float newValue, bool bInstant = false)
        {
            return SetValue(newValue, MaxValue, bInstant);
        }

        /// <summary>
        /// Sets the value of the progress bar
        /// </summary>
        /// <param name="newValue">The new value of the progress bar</param>
        /// <param name="newMax">A new maximum value for this component</param>
        /// <param name="bInstant">If true, doesn't interpolate to the new value</param>
        /// <returns>This progress bar component</returns>
        public ProgressBarComponent SetValue(float newValue, float newMax, bool bInstant = false)
        {
            Value = newValue;
            MaxValue = newMax;

            if (bInstant)
                ActualValue = Value;

            return this;
        }

        /// <summary>
        /// Sets the bar color
        /// </summary>
        /// <param name="newColor">The new color to set</param>
        /// <returns>This progress bar component</returns>
        public ProgressBarComponent SetForegroundColor(Color newColor)
        {
            return SetColors(newColor, BackgroundColor);
        }

        /// <summary>
        /// Sets the background color
        /// </summary>
        /// <param name="newColor">The new color to set</param>
        /// <returns>This progress bar component</returns>
        public ProgressBarComponent SetBackgroundColor(Color newColor)
        {
            return SetColors(ForegroundColor, newColor);
        }

        /// <summary>
        /// Set's both the bar and background colors
        /// </summary>
        /// <param name="newFGColor">The new bar color</param>
        /// <param name="newBGColor">The new background color</param>
        /// <returns>This progress bar component</returns>
        public ProgressBarComponent SetColors(Color newFGColor, Color newBGColor)
        {
            ForegroundColor = newFGColor;
            BackgroundColor = newBGColor;
            return this;
        }
    }
}
