using System.Diagnostics;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Utility {
    public static class Util {
        [Conditional("DEBUG")]
        public static void CheckGLError() {
            ErrorCode error = GL.GetError();
            if(error != ErrorCode.NoError) throw new GraphicsErrorException("GL.GetError() returned " + error);
        }

        [Conditional("DEBUG")]
        public static void CheckALError() {
            ALError error = AL.GetError();
            if(error != ALError.NoError) throw new AudioException("AL.GetError() returned " + error);
        }
    }
}