// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceGame.Entities;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame
{
    public enum ERenderSpace
    {
        WorldSpace, //Co-ords are representitive of a location in the world
        ScreenSpace, //Co-ords are representitive of a location on the screen
    }

    public enum ETextHorizontalAlign
    {
        Left,
        Center,
        Right
    }

    public enum ETextVerticalAlign
    {
        Top,
        Middle,
        Bottom
    }

    /// <summary>
    /// The base entity class
    /// </summary>
    public class Entity
    {
        protected static SpriteBatch SpriteBatch { get { return Main.SpriteBatch; } }
        protected static Texture2D Pixel { get { return UtilityManager.Pixel; } }

        /// <summary>This entity has spawned and is live</summary>
        public bool IsAlive { get; private set; }
        /// <summary>This entity has been destroyed. Don't call any methods if this is true.</summary>
        public bool IsDestroyed { get; private set; }
        private bool _isVisible = true;
        /// <summary>This entity is not visible and it and its components won't be drawn.</summary>
        public bool IsVisible { get { return _isVisible; } private set { _isVisible = value; } }
        List<Component> components = new List<Component>();

        /// <summary>Owning entity (If any)</summary>
        public Entity Parent { get; private set; }
        /// <summary>Owning player (If any)</summary>
        public Player OwningPlayer { get; private set; }

        public Vector2 Position { get; private set; }
        public Vector2 Scale { get; private set; }
        public float Rotation { get; private set; }
        /// <summary>Rotation when taking into account the parent's rotation</summary>
        public float TrueRotation
        {
            get
            {
                Entity p = Parent;
                float r = Rotation;
                while (p != null)
                {
                    r += p.Rotation;
                    p = p.Parent;
                }
                return r;
            }
        }
        public Vector2 Center { get; private set; }
        public int ZIndex { get; private set; }
        private Vector2 _parallaxScale = new Vector2(1.0f, 1.0f);
        public Vector2 ParallaxScale { get { return _parallaxScale; } private set { _parallaxScale = value; } }
        public Rectangle Bounds { get; private set; }
        public string Texture { get; private set; }
        protected Rectangle SourceRectangle { get; set; }
        private Color _tint = Color.White;
        protected Color Tint { get { return _tint; } private set { _tint = value; } }
        private float _alpha = 1.0f;
        protected float Alpha { get { return _alpha; } set { _alpha = value; } }

        private float fadeOutProgress = -1.0f, fadeOutMaxTime = 1.0f;
        bool deleteOnFadeComplete = false;

        /// <summary>
        /// How this renders
        /// ScreenSpace = Coordinates are respective to the screen
        /// WorldSpace = Coordinates are respective to the world
        /// </summary>
        public ERenderSpace RenderSpace { get; private set; }
        public EMouseState MouseState { get; private set; }

        /// <summary>
        /// Register a component to this entity to ensure components are correctly handled
        /// </summary>
        /// <param name="component">The component to register</param>
        /// <returns>The registered component</returns>
        protected Component RegisterComponent(Component component)
        {
            ComponentManager.RegisterComponent(component, this);
            components.Add(component);
            return component;
        }

        /// <summary>
        /// Removes a component from this entities component registry
        /// </summary>
        /// <param name="component">The component to unregister</param>
        /// <returns>The unregistered component</returns>
        protected Component UnregisterComponent(Component component)
        {
            ComponentManager.UnregisterComponent(component);
            components.Remove(component);
            return component;
        }

        /// <summary>
        /// Spawns a new entity into the world
        /// </summary>
        /// <param name="toSpawn">The entity to spawn</param>
        /// <param name="position">An optional position to set to the entity</param>
        /// <param name="scale">An optional scale to set to the entity</param>
        /// <returns>The spawned entity</returns>
        public Entity Spawn(Entity toSpawn, Vector2? position = null, Vector2? scale = null)
        {
            SceneManager.ActiveScene.Spawn(toSpawn, position, scale);
            return toSpawn;
        }

        /// <summary>
        /// Destroys an entity and removes it from the game
        /// </summary>
        /// <param name="toDelete">The entity to destroy</param>
        public void Destroy(Entity toDelete)
        {
            SceneManager.ActiveScene.Destroy(toDelete);
        }

        /// <summary>
        /// This is called when this entity is spawned
        /// </summary>
        /// <param name="position">An optional position to set this entity to</param>
        /// <param name="scale">An optional scale to set this entity to</param>
        public void DoSpawn(Vector2? position = null, Vector2? scale = null)
        {
            IsAlive = true;
            IsDestroyed = false;

            if (position != null)
                SetPosition((Vector2)position);
            if (scale != null)
                SetScale((Vector2)scale);

            OnSpawn();

            NotifyResolutionChanged(); //Update any components to their correct locations and sizes
        }

        /// <summary>
        /// This is called when this entity is destroyed
        /// </summary>
        public void DoDestroy()
        {
            IsAlive = false;
            IsDestroyed = true;

            for (int i = 0; i < components.Count; ++i)
                ComponentManager.UnregisterComponent(components[i]);

            components.Clear();

            OnDestroy();
        }

        /// <summary>
        /// Called when this entity is updated
        /// </summary>
        /// <param name="delta">The time since the last frame in seconds</param>
        public void Update(float delta)
        {
            if (fadeOutProgress > 0)
            {
                fadeOutProgress -= delta;
                Alpha = Math.Max(0.0f, Math.Min(1.0f, fadeOutProgress / fadeOutMaxTime));
                if (deleteOnFadeComplete)
                {
                    Destroy(this);
                    return;
                }
            }
            OnUpdate(delta);
        }

        /// <summary>
        /// Called when this entity is rendered
        /// </summary>
        /// <param name="delta">The time since the last frame in seconds</param>
        public void Render(float delta)
        {
            OnRender(delta);
        }

        /// <summary>Called when this entity is spawned. You can override this in subclasses</summary>
        protected virtual void OnSpawn() { }
        /// <summary>Called when this entity is destroyed. You can override this in subclasses</summary>
        protected virtual void OnDestroy() { }
        /// <summary>Called when this entity is updated. You can override this in subclasses</summary>
        /// <param name="delta">The time since the last frame in seconds</param>
        protected virtual void OnUpdate(float delta) { }
        /// <summary>Called when this entity is rendered. You can override this in subclasses</summary>
        /// <param name="delta">The time since the last frame in seconds</param>
        protected virtual void OnRender(float delta) { }

        /// <summary>Called once on spawn and any time the resolution is changed</summary>
        public virtual void NotifyResolutionChanged() { }

        /// <summary>Handles mouse input</summary>
        /// <param name="e">A mouse event to prevent clickthrough</param>
        public void HandleMouse(MouseEvent e)
        {
            if (Bounds.Contains(InputManager.GetMousePosition(RenderSpace)))
            {
                if (InputManager.MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (MouseState == EMouseState.Over)
                    {
                        MouseDown(e);
                        MouseState = EMouseState.Down;
                    }
                }
                else
                {
                    if (MouseState == EMouseState.Down)
                        MouseUp(e);
                    else if (MouseState == EMouseState.None)
                        MouseOver(e);

                    MouseState = EMouseState.Over;
                }
            }
            else
            {
                if (MouseState == EMouseState.Down && InputManager.MouseState.LeftButton == ButtonState.Released)
                {
                    MouseState = EMouseState.Over;
                }

                if (MouseState == EMouseState.Over)
                {
                    MouseOut(e);
                    MouseState = EMouseState.None;
                }
            }
        }

        /// <summary>Called when the mouse is over this entity. Overridable in subclasses.</summary>
        /// <param name="e">A mouse event to prevent clickthrough</param>
        public virtual void MouseOver(MouseEvent e) {}
        /// <summary>Called when the mouse moves out from this entity. Overridable in subclasses.</summary>
        /// <param name="e">A mouse event to prevent clickthrough</param>
        public virtual void MouseOut(MouseEvent e) { }
        /// <summary>Called when the left mouse button is pressend when on this entity. Overridable in subclasses.</summary>
        /// <param name="e">A mouse event to prevent clickthrough</param>
        public virtual void MouseDown(MouseEvent e) { }
        /// <summary>Called when the left mouse button is released when on this entity. Overridable in subclasses.</summary>
        /// <param name="e">A mouse event to prevent clickthrough</param>
        public virtual void MouseUp(MouseEvent e) { }

        /// <summary>
        /// Sets how the entity is rendered.
        /// ScreenSpace = Coordinates correspond to the screen
        /// WorldSpace = Coordinates correspond to the world
        /// </summary>
        /// <param name="newRenderSpace">The new renderspace this entity will render in</param>
        /// <returns>This entity</returns>
        public Entity SetRenderSpace(ERenderSpace newRenderSpace)
        {
            RenderSpace = newRenderSpace;
            return this;
        }

        /// <summary>
        /// Sets this entity's parent entity
        /// </summary>
        /// <param name="newParent">The new parent entity</param>
        /// <returns>This entity</returns>
        public Entity SetParent(Entity newParent)
        {
            Parent = newParent;
            return this;
        }

        /// <summary>
        /// Sets this entity's owning player
        /// </summary>
        /// <param name="newOwner">The new owning player</param>
        /// <returns>This entity</returns>
        public Entity SetOwningPlayer(Player newOwner)
        {
            if(OnSetOwningPlayer(newOwner))
                OwningPlayer = newOwner;

            return this;
        }

        /// <summary>
        /// Called just before the owner is set, and can override whether the player is set.
        /// </summary>
        /// <param name="newOwner">The owner that will take over this entity</param>
        /// <returns>True = Allow the new owner, False = disallow</returns>
        public virtual bool OnSetOwningPlayer(Player newOwner) { return true; }

        /// <summary>
        /// Sets the position of the entity
        /// </summary>
        /// <param name="newPosition">The new position to set to this entity</param>
        /// <param name="setCenter">Whether the position corresponds to the center (true) or the top left (false) of the entity bounds</param>
        /// <returns>This entity</returns>
        public Entity SetPosition(Vector2 newPosition, bool setCenter = false)
        {
            return SetPositionScale(newPosition, Scale, setCenter);
        }

        /// <summary>
        /// Sets the size of this entity
        /// </summary>
        /// <param name="newScale">The new size to set to this entity</param>
        /// <returns>This entity</returns>
        public Entity SetScale(Vector2 newScale)
        {
            return SetPositionScale(Position, newScale, false);
        }

        /// <summary>
        /// Sets both the position and scale of this entity
        /// </summary>
        /// <param name="newPosition">The new position to set to this entity</param>
        /// <param name="newScale">The new size to set to this entity</param>
        /// <param name="setCenter">Whether the position corresponds to the center (true) or the top left (false) of the entity bounds</param>
        /// <returns></returns>
        public Entity SetPositionScale(Vector2 newPosition, Vector2 newScale, bool setCenter = false)
        {
            Position = newPosition;
            Scale = newScale;

            if (setCenter)
                Position -= Scale / 2;

            Center = Position + (Scale / 2);
            UpdateBounds();
            return this;
        }

        /// <summary>
        /// Sets the rotation of this entity
        /// </summary>
        /// <param name="newRotation">The new rotation to set to this entity</param>
        /// <returns>This entity</returns>
        public Entity SetRotation(float newRotation)
        {
            Rotation = newRotation;
            return this;
        }

        /// <summary>
        /// Sets the rotation of this entity in degrees
        /// </summary>
        /// <param name="newRotation">The new rotation in degress to set to this entity</param>
        /// <returns>This entity</returns>
        public Entity SetRotationDeg(float newRotation)
        {
            return SetRotation((float)(newRotation / 180 * Math.PI));
        }

        /// <summary>
        /// Sets the z-index of this entity. This is seperate to parallax, and determines which entity draws on top of which.
        /// Higher z-index = More on top
        /// </summary>
        /// <param name="newZIndex">The new z-index to set to this entity</param>
        /// <returns>This entity</returns>
        public Entity SetZIndex(int newZIndex)
        {
            if (ZIndex == newZIndex)
                return this;

            ZIndex = newZIndex;
            if (this is Component)
                ComponentManager.UpdateZ();
            else
                EntityManager.UpdateZ();
            return this;
        }

        /// <summary>
        /// Sets the parallax modifier to this entity. This does not determine the z-index (height ordering) of the entity.
        /// </summary>
        /// <param name="newParallax">The new parallax offset to set to this entity</param>
        /// <returns>This entity</returns>
        public Entity SetParallax(Vector2 newParallax)
        {
            ParallaxScale = newParallax;
            return this;
        }

        /// <summary>
        /// Sets whether this entity is visible or not, optionally with a new alpha value
        /// </summary>
        /// <param name="isVisible">If true, this entity is visible</param>
        /// <param name="newAlpha">Optionally set a new alpha value</param>
        /// <returns>This entity</returns>
        public Entity SetVisible(bool isVisible, float newAlpha = -1.0f)
        {
            IsVisible = isVisible;

            if (newAlpha >= 0.0f && newAlpha <= 1.0f)
            {
                fadeOutProgress = -1;
                Alpha = newAlpha;
            }

            return this;
        }

        /// <summary>
        /// Fades out the entity over a period of time
        /// </summary>
        /// <param name="fadeOutTime">The time taken to fade out the entity</param>
        /// <param name="deleteOnFadeout">Delete this entity when it's fully faded out</param>
        /// <param name="additionalStartTime">How long to wait before fading out</param>
        /// <returns>This entity</returns>
        public Entity FadeOut(float fadeOutTime = 1.0f, bool deleteOnFadeout = false, float additionalStartTime = 0.0f)
        {
            fadeOutMaxTime = fadeOutTime;
            fadeOutProgress = fadeOutTime + additionalStartTime;
            deleteOnFadeComplete = deleteOnFadeout;
            return this;
        }

        /// <summary>
        /// Sets the texture this entity will use to render (If no special rendering is occuring)
        /// </summary>
        /// <param name="texture">The new texture to render</param>
        /// <returns>This entity</returns>
        public Entity SetTexture(string texture)
        {
            Texture = texture;
            return this;
        }

        /// <summary>
        /// Sets the color tint to render this entity
        /// </summary>
        /// <param name="newTint">The new color for the tint</param>
        /// <returns>This entity</returns>
        public Entity SetTint(Color newTint)
        {
            Tint = newTint;
            return this;
        }

        /// <summary>
        /// Sets the alpha value (Opacity) of this entity
        /// </summary>
        /// <param name="newAlpha">A value from 0 to 1 where 1 is fully opaque and 0 is fully transparent</param>
        /// <returns></returns>
        public Entity SetAlpha(float newAlpha)
        {
            Alpha = newAlpha;
            return this;
        }

        private void UpdateBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Scale.X, (int)Scale.Y);
        }

        /// <summary>
        /// Draws the entity with the given texture, bounds, source rectangle, tint and alpha
        /// </summary>
        protected void Draw()
        {
            Draw(GraphicsManager.GetTexture(Texture), Bounds, SourceRectangle, Tint * Alpha);
        }

        /// <summary>
        /// Draws the entity with the given texture, bounds, source rectangle, and alpha. This uses the supplied tint
        /// </summary>
        /// <param name="tint">Custom tint to draw</param>
        protected void Draw(Color tint)
        {
            Draw(GraphicsManager.GetTexture(Texture), Bounds, SourceRectangle, tint * Alpha);
        }

        /// <summary>
        /// Draws the given texture with the specified source and destination rectangles and tint
        /// </summary>
        /// <param name="texture">The texture to render</param>
        /// <param name="destRectangle">The rectangle to draw in the world</param>
        /// <param name="sourceRectangle">The source rectangle, or null</param>
        /// <param name="tint">Tint for drawing</param>
        protected void Draw(Texture2D texture, Rectangle destRectangle, Rectangle? sourceRectangle, Color tint)
        {
            Draw(texture, destRectangle, sourceRectangle, tint, 0, false);
        }

        /// <summary>
        /// Draws the given texture with the specified source and destination rectangles, tint and rotation
        /// </summary>
        /// <param name="texture">The texture to render</param>
        /// <param name="destRectangle">The rectangle to draw in the world</param>
        /// <param name="sourceRectangle">The source rectangle, or null</param>
        /// <param name="tint">Tint for drawing</param>
        /// <param name="rotation">The rotation to render the texture in radians</param>
        /// <param name="rotateFromCenter">True = Rotate from center of texture, False = rotate from top left. Default: true</param>
        protected void Draw(Texture2D texture, Rectangle destRectangle, Rectangle? sourceRectangle, Color tint, float rotation, bool rotateFromCenter = true)
        {
            if (!IsVisible)
                return;

            Rectangle actualRenderRectangle = destRectangle;
            RectangleF cameraBounds = CameraManager.ActiveCamera.GetBounds();
            Vector2 cameraZoom = CameraManager.CameraZoom;
            Vector2 origin = Vector2.Zero;

            if (rotateFromCenter)
            {
                if (sourceRectangle != null)
                    origin = new Vector2((float)sourceRectangle.Value.Width / 2, (float)sourceRectangle.Value.Height / 2);
                else
                    origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
            }

            if (RenderSpace == ERenderSpace.WorldSpace)
            {
                float centerBiasX = 0.0f;
                float centerBiasY = 0.0f;

                if (rotateFromCenter)
                {
                    centerBiasX = actualRenderRectangle.Width / 2;
                    centerBiasY = actualRenderRectangle.Height / 2;
                }

                actualRenderRectangle.X = (int)((actualRenderRectangle.X + centerBiasX - cameraBounds.X * ParallaxScale.X) * cameraZoom.X);
                actualRenderRectangle.Y = (int)((actualRenderRectangle.Y + centerBiasY - cameraBounds.Y * ParallaxScale.Y) * cameraZoom.Y);
                actualRenderRectangle.Width = (int)(cameraZoom.X * actualRenderRectangle.Width);
                actualRenderRectangle.Height = (int)(cameraZoom.Y * actualRenderRectangle.Height);
            }
            else
            {
                actualRenderRectangle.X += (int)(origin.X * actualRenderRectangle.Width);
                actualRenderRectangle.Y += (int)(origin.Y * actualRenderRectangle.Height);
            }

            if (actualRenderRectangle.X + actualRenderRectangle.Width < -Main.Resolution.X ||
                actualRenderRectangle.Y + actualRenderRectangle.Height < -Main.Resolution.Y ||
                actualRenderRectangle.X - Main.Resolution.X > Main.Resolution.X * 2 * cameraZoom.X ||
                actualRenderRectangle.Y - Main.Resolution.Y > Main.Resolution.Y * 2 * cameraZoom.Y)
                return;

            SpriteBatch.Draw(texture, actualRenderRectangle, sourceRectangle, tint, rotation, origin, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a string with the given font at the given position, scaled and coloured.
        /// This text is anchored at the top left of position
        /// </summary>
        /// <param name="font">The font to render the text in</param>
        /// <param name="text">The text to render</param>
        /// <param name="position">The position to render the text</param>
        /// <param name="tint">The color to render the text</param>
        /// <param name="scale">The scale to render the text. Default: 1.0f</param>
        protected void DrawString(string font, string text, Vector2 position, Color tint, float scale = 1.0f)
        {
            DrawString(font, text, position, tint, scale, ETextHorizontalAlign.Left, ETextVerticalAlign.Top);
        }

        /// <summary>
        /// Draws a string with the given font at the given position, scaled and coloured with anchors.
        /// </summary>
        /// <param name="font">The font to render the text in</param>
        /// <param name="text">The text to render</param>
        /// <param name="position">The position to render the text</param>
        /// <param name="tint">The color to render the text</param>
        /// <param name="scale">The scale to render the text. Default: 1.0f</param>
        /// <param name="hAlign">The horizontal anchor</param>
        /// <param name="vAlign">The vertical anchor</param>
        protected void DrawString(string font, string text, Vector2 position, Color tint, float scale, ETextHorizontalAlign hAlign, ETextVerticalAlign vAlign)
        {
            if (!IsVisible)
                return;

            scale *= 0.1f;

            RectangleF cameraBounds = CameraManager.ActiveCamera.GetBounds();
            Vector2 cameraZoom = CameraManager.CameraZoom;
            SpriteFont fontActual = GraphicsManager.GetFont(font);
            Vector2 fontSize = fontActual.MeasureString(text);
            Vector2 scaledFontSize = fontSize * scale;
            Vector2 origin = fontSize / 2;
            Vector2 trueScale;

            if (hAlign == ETextHorizontalAlign.Left)
                position.X += scaledFontSize.X / 2;
            else if (hAlign == ETextHorizontalAlign.Right)
                position.X -= scaledFontSize.X / 2;

            if (vAlign == ETextVerticalAlign.Top)
                position.Y += scaledFontSize.Y / 2;
            else if (vAlign == ETextVerticalAlign.Bottom)
                position.Y -= scaledFontSize.Y / 2;

            if (RenderSpace == ERenderSpace.WorldSpace)
            {
                position.X = (position.X - cameraBounds.X * ParallaxScale.X) * cameraZoom.X;
                position.Y = (position.Y - cameraBounds.Y * ParallaxScale.Y) * cameraZoom.Y;
                trueScale = new Vector2(scale * cameraZoom.X, scale * cameraZoom.Y);
            }
            else
            {
                trueScale = new Vector2(scale, scale);
            }

            SpriteBatch.DrawString(GraphicsManager.GetFont(font), text, position, tint, Rotation, origin, trueScale, SpriteEffects.None, 0);
        }
    }
}
