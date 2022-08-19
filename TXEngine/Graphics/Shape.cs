using System.Drawing;
using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     封装几何图形
/// </summary>
public abstract class Shape : TextureTarget, IDisposable
{
    protected readonly IndexBuffer IndexBuffer;
    protected readonly VertexBuffer VertexBuffer;

    /// <summary>
    ///     颜色
    /// </summary>
    protected Color _color;

    /// <summary>
    ///     坐标
    /// </summary>
    protected float _x, _y;

    protected Shape(float x, float y, Color color, BaseTexture? texture = null)
    {
        _x = x;
        _y = y;
        _color = color;
        _texture = texture;

        VertexBuffer = new VertexBuffer();
        IndexBuffer = new IndexBuffer();
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

            UpdateVertices();
        }
    }

    /// <summary>
    ///     颜色
    /// </summary>
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            UpdateVertices();
        }
    }

    /// <summary>
    ///     销毁所有资源
    /// </summary>
    public void Dispose()
    {
        IndexBuffer.Dispose();
        VertexBuffer.Dispose();
    }

    /// <summary>
    ///     更新所有顶点
    /// </summary>
    protected abstract void UpdateVertices();

    public override void Update()
    {
        UpdateVertices();
    }
}