using System.Drawing;
using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     文本
/// </summary>
public class Text : IDrawable, IDisposable
{
    /// <summary>
    ///     制表符所占空格数
    /// </summary>
    private const int TAB_WIDTH = 4;

    /// <summary>
    ///     字符图形
    /// </summary>
    private readonly List<RectangleShape> _shapes;

    private Color _color;
    private Font _font;
    private int _size;
    private string _source;
    private float _x, _y;

    public Text(string source, float x, float y, Font font)
        : this(source, x, y, 32, Color.White, font)
    {
    }

    public Text(string source, float x, float y, int size, Color color, Font font)
    {
        _source = source;
        _x = x;
        _y = y;
        _color = color;
        _font = font;
        _shapes = new List<RectangleShape>();
        _size = size;

        GenerateShape();
    }

    /// <summary>
    ///     文字
    /// </summary>
    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            GenerateShape();
        }
    }

    /// <summary>
    ///     大小
    /// </summary>
    public int Size
    {
        get => _size;
        set
        {
            _size = value;
            GenerateShape();
        }
    }

    /// <summary>
    ///     字体
    /// </summary>
    public Font Font
    {
        get => _font;
        set
        {
            _font = value;
            GenerateShape();
        }
    }

    /// <summary>
    ///     位置
    /// </summary>
    public Vector2 Position
    {
        get => new(_x, _y);
        set
        {
            _x = value.X;
            _y = value.Y;
            UpdatePosition();
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
            UpdateColor();
        }
    }

    public void Dispose()
    {
        _shapes.ForEach(shape => shape.Dispose());
    }

    public void Draw(Shader shader)
    {
        GL.Uniform1(shader.GetUniform("isText"), 1);
        _shapes.ForEach(shape => shape.Draw(shader));
        GL.Uniform1(shader.GetUniform("isText"), 0);
    }

    /// <summary>
    ///     更新所有字符坐标
    /// </summary>
    private void UpdatePosition()
    {
        var x = _x;
        var y = _y;

        for (int i = 0, index = 0; i < _source.Length; i++)
        {
            switch (_source[i])
            {
                case '\n':
                    x = _x;
                    y += _size;
                    continue;
                case '\t':
                    x += TAB_WIDTH * _font.SpaceWidth;
                    continue;
                case ' ':
                    x += _font.SpaceWidth;
                    continue;
            }

            var shape = _shapes[index++];
            var texture = (CharacterTexture)shape.Texture!;
            var offset = texture.Offset;

            shape.Position = new Vector2(x + offset.X, y + offset.Y);
            x += texture.Advance;
        }
    }

    private void UpdateColor()
    {
        _shapes.ForEach(shape => shape.Color = _color);
    }

    /// <summary>
    ///     为每个字符生成Shape
    /// </summary>
    private void GenerateShape()
    {
        _shapes.ForEach(shape => shape.Dispose());
        _shapes.Clear();

        var x = _x;
        var y = _y;
        var charMap = _font.GetTextures(_source, _size);

        foreach (var c in _source)
        {
            // 空白字符
            switch (c)
            {
                case '\n':
                    x = _x;
                    y += _size;
                    continue;
                case '\t':
                    x += TAB_WIDTH * _font.SpaceWidth;
                    continue;
                case ' ':
                    x += _font.SpaceWidth;
                    continue;
            }

            var texture = charMap[c];
            var offset = texture.Offset;

            _shapes.Add(new RectangleShape(x + offset.X, y + offset.Y, texture.Width, texture.Height, _color, texture));
            x += texture.Advance;
        }

        GC.Collect();
    }
}