using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace BasicOpenTK
{
    class Texture
    {
        public int id;

        public Texture(ref System.Drawing.Imaging.BitmapData image, ref Shaders shader, int slot)
        {
            texUnit(shader, slot);
            id = GL.GenTexture();
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, image.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        }

        public void texUnit(Shaders shader, int slot)
        {
            shader.SetUniformSampler2d("ourTexture", slot);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Delete()
        {
            GL.DeleteTexture(id);
        }
    }
}