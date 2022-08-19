using TXEngine.Core;

namespace TXEngine.OpenGL;

/// <summary>
///     所有OpenGL缓冲对象的基类
/// </summary>
public abstract class BaseBuffer : IBind, IDisposable
{
    protected int Buffer;

    public abstract void Bind();

    public abstract void UnBind();

    /// <summary>
    ///     清理Buffer对象
    /// </summary>
    public virtual void Dispose()
    {
        GL.DeleteBuffer(Buffer);
    }
}