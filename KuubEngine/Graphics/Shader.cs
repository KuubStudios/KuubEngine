using System;
using System.Diagnostics;

using KuubEngine.Diagnostics;
using KuubEngine.Utility;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class Shader : IDisposable {
        private int id;
        public int ID {
            get {
                if(id == 0) id = GL.CreateShader(Type);
                return id;
            }
        }

        public ShaderType Type { get; protected set; }

        /// <summary>
        ///     Creates a new Shader of ShaderType type
        /// </summary>
        /// <param name="type"></param>
        public Shader(ShaderType type) {
            Type = type;
        }

        /// <summary>
        ///     Creates a new Shader of ShaderType type and compiles source
        /// </summary>
        /// <param name="type"></param>
        /// <param name="source"></param>
        public Shader(ShaderType type, string source) : this(type) {
            Load(source);
        }

#if DEBUG
        ~Shader() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public void Dispose() {
            if(id != 0) {
                GL.DeleteShader(id);
                id = 0;
            }
        }

        public override string ToString() {
            return "{0} {1}".Format(Type, id);
        }

        /// <summary>
        ///     Load and compile sourcecode
        /// </summary>
        /// <param name="source">GLSL sourcecode</param>
        public void Load(string source) {
            GL.ShaderSource(ID, source);
            GL.CompileShader(ID);

            int status;
            GL.GetShader(ID, ShaderParameter.CompileStatus, out status);
            if(status != 1) throw new ShaderCompileException(GL.GetShaderInfoLog(ID));

            int length;
            GL.GetShader(ID, ShaderParameter.InfoLogLength, out length);
            if(length > 1) Log.Warn("{0} info log not empty:\n\t{1}", this, GL.GetShaderInfoLog(ID).TrimEnd('\n').Replace("\n", "\n\t"));
        }

        /// <summary>
        ///     Attaches this Shader to a ShaderProgram
        /// </summary>
        /// <param name="program"></param>
        public void Attach(ShaderProgram program) {
            GL.AttachShader(program.ID, ID);
        }
    }
}