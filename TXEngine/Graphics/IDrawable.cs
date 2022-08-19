using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     声明一个类可绘制
/// </summary>
public interface IDrawable
{
    /// <summary>
    ///     绘制
    /// </summary>
    /// <param name="shader">着色器</param>
    public void Draw(Shader shader);
}