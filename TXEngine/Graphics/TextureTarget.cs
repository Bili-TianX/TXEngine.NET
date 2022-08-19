using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     声明一个类拥有纹理
/// </summary>
public abstract class TextureTarget : IDrawable
{
    protected BaseTexture? _texture;

    public BaseTexture? Texture
    {
        get => _texture;
        set
        {
            _texture = value;
            Update();
        }
    }

    public abstract void Draw(Shader shader);

    public virtual void Update()
    {
    }
}