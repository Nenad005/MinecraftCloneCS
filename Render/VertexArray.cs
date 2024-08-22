using OpenTK.Graphics.OpenGL4;

namespace BasicOpenTK
{
    class VertexArray
    {
        public int id;

        public VertexArray()
        {
            id = GL.GenVertexArray();
        }

        public void LinkVBO(ref VertexBuffer vbo)
        {
            vbo.Bind();
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            vbo.Unbind();
        }

        public void Bind()
        {
            GL.BindVertexArray(id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteVertexArray(id);
        }
    }
}
