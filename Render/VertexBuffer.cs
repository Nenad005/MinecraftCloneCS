using OpenTK.Graphics.OpenGL4;

namespace BasicOpenTK
{
    public class VertexBuffer
    {
        public int id;

        public VertexBuffer(float[] vertices, BufferUsageHint bufferUsageHint)
        {
            id = GL.GenBuffer();
            Bind();
            Data(vertices, bufferUsageHint);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void Data(float[] vertices, BufferUsageHint BUH)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BUH);
        }

        public void Delete()
        {
            GL.DeleteBuffer(id);
        }

        // ~VertexBuffer()
        // {
        //     Delete();
        // }
    }
}