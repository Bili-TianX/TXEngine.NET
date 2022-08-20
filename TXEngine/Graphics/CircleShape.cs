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
    private static IndexBuffer? _indexBuffer;


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

        if (_indexBuffer == null)
        {
            _indexBuffer = new IndexBuffer();
            int[] indices = new int[PointCount + 2];
            indices[0] = PointCount;
            for (int i = 1; i < PointCount + 1; i++)
            {
                indices[i] = i - 1;
            }

            indices[^1] = 0;
            _indexBuffer.AttachData(indices);
        }

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
        _indexBuffer?.Bind();
        Texture?.Bind();

        GL.DrawElements(PrimitiveType.TriangleFan, (int)_indexBuffer?.Count!, DrawElementsType.UnsignedInt, 0);

        Texture?.UnBind();
        VertexBuffer.FinishDraw();
        _indexBuffer?.UnBind();
    }

    protected override void UpdateVertices()
    {
        Vertex[] vertices = new Vertex[PointCount + 1];

        for (int i = 0; i < PointCount; i++)
        {
            double angle = i * 2.0 / PointCount * Math.PI;
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);
            double x = c * _radius;
            double y = s * _radius;

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
                    (float)((c / 2) + 0.5),
                    (float)((-s / 2) + 0.5)
                );
        }

        vertices[^1] = Texture == null
            ? new Vertex(_x, _y, _color)
            : new Vertex(_x, _y, _color, 0.5f, 0.5f);


        VertexBuffer.AttachData(vertices);
    }
}