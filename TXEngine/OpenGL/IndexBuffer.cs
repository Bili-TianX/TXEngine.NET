namespace TXEngine.OpenGL;

/// <summary>
///     索引缓冲对象
/// </summary>
public class IndexBuffer : BaseBuffer
{
    public IndexBuffer()
    {
        Buffer = GL.GenBuffer();
    }

    /// <summary>
    ///     顶点个数（需调用过AttachData方法）
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    ///     将顶点索引发送至显卡
    /// </summary>
    /// <param name="indices"></param>
    public void AttachData(int[] indices)
    {
        Bind();

        Count = indices.Length;
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            Count * sizeof(int),
            indices,
            BufferUsageHint.StaticDraw
        );
    }

    public override void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, Buffer);
    }

    public override void UnBind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }
}