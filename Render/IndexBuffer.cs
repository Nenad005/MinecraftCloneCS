using OpenTK.Graphics.OpenGL4;

namespace BasicOpenTK
{
    public class IndexBuffer
    {
        public int id;

        public IndexBuffer(uint[] indices, BufferUsageHint bufferUsageHint)
        {
            id = GL.GenBuffer();
            this.Bind();
            this.Data(indices, bufferUsageHint);
        }

        private void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
        }

        private void Data(uint[] indices, BufferUsageHint BUH)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BUH);
        }

        public void Delete()
        {
            GL.DeleteBuffer(id);
        }
        
        // ~IndexBuffer()
        // {
        //     Delete();
        // }
    }
}