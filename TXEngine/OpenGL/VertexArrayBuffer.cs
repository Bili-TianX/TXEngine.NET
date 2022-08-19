namespace TXEngine.OpenGL;

public class VertexArrayBuffer : BaseBuffer
{
    public VertexArrayBuffer()
    {
        Buffer = GL.GenVertexArray();
    }

    public override void Bind()
    {
        GL.BindVertexArray(Buffer);
    }

    public override void UnBind()
    {
        GL.BindVertexArray(0);
    }

    public override void Dispose()
    {
        GL.DeleteVertexArray(Buffer);
    }
}