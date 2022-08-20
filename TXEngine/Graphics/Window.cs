using System.Drawing;
using TXEngine.Audio;
using TXEngine.OpenGL;
using TXEngine.Util;

namespace TXEngine.Graphics;

/// <summary>
///     窗口
/// </summary>
public class Window : IDisposable
{
    /// <summary>
    ///     窗口关闭事件的委托
    /// </summary>
    public delegate void WindowCloseHandler();

    public delegate void WindowResizeHandler(int width, int height);

    public delegate void KeyPressHandler(Keys key);

    public delegate void KeyReleaseHandler(Keys key);

    public delegate void MousePressHandler(MouseButton button);

    public delegate void MouseReleaseHandler(MouseButton button);

    public delegate void MouseMoveHandler(double x, double y);

    public delegate void TextEnterHandler(char unicode);

    /// <summary>
    ///     GLFW窗口的指针
    /// </summary>
    internal readonly unsafe GLFWWindowHandle* _handle;

    /// <summary>
    ///     着色器
    /// </summary>
    private readonly Shader _shader;

    /// <summary>
    ///     启用音频
    /// </summary>
    private bool _audioEnabled;

    /// <summary>
    ///     窗口标题
    /// </summary>
    private string _title;

    /// <summary>
    ///     启用垂直同步
    /// </summary>
    private bool _vSyncEnabled;

    /// <summary>
    ///     窗口大小
    /// </summary>
    private int _width, _height;

    private GLFWCallbacks.WindowCloseCallback _windowCloseCallback;
    private GLFWCallbacks.WindowSizeCallback _windowSizeCallback;
    private GLFWCallbacks.KeyCallback _keyCallback;
    private GLFWCallbacks.MouseButtonCallback _mouseButtonCallback;
    private GLFWCallbacks.CursorPosCallback _cursorPosCallback;
    private GLFWCallbacks.CharCallback _charCallback;

    /// <summary>
    ///     创建窗口
    /// </summary>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="title">标题</param>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public Window(int width, int height, string title)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    {
        OpenGLUtil.InitGLFW();

        _width = width;
        _height = height;
        _title = title;
        Open = true;
        _audioEnabled = false;
        _vSyncEnabled = true;

        unsafe
        {
            _handle = GLFW.CreateWindow(_width, _height, Title, null, null);
            Activate();
            OpenGLUtil.InitGL();
            InitCallbacks();
        }

        // 添加Alpha通道的支持
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _shader = Shader.GetDefaultShader();
        Viewport = new Vector4(0, 0, _width, _height);
    }

    /// <summary>
    ///     窗口大小
    ///     （请勿通过Vector2的X、Y属性修改数据）
    /// </summary>
    public Vector2i Size
    {
        get => new(_width, _height);
        set
        {
            _width = value.X;
            _height = value.Y;

            unsafe
            {
                GLFW.SetWindowSize(_handle, _width, _height);
            }
        }
    }

    /// <summary>
    ///     设置标题
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            unsafe
            {
                GLFW.SetWindowTitle(_handle, _title);
            }
        }
    }

    /// <summary>
    ///     启用音频
    /// </summary>
    public bool AudioEnabled
    {
        get => _audioEnabled;
        set
        {
            _audioEnabled = value;
            if (_audioEnabled)
            {
                ALUT.Init();
            }
            else
            {
                ALUT.Exit();
            }
        }
    }

    /// <summary>
    ///     窗口是否开启
    /// </summary>
    public bool Open { get; private set; }

    /// <summary>
    ///     启用垂直同步
    /// </summary>
    public bool VSyncEnabled
    {
        get => _vSyncEnabled;
        set
        {
            _vSyncEnabled = value;
            GLFW.SwapInterval(_vSyncEnabled ? 1 : 0);
        }
    }

    /// <summary>
    ///     视口（即窗口渲染的区域）
    /// </summary>
    public Vector4 Viewport
    {
        set
        {
            _shader.Bind();

            float x = value.X;
            float y = value.Y;
            float width = value.Z;
            float height = value.W;

            // 缩放
            Matrix3 mat1 = new(
                2.0f / width, 0, 0,
                0, -2.0f / height, 0,
                0, 0, 1
            );
            // 平移
            Matrix3 mat2 = new(
                1, 0, 0,
                0, 1, 0,
                -1, 1, 1
            );
            // 平移
            Vector3 pos = mat1 * new Vector3(x, y, 1);
            Matrix3 mat3 = new(
                1, 0, 0,
                0, 1, 0,
                -pos.X, -pos.Y, 1
            );

            Matrix3 mat = mat1 * mat3 * mat2;

            GL.UniformMatrix3(_shader.GetUniform("windowMatrix"), false, ref mat);

            _shader.UnBind();
        }
    }

    /// <summary>
    ///     销毁GLFW窗口
    /// </summary>
    public unsafe void Dispose()
    {
        GLFW.DestroyWindow(_handle);
    }

    /// <summary>
    ///     初始化回调事件
    /// </summary>
    private unsafe void InitCallbacks()
    {
        _windowCloseCallback = _ => OnWindowClosed?.Invoke();
        _windowSizeCallback = (_, _w, _h) =>
        {
            GL.Viewport(0, 0, _w, _h);
            OnWindowResized?.Invoke(_w, _h);
        };
        _keyCallback = (_, key, _, action, _) =>
        {
            switch (action)
            {
                case InputAction.Press:
                    OnKeyPressed?.Invoke(key);
                    break;
                case InputAction.Release:
                    OnKeyReleased?.Invoke(key);
                    break;
            }
        };
        _mouseButtonCallback = (_, button, action, _) =>
        {
            switch (action)
            {
                case InputAction.Press:
                    OnMousePressed?.Invoke(button);
                    break;
                case InputAction.Release:
                    OnMouseReleased?.Invoke(button);
                    break;
            }
        };
        _cursorPosCallback = (_, x, y) => { OnMouseMoved?.Invoke(x, y); };
        _charCallback = (_, codepoint) => { OnTextEntered?.Invoke(Convert.ToChar(codepoint)); };

        _ = GLFW.SetWindowCloseCallback(_handle, _windowCloseCallback);
        _ = GLFW.SetWindowSizeCallback(_handle, _windowSizeCallback);
        _ = GLFW.SetKeyCallback(_handle, _keyCallback);
        _ = GLFW.SetMouseButtonCallback(_handle, _mouseButtonCallback);
        _ = GLFW.SetCursorPosCallback(_handle, _cursorPosCallback);
        _ = GLFW.SetCharCallback(_handle, _charCallback);
    }


    /// <summary>
    ///     窗口关闭事件
    /// </summary>
    public event WindowCloseHandler? OnWindowClosed;

    public event WindowResizeHandler? OnWindowResized;
    public event KeyPressHandler? OnKeyPressed;
    public event KeyReleaseHandler? OnKeyReleased;
    public event MousePressHandler? OnMousePressed;
    public event MouseReleaseHandler? OnMouseReleased;
    public event MouseMoveHandler? OnMouseMoved;
    public event TextEnterHandler? OnTextEntered;

    /// <summary>
    ///     处理事件
    /// </summary>
    public void PollEvents()
    {
        if (!Open)
        {
            return;
        }

        GLFW.PollEvents();
    }

    /// <summary>
    ///     清空屏幕（使用黑色填充屏幕）
    /// </summary>
    public void Clear()
    {
        Clear(Color.Black);
    }

    /// <summary>
    ///     清空屏幕
    /// </summary>
    /// <param name="color">填充的颜色</param>
    public void Clear(Color color)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.ClearColor(color);
    }

    /// <summary>
    ///     将绘制的内容呈现到屏幕上
    /// </summary>
    public unsafe void Display()
    {
        GLFW.SwapBuffers(_handle);
    }

    /// <summary>
    ///     绘制对象
    /// </summary>
    /// <param name="drawable">可绘制的对象</param>
    public void Draw(IDrawable drawable)
    {
        lock (_shader)
        {
            lock (drawable)
            {
                _shader.Bind();

                drawable.Draw(_shader);

                _shader.UnBind();
            }
        }
    }

    /// <summary>
    ///     关闭窗口并清理资源
    /// </summary>
    public unsafe void Close()
    {
        Open = false;
        GLFW.HideWindow(_handle);
    }

    /// <summary>
    ///     激活窗口 以供OpenGL渲染
    ///     通常用于多窗口的情形
    /// </summary>
    public unsafe void Activate()
    {
        if (!Open)
        {
            return;
        }

        GLFW.MakeContextCurrent(_handle);
    }

    /// <summary>
    ///     设置窗口图标
    /// </summary>
    /// <param name="filename">图标文件名</param>
    public unsafe void SetIcon(string filename)
    {
        (byte[] pixels, int width, int height) = ImageUtil.LoadFromFile(filename);

        fixed (byte* ptr = pixels)
        {
            GLFW.SetWindowIcon(_handle, new ReadOnlySpan<Image>(new[] { new Image(width, height, ptr) }));
        }
    }
}