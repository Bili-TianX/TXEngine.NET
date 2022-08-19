namespace TXEngine.Core;

/// <summary>
///     声明一个类可以从文件中加载数据
/// </summary>
public interface IFileLoader
{
    /// <summary>
    ///     从文件中加载
    /// </summary>
    /// <param name="filename">文件名</param>
    public void LoadFromFile(string filename);
}