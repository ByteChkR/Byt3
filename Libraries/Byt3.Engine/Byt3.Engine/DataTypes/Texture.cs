using Byt3.Utilities.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace Byt3.Engine.DataTypes
{
    /// <summary>
    /// A Data Type that is containing the information that is needed to store a Texture in OpenGL
    /// </summary>
    public class Texture : DisposableGLObjectBase
    {
        /// <summary>
        /// Private flag to keep from disposing the texture twice
        /// </summary>
        public bool IsDisposed;

        private bool dontDispose;

        private readonly long bytes;

        /// <summary>
        /// Internal Constructor to Create a Texture Object from a GL Texture Handle
        /// </summary>
        /// <param name="textureId"></param>
        /// <param name="bytes">Size of the Texture(for statistics)</param>
        internal Texture(int textureId, long bytes, bool isCopy, object handleIdentifier) : base(isCopy,
            handleIdentifier)
        {
            this.bytes = bytes;
            TextureId = textureId;
        }

        /// <summary>
        /// The Texture type used to automatically bind textures to the right Uniforms in the shader
        /// None -> Diffuse
        /// </summary>
        public TextureType TexType { get; set; }

        /// <summary>
        /// The OpenGL Handle to the texture
        /// </summary>
        public int TextureId { get; private set; }

        /// <summary>
        /// Convenient Wrapper to query the texture width
        /// </summary>
        public float Width
        {
            get
            {
                if (IsDisposed)
                {
                    throw new Byt3Exception("Use of Disposed Texture");
                }

                GL.BindTexture(TextureTarget.Texture2D, TextureId);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out float width);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                return width;
            }
        }

        /// <summary>
        /// Convenient Wrapper to query the texture height
        /// </summary>
        public float Height
        {
            get
            {
                if (IsDisposed)
                {
                    throw new Byt3Exception("Use of Disposed Texture");
                }

                GL.BindTexture(TextureTarget.Texture2D, TextureId);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight,
                    out float height);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                return height;
            }
        }

        /// <summary>
        /// Disposed Implementation to free the texture memory when it is no longer in use.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (!IsDisposed && !dontDispose)
            {
                GL.DeleteTexture(TextureId);
                TextureId = -1;
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Returns a Copy of this Mesh(It Does not copy the buffers on the GPU)
        /// It is save to Dispose the copied object.
        /// BUT NEVER DISPOSE THE SOURCE OBJECT BEFORE ALL COPIES HAVE BEEN DISPOSED, otherwise they become unusable and will probably crash the engine once interacted with
        /// </summary>
        /// <returns>A Copy of this Mesh Object</returns>
        public Texture Copy()
        {
            return new Texture(TextureId, bytes, true, HandleIdentifier)
            {
                dontDispose = true
            };
        }
    }
}