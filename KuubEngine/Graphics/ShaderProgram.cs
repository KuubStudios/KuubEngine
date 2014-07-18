using System;
using System.Diagnostics;

using KuubEngine.Diagnostics;
using KuubEngine.Utility;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class ShaderProgram : IDisposable {
        /// <summary>
        ///     The ShaderProgram that is currently in use
        /// </summary>
        public static ShaderProgram Current;

        private int id;
        public int ID {
            get {
                if(id == 0) id = GL.CreateProgram();
                return id;
            }
        }

#if DEBUG
        ~ShaderProgram() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public void Dispose() {
            if(Current == this) Clear();
            if(id != 0) {
                GL.DeleteProgram(ID);
                id = 0;
            }
        }

        /// <summary>
        ///     Reset the current ShaderProgram being used
        /// </summary>
        public static void Clear() {
            GL.UseProgram(0);
            Current = null;
        }

        /// <summary>
        ///     Links the ShaderProgram after attaching all Shaders
        /// </summary>
        public void Link() {
            int status;

            GL.LinkProgram(ID);
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out status);
            if(status != 1) throw new ShaderLinkException(GL.GetProgramInfoLog(ID));

#if DEBUG
            GL.ValidateProgram(ID);
            GL.GetProgram(ID, GetProgramParameterName.ValidateStatus, out status);
            if(status != 1) throw new ShaderValidateException(GL.GetProgramInfoLog(ID));
#endif

            int length;
            GL.GetProgram(ID, GetProgramParameterName.InfoLogLength, out length);
            if(length > 1) Log.Warn("ShaderProgram {0} info log not empty:\n\t{1}", ID, GL.GetProgramInfoLog(ID));

            int attributes;
            GL.GetProgram(ID, GetProgramParameterName.ActiveAttributes, out attributes);
            Log.Debug("Program {0} linked with {1} attributes:", ID, attributes);
            for(int i = 0; i < attributes; i++) {
                int size;
                ActiveAttribType type;
                string attrib = GL.GetActiveAttrib(ID, i, out size, out type);
                Log.Debug("\t{0}: {1} {2} {3}", i, size, type, attrib);
            }
        }

        /// <summary>
        ///     Start using this ShaderProgram for drawing
        /// </summary>
        public void Use() {
            if(Current == this) return;

            GL.UseProgram(ID);
            Current = this;
        }

        public int GetAttribLocation(string name) {
            int index = GL.GetAttribLocation(ID, name);
            if(index < 0) throw new ArgumentException("Couldn't find attrib location", "name");
            return index;
        }

        public int GetUniformLocation(string name) {
            int index = GL.GetUniformLocation(ID, name);
            if(index < 0) throw new ArgumentException("Couldn't find uniform location", "name");
            return index;
        }

        public void SetUniform1(string name, float value) {
            Use();
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void SetUniform1(string name, int value) {
            Use();
            GL.Uniform1(GetUniformLocation(name), value);  
        }

        public void SetUniform2(string name, Vector2 value) {
            Use();
            GL.Uniform2(GetUniformLocation(name), value);
        }

        public void SetUniform3(string name, Vector3 value) {
            Use();
            GL.Uniform3(GetUniformLocation(name), value);
        }

        public void SetUniform4(string name, Vector4 value) {
            Use();
            GL.Uniform4(GetUniformLocation(name), value);
        }
    }
}