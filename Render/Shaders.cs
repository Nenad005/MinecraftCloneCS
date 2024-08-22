using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BasicOpenTK
{
    class Shaders
    {
        public int id 
        { 
            get
            {
                return sp.id;
            }
        }

        private ShaderProgram sp{ get; set; }

        public Shaders(string vertexShaderLocation, string fragmentShaderLocation)
        {
            sp = loadShaderProgram(vertexShaderLocation, fragmentShaderLocation);
        }

        private static Shader loadShader(string shaderLocation, ShaderType type)
        {
            int shaderId = GL.CreateShader(type);
            GL.ShaderSource(shaderId, File.ReadAllText(shaderLocation));
            GL.CompileShader(shaderId);
            string shaderInfoLog = GL.GetShaderInfoLog(shaderId);

            if (!string.IsNullOrEmpty(shaderInfoLog))
            {
                throw new Exception(shaderInfoLog);
            }
            return new Shader() { id = shaderId };
        }

        public static ShaderProgram loadShaderProgram(string vertexShaderLocation, string fragmentShaderLocation)
        {
            int shaderProgramId = GL.CreateProgram();
            Shader vertexShader = loadShader(vertexShaderLocation, ShaderType.VertexShader);
            Shader fragmentShader = loadShader(fragmentShaderLocation, ShaderType.FragmentShader);

            GL.AttachShader(shaderProgramId, vertexShader.id);
            GL.AttachShader(shaderProgramId, fragmentShader.id);
            GL.LinkProgram(shaderProgramId);
            GL.DetachShader(shaderProgramId, vertexShader.id);
            GL.DetachShader(shaderProgramId, fragmentShader.id);
            GL.DeleteShader(vertexShader.id);
            GL.DeleteShader(fragmentShader.id);

            string infoLog = GL.GetProgramInfoLog(shaderProgramId);

            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }
            return new ShaderProgram() { id = shaderProgramId };
        }

        public void SetUniformMat4f(string name, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(GetUniformLocationS(name), false, ref matrix);
        }

        public void SetUniformSampler2d(string name, int sampler)
        {
            GL.Uniform1(GetUniformLocationS(name), sampler);
        }

        private int GetUniformLocationS(string name)
        {
            int location = GL.GetUniformLocation(sp.id, name);
            return location;
        }

        public void Delete()
        {
            GL.DeleteProgram(sp.id);
        }

        public void Activate()
        {
            GL.UseProgram(sp.id);
        }

        public struct ShaderProgram
        {
            public int id;
        }

        public struct Shader
        {
            public int id;
        }
    }
}