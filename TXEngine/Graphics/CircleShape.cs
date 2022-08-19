using System.Drawing;
using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     圆
/// </summary>
public class CircleShape : Shape
{
    /// <summary>
    ///     顶点数
    ///     由于OpenGL默认不提供圆形渲染，只能通过多个三角形叠加而成
    /// </summary>
    private const int PointCount = 48;

    /// <summary>
    ///     半径
    /// </summary>
    private float _radius;

    public CircleShape(float x, float y, float radius) : this(x, y, radius, Color.White)
    {
    }

    public CircleShape(float x, float y, float radius, Color color, BaseTexture? texture = null)
        : base(x, y, color, texture)
    {
        _radius = radius;
        UpdateVertices();
    }

    /// <summary>
    ///     半径
    /// </summary>
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            UpdateVertices();
        }
    }

    public override void Draw(Shader shader)
    {
        VertexBuffer.SetupDraw();
        IndexBuffer.Bind();
        Texture?.Bind();

        GL.DrawElements(PrimitiveType.TriangleFan, IndexBuffer.Count, DrawElementsType.UnsignedInt, 0);

        Texture?.UnBind();
        VertexBuffer.FinishDraw();
        IndexBuffer.UnBind();
    }

    protected override void UpdateVertices()
    {
        var vertices = new Vertex[PointCount + 1];

        for (var i = 0; i < PointCount; i++)
        {
            var angle = i * 2.0 / PointCount * Math.PI;
            var c = Math.Cos(angle);
            var s = Math.Sin(angle);
            var x = c * _radius;
            var y = s * _radius;

            vertices[i] = Texture == null
                ? new Vertex(
                    (float)(_x + x),
                    (float)(_y + y),
                    _color
                )
                : new Vertex(
                    (float)(_x + x),
                    (float)(_y + y),
                    _color,
                    (float)(c / 2 + 0.5),
                    (float)(-s / 2 + 0.5)
                );
        }

        vertices[^1] = Texture == null
            ? new Vertex(_x, _y, _color)
            : new Vertex(_x, _y, _color, 0.5f, 0.5f);

        var indies = new int[vertices.Length + 1];
        indies[0] = PointCount;
        for (var i = 1; i < vertices.Length; i++) indies[i] = i - 1;

        indies[^1] = 0;

        VertexBuffer.AttachData(vertices);
        IndexBuffer.AttachData(indies);
    }
}