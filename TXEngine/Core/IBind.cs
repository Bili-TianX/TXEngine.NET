namespace TXEngine.Core;

/// <summary>
///     声明一个类是否可被OpenGL状态机绑定
/// </summary>
public interface IBind
{
    /// <summary>
    ///     绑定至OpenGL状态机
    /// </summary>
    public void Bind();

    /// <summary>
    ///     解除绑定
    /// </summary>
    public void UnBind();
}