namespace TXEngine.OpenGL;

/// <summary>
///     所有种类的纹理的基类
/// </summary>
public abstract class BaseTexture : BaseBuffer
{
    public BaseTexture()
    {
        Buffer = GL.GenTexture();
    }


    /// <summary>
    ///     从内存中加载纹理
    /// </summary>
    /// <param name="data">byte格式的数据</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    public abstract void LoadFromMemory(byte[] data, int width, int height);

    public override void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, Buffer);
    }

    public override void UnBind()
    {
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    ///     清理纹理
    /// </summary>
    public override void Dispose()
    {
        GL.DeleteTexture(Buffer);
    }
}