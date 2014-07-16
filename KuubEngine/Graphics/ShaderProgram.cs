using System;
using System.Diagnostics;

using KuubEngine.Diagnostics;
using KuubEngine.Utility;

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
            Log.Debug("Program {0} linked with {1} attributes", ID, attributes);
        }

        /// <summary>
        ///     Start using this ShaderProgram for drawing
        /// </summary>
        public void Use() {
            if(Current == this) return;

            GL.UseProgram(ID);
            Current = this;
        }
    }
}