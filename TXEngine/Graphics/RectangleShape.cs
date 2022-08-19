using System.Drawing;
using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     矩形
/// </summary>
public class RectangleShape : Shape
{
    /// <summary>
    ///     大小（宽、高）
    /// </summary>
    private float _width, _height;

    public RectangleShape(float x, float y, float width, float height)
        : this(x, y, width, height, Color.White)
    {
    }

    public RectangleShape(float x, float y, float width, float height, Color color, BaseTexture? texture = null)
        : base(x, y, color, texture)
    {
        _width = width;
        _height = height;

        UpdateVertices();
    }

    /// <summary>
    ///     大小
    /// </summary>
    public Vector2 Size
    {
        get => new(_width, _height);
        set
        {
            _width = value.X;
            _height = value.Y;

            UpdateVertices();
        }
    }

    public override void Draw(Shader shader)
    {
        Texture?.Bind();
        VertexBuffer.SetupDraw();
        IndexBuffer.Bind();

        GL.DrawElements(PrimitiveType.Triangles, IndexBuffer.Count, DrawElementsType.UnsignedInt, 0);

        Texture?.UnBind();
        VertexBuffer.FinishDraw();
        IndexBuffer.UnBind();
    }

    protected override void UpdateVertices()
    {
        var vertices = Texture == null
            ? new[]
            {
                new Vertex(_x, _y, _color),
                new Vertex(_x, _y + _height, _color),
                new Vertex(_x + _width, _y + _height, _color),
                new Vertex(_x + _width, _y, _color)
            }
            : new[]
            {
                new Vertex(_x, _y, _color, 0, 0),
                new Vertex(_x, _y + _height, _color, 0, 1),
                new Vertex(_x + _width, _y + _height, _color, 1, 1),
                new Vertex(_x + _width, _y, _color, 1, 0)
            };

        var indices = new[]
        {
            0, 1, 2,
            0, 3, 2
        };

        VertexBuffer.AttachData(vertices);
        IndexBuffer.AttachData(indices);
    }
}