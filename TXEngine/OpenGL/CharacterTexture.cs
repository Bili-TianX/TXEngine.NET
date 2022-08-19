using TXEngine.Graphics;
using TextureTarget = OpenTK.Graphics.OpenGL4.TextureTarget;

namespace TXEngine.OpenGL;

/// <summary>
///     字符纹理（仅用于<see cref="Text" />）
/// </summary>
internal class CharacterTexture : BaseTexture
{
    /// <summary>
    ///     宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    ///     高度
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    ///     偏移量
    /// </summary>
    public Vector2 Offset { get; set; }

    public float Advance { get; set; }

    public override void LoadFromMemory(byte[] data, int width, int height)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        Width = width;
        Height = height;

        Bind();
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.R8,
            width, height, 0,
            PixelFormat.Red,
            PixelType.UnsignedByte,
            data
        );
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        UnBind();
    }
}