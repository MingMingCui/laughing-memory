using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
    /// <summary>
    ///   <para>Renders a Sprite for 2D graphics.</para>
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public sealed class SpriteRendererExpand : Renderer
    {
        /// <summary>
        ///   <para>The Sprite to render.</para>
        /// </summary>
        public Sprite sprite
        {
            get
            {
                return this.GetSprite_INTERNAL();
            }
            set
            {
                this.SetSprite_INTERNAL(value);
            }
        }

        /// <summary>
        ///   <para>The current draw mode of the Sprite Renderer.</para>
        /// </summary>
        public extern SpriteDrawMode drawMode
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        internal extern bool shouldSupportTiling
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        ///   <para>Property to set/get the size to render when the SpriteRenderer.drawMode is set to SpriteDrawMode.NineSlice.</para>
        /// </summary>
        public Vector2 size
        {
            get
            {
                Vector2 result;
                this.INTERNAL_get_size(out result);
                return result;
            }
            set
            {
                this.INTERNAL_set_size(ref value);
            }
        }

        /// <summary>
        ///   <para>The current threshold for Sprite Renderer tiling.</para>
        /// </summary>
        public extern float adaptiveModeThreshold
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        ///   <para>The current tile mode of the Sprite Renderer.</para>
        /// </summary>
        public extern SpriteTileMode tileMode
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        ///   <para>Rendering color for the Sprite graphic.</para>
        /// </summary>
        public Color color
        {
            get
            {
                Color result;
                this.INTERNAL_get_color(out result);
                return result;
            }
            set
            {
                this.INTERNAL_set_color(ref value);
            }
        }

        /// <summary>
        ///   <para>Flips the sprite on the X axis.</para>
        /// </summary>
        public extern bool flipX
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        ///   <para>Flips the sprite on the Y axis.</para>
        /// </summary>
        public extern bool flipY
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        ///   <para>Specifies how the sprite interacts with the masks.</para>
        /// </summary>
        public extern SpriteMaskInteraction maskInteraction
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_size(out Vector2 value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_size(ref Vector2 value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern Sprite GetSprite_INTERNAL();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void SetSprite_INTERNAL(Sprite sprite);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_color(out Color value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_color(ref Color value);

        internal Bounds GetSpriteBounds()
        {
            Bounds result;
            INTERNAL_CALL_GetSpriteBounds(this, out result);
            return result;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_GetSpriteBounds(SpriteRendererExpand self, out Bounds value);
    }
}
