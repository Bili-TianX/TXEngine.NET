using TXEngine.Core;
using TXEngine.Util;

namespace TXEngine.OpenGL;

/// <summary>
///     图片纹理
/// </summary>
public class ImageTexture : BaseTexture, IFileLoader
{
    /// <summary>
    ///     从图形文件中加载纹理
    /// </summary>
    /// <param name="filename">文件名</param>
    public void LoadFromFile(string filename)
    {
        (byte[] pixels, int width, int height) = ImageUtil.LoadFromFile(filename);
        LoadFromMemory(pixels, width, height);
    }

    public override void LoadFromMemory(byte[] data, int width, int height)
    {
        Bind();

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            width, height, 0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            data
        );
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        UnBind();
    }
}