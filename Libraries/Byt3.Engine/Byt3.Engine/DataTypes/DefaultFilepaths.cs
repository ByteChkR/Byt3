using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Byt3.ADL;
using Byt3.Callbacks;
using Byt3.Engine.Debug;
using Byt3.Engine.Exceptions;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using Byt3.Engine.UI;
using OpenTK.Graphics.OpenGL;

namespace Byt3.Engine.DataTypes
{
    /// <summary>
    /// A Class that has all default resources as fields
    /// </summary>
    [Serializable]
    public static class DefaultFilepaths
    {
        private static readonly ADLLogger<DebugChannel> Logger =
            new ADLLogger<DebugChannel>(EngineDebugConfig.Settings, "DefaultFilepaths");
        

        public static void DisposeObjects()
        {
            _defaultFont?.Dispose();
            _defaultFont = null;
            _defaultMesh?.Dispose();
            _defaultMesh = null;
            _defaultTexture?.Dispose();
            _defaultTexture = null;
            _defaultLitShader?.Dispose();
            _defaultLitShader = null;
            _defaultUnlitShader?.Dispose();
            _defaultUnlitShader = null;
            _defaultUiTextShader?.Dispose();
            _defaultUiTextShader = null;
            _defaultUiImageShader?.Dispose();
            _defaultUiImageShader = null;
            _defaultUiGraphShader?.Dispose();
            _defaultUiGraphShader = null;
            _defaultMergeMulShader?.Dispose();
            _defaultMergeMulShader = null;
            _defaultMergeAddShader?.Dispose();
            _defaultMergeAddShader = null;
            _defaultScreenShader?.Dispose();
            _defaultScreenShader = null;
        }
        

        #region Private Static Fields

        /// <summary>
        /// Backing field for the default font
        /// </summary>
        private static GameFont _defaultFont;

        /// <summary>
        /// The Backing field of the default mesh
        /// </summary>
        private static Mesh _defaultMesh;

        /// <summary>
        /// Backing field for the default texture
        /// </summary>
        private static Texture _defaultTexture;


        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultUnlitShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultLitShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultUiTextShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultUiImageShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultUiGraphShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultMergeMulShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultMergeAddShader;

        /// <summary>
        /// Backing field of the Default shader
        /// </summary>
        private static ShaderProgram _defaultScreenShader;

        private static Dictionary<ShaderType, string> DefaultLitShaderPath =>
            GetDictionary(defaultLitShader);

        private static Dictionary<ShaderType, string> DefaultUnlitShaderPath =>
            GetDictionary(defaultUnlitShader);

        private static Dictionary<ShaderType, string> DefaultUiTextShaderPath =>
            GetDictionary(defaultUiTextShader);

        private static Dictionary<ShaderType, string> DefaultUiImageShaderPath =>
            GetDictionary(defaultUiImageShader);

        private static Dictionary<ShaderType, string> DefaultUiGraphShaderPath =>
            GetDictionary(defaultUiGraphShader);

        private static Dictionary<ShaderType, string> DefaultMergeAddShaderPath =>
            GetDictionary(defaultMergeAddShader);

        private static Dictionary<ShaderType, string> DefaultMergeMulShaderPath =>
            GetDictionary(defaultMergeMulShader);

        private static Dictionary<ShaderType, string> DefaultScreenShaderPath =>
            GetDictionary(defaultScreenShader);

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Path to the Default Mesh
        /// </summary>
        public static string DefaultMeshPath => defaultMesh;

        /// <summary>
        /// Path to the Default Texture
        /// </summary>
        public static string DefaultTexturePath => defaultTexture;

        /// <summary>
        /// Path to the Default Font
        /// </summary>
        public static string DefaultFontPath => defaultFont;

        /// <summary>
        /// The default font
        /// </summary>
        public static GameFont DefaultFont => _defaultFont ?? (_defaultFont = GetDefaultFont());

        /// <summary>
        /// The Default Mesh
        /// </summary>
        public static Mesh DefaultMesh => _defaultMesh ?? (_defaultMesh = GetDefaultMesh());


        /// <summary>
        /// Path to the Default Texture
        /// </summary>
        public static Texture DefaultTexture => _defaultTexture ?? (_defaultTexture = GetDefaultTexture());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultUnlitShader =>
            _defaultUnlitShader ?? (_defaultUnlitShader = GetDefaultUnlitShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultLitShader =>
            _defaultLitShader ?? (_defaultLitShader = GetDefaultLitShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultUiTextShader =>
            _defaultUiTextShader ?? (_defaultUiTextShader = GetDefaultUiTextShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultUiImageShader =>
            _defaultUiImageShader ?? (_defaultUiImageShader = GetDefaultUiImageShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultUiGraphShader =>
            _defaultUiGraphShader ?? (_defaultUiGraphShader = GetDefaultUiGraphShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultMergeMulShader =>
            _defaultMergeMulShader ?? (_defaultMergeMulShader = GetDefaultMergeMulShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultMergeAddShader =>
            _defaultMergeAddShader ?? (_defaultMergeAddShader = GetDefaultMergeAddShader());

        /// <summary>
        /// The default shader
        /// </summary>
        public static ShaderProgram DefaultScreenShader =>
            _defaultScreenShader ?? (_defaultScreenShader = GetDefaultScreenShader());

        #endregion

        #region Private Properties

        private static string defaultFont = "assets/fonts/default_font.ttf";


        private static List<ShaderPath> defaultLitShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/lit/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/lit/shader.vs"}
        };

        private static List<ShaderPath> defaultMergeAddShader = new List<ShaderPath>
        {
            new ShaderPath
                {Type = ShaderType.FragmentShader, Path = "assets/shader/internal/merge_stage/merge_shader_add.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/internal/merge_stage/merge_shader.vs"}
        };

        private static List<ShaderPath> defaultMergeMulShader = new List<ShaderPath>
        {
            new ShaderPath
                {Type = ShaderType.FragmentShader, Path = "assets/shader/internal/merge_stage/merge_shader_mul.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/internal/merge_stage/merge_shader.vs"}
        };

        private static string defaultMesh = "assets/models/default_mesh.obj";

        private static List<ShaderPath> defaultScreenShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/internal/screen_stage/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/internal/screen_stage/shader.vs"}
        };

        private static string defaultTexture = "assets/textures/default_texture.bmp";

        private static List<ShaderPath> defaultUiGraphShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/ui/graph/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/ui/graph/shader.vs"}
        };

        private static List<ShaderPath> defaultUiImageShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/ui/image/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/ui/image/shader.vs"}
        };

        private static List<ShaderPath> defaultUiTextShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/ui/text/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/ui/text/shader.vs"}
        };

        private static List<ShaderPath> defaultUnlitShader = new List<ShaderPath>
        {
            new ShaderPath {Type = ShaderType.FragmentShader, Path = "assets/shader/unlit/shader.fs"},
            new ShaderPath {Type = ShaderType.VertexShader, Path = "assets/shader/unlit/shader.vs"}
        };

        #endregion

        private static Dictionary<ShaderType, string> GetDictionary(List<ShaderPath> list)
        {
            Dictionary<ShaderType, string> ret = new Dictionary<ShaderType, string>();
            for (int i = 0; i < list.Count; i++)
            {
                ret.Add(list[i].Type, list[i].Path);
            }

            return ret;
        }
        

        #region Private Static Helper Functions

        /// <summary>
        /// Creates the default font from embedded program resources
        /// </summary>
        /// <returns>The Default font</returns>
        private static GameFont GetDefaultFont()
        {
            return FontLibrary.LoadFontInternal(IOManager.GetStream(DefaultFontPath), 32, out string name);
        }

        /// <summary>
        /// Creates the default mesh from embedded program resources
        /// </summary>
        /// <returns>The Default mesh</returns>
        private static Mesh GetDefaultMesh()
        {
            return MeshLoader.LoadModel(IOManager.GetStream(DefaultMeshPath), "DefaultMesh",
                Path.GetExtension(DefaultMeshPath))[0];
        }

        /// <summary>
        /// Creates the default Texture from embedded program resources
        /// </summary>
        /// <returns>The Default Texture</returns>
        private static Texture GetDefaultTexture()
        {
            return TextureLoader.BitmapToTexture(new Bitmap(IOManager.GetStream(DefaultTexturePath)), "DefaultTexture");
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultUnlitShader()
        {
            ShaderProgram.TryCreate(DefaultUnlitShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultLitShader()
        {
            ShaderProgram.TryCreate(DefaultLitShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultUiTextShader()
        {
            ShaderProgram.TryCreate(DefaultUiTextShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultUiImageShader()
        {
            ShaderProgram.TryCreate(DefaultUiImageShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultUiGraphShader()
        {
            ShaderProgram.TryCreate(DefaultUiGraphShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultMergeMulShader()
        {
            ShaderProgram.TryCreate(DefaultMergeMulShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultMergeAddShader()
        {
            ShaderProgram.TryCreate(DefaultMergeAddShaderPath, out ShaderProgram shader);
            return shader;
        }

        /// <summary>
        /// Creates the default Shader from embedded program resources
        /// </summary>
        /// <returns>The Default Shader</returns>
        private static ShaderProgram GetDefaultScreenShader()
        {
            ShaderProgram.TryCreate(DefaultScreenShaderPath, out ShaderProgram shader);
            return shader;
        }

        #endregion
    }
}