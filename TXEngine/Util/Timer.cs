namespace TXEngine.Util;

/// <summary>
///     计时器 <br />
///     使用GLFW.GetTime()实现
/// </summary>
public class Timer
{
    /// <summary>
    ///     开始时间
    /// </summary>
    private double _start;

    /// <summary>
    ///     创建计时器并启动
    /// </summary>
    public Timer()
    {
        Start();
    }

    /// <summary>
    ///     返回经过的时间
    /// </summary>
    public double Elapsed => GLFW.GetTime() - _start;

    /// <summary>
    ///     启动计时器
    /// </summary>
    public void Start()
    {
        _start = GLFW.GetTime();
    }

    /// <summary>
    ///     重启计时器，并返回经过的时间
    /// </summary>
    /// <returns>经过的时间</returns>
    public double Restart()
    {
        var now = GLFW.GetTime();
        var result = now - _start;
        _start = now;
        return result;
    }
}