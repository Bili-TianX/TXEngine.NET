using TXEngine.Util;

namespace TXEngine.OpenGL;

/// <summary>
///     顶点缓冲对象
/// </summary>
public class VertexBuffer : BaseBuffer
{
    private readonly VertexArrayBuffer _vertexArrayBuffer;

    public VertexBuffer()
    {
        Buffer = GL.GenBuffer();
        _vertexArrayBuffer = new VertexArrayBuffer();
    }

    public override void Dispose()
    {
        base.Dispose();
        _vertexArrayBuffer.Dispose();
    }

    /// <summary>
    ///     将float数据发送至显卡
    /// </summary>
    /// <param name="data">数据</param>
    public void AttachData(float[] data)
    {
        Bind();
        _vertexArrayBuffer.Bind();

        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

        // Position
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        // Color
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        // Texture
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        _vertexArrayBuffer.UnBind();
        UnBind();
    }

    /// <summary>
    ///     将顶点转换为float数组，并将数据发送至显卡
    /// </summary>
    /// <param name="vertices">顶点数组</param>
    public void AttachData(Vertex[] vertices)
    {
        float[] data = new float[8 * vertices.Length];
        int i = 0;

        foreach (Vertex vertex in vertices)
        {
            Vector2 vertexPosition = vertex.Position;
            (float r, float g, float b, float a) = vertex.Color.Normalize();
            Vector2 vertexTexturePosition = vertex.TexturePosition;

            data[i++] = vertexPosition.X;
            data[i++] = vertexPosition.Y;

            data[i++] = r;
            data[i++] = g;
            data[i++] = b;
            data[i++] = a;

            data[i++] = vertexTexturePosition.X;
            data[i++] = vertexTexturePosition.Y;
        }

        AttachData(data);
    }

    /// <summary>
    ///     初始化绘制
    /// </summary>
    public void SetupDraw()
    {
        _vertexArrayBuffer.Bind();
    }

    /// <summary>
    ///     完成绘制
    /// </summary>
    public void FinishDraw()
    {
        _vertexArrayBuffer.UnBind();
    }

    public override void Bind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
    }

    public override void UnBind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
    }
}