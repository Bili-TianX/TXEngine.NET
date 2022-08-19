namespace TXEngine.Util;

/// <summary>
///     提供对OpenGL的初始化功能
/// </summary>
internal static class OpenGLUtil
{
    private static bool _glfwInitialized, _glInitialized;

    /// <summary>
    ///     （全局）初始化GLFW，
    ///     并注册一个函数，在程序退出时调用<see cref="GLFW.Terminate()" />
    /// </summary>
    internal static void InitGLFW()
    {
        if (_glfwInitialized)
        {
            return;
        }

        _ = GLFW.Init();

        _glfwInitialized = true;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => GLFW.Terminate();
    }

    /// <summary>
    ///     （全局）初始化OpenGL
    /// </summary>
    /// <exception cref="Exception">如果GLFW尚未初始化</exception>
    internal static void InitGL()
    {
        if (_glInitialized)
        {
            return;
        }

        if (!_glfwInitialized)
        {
            throw new Exception("Please Init GLFW first!");
        }

        GL.LoadBindings(new GLFWBindingsContext());

        _glInitialized = true;
    }
}