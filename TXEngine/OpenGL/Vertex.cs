using System.Drawing;

namespace TXEngine.OpenGL;

/// <summary>
///     封装一个顶点
/// </summary>
public class Vertex
{
    /// <summary>
    ///     2D纹理坐标
    /// </summary>
    private float _textureX, _textureY;

    /// <summary>
    ///     2D坐标
    /// </summary>
    private float _x, _y;

    /// <summary>
    ///     无纹理的白色顶点
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    public Vertex(float x, float y) : this(x, y, Color.White)
    {
    }

    /// <summary>
    ///     无纹理的彩色顶点
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="color">颜色</param>
    public Vertex(float x, float y, Color color) : this(x, y, color, -1, -1)
    {
    }

    /// <summary>
    ///     有纹理的彩色顶点
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="color">颜色</param>
    /// <param name="textureX">纹理X坐标</param>
    /// <param name="textureY">纹理Y坐标</param>
    public Vertex(
        float x, float y,
        Color color,
        float textureX, float textureY)
    {
        _x = x;
        _y = y;
        Color = color;
        _textureX = textureX;
        _textureY = textureY;
    }

    /// <summary>
    ///     2D坐标
    ///     （请勿通过Vector2的X、Y属性修改数据）
    /// </summary>
    public Vector2 Position
    {
        get => new(_x, _y);
        set
        {
            _x = value.X;
            _y = value.Y;
        }
    }

    /// <summary>
    ///     颜色
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     2D纹理坐标
    ///     （请勿通过Vector2的X、Y属性修改数据）
    /// </summary>
    public Vector2 TexturePosition
    {
        get => new(_textureX, _textureY);
        set
        {
            _textureX = value.X;
            _textureY = value.Y;
        }
    }
}