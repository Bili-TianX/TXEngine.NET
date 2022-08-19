using TXEngine.Core;
using Boolean = OpenTK.Graphics.OpenGL4.Boolean;

namespace TXEngine.OpenGL;

public class Shader : IBind, IDisposable
{
    /// <summary>
    ///     着色器ID
    /// </summary>
    private readonly int _program;

    /// <summary>
    ///     Uniform ID的缓冲
    /// </summary>
    private readonly Dictionary<string, int> _uniformLocationCache;

    /// <summary>
    ///     创建着色器
    /// </summary>
    /// <param name="vertexShaderSource">顶点着色器的源代码</param>
    /// <param name="fragmentShaderSource">片段着色器的源代码</param>
    public Shader(string vertexShaderSource, string fragmentShaderSource)
    {
        _program = GL.CreateProgram();
        _uniformLocationCache = new Dictionary<string, int>();

        var vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
        var fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

        GL.AttachShader(_program, vertexShader);
        GL.AttachShader(_program, fragmentShader);

        GL.LinkProgram(_program);
        GL.ValidateProgram(_program);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Bind()
    {
        GL.UseProgram(_program);
    }

    public void UnBind()
    {
        GL.UseProgram(0);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_program);
    }

    /// <summary>
    ///     获取默认的着色器
    /// </summary>
    /// <returns>默认的着色器</returns>
    internal static Shader GetDefaultShader()
    {
        return new Shader(
            @"#version 330 core

layout(location = 0) in vec2 txPosition;
layout(location = 1) in vec4 txColor;
layout(location = 2) in vec2 txTexCoord;

out vec4 txVertexColor;
out vec2 txVertexTexCoord;

uniform mat3 windowMatrix;

void main() {
    gl_Position = vec4(windowMatrix * vec3(txPosition, 1), 1);
    txVertexTexCoord = txTexCoord;
    txVertexColor = txColor;
}",
            @"#version 330 core

out vec4 FragColor;

in vec4 txVertexColor;
in vec2 txVertexTexCoord;

uniform sampler2D txTexture;
uniform int isText;

void main() {
    if (isText == 1) {
        FragColor = txVertexColor * vec4(1, 1, 1, texture(txTexture, txVertexTexCoord).r);
    } else {
        if (txVertexTexCoord.x == -1 && txVertexTexCoord.y == -1) {
            FragColor = txVertexColor;
        } else {
            FragColor = txVertexColor * texture(txTexture, txVertexTexCoord);
        }
    }
}"
        );
    }

    /// <summary>
    ///     编译着色器
    /// </summary>
    /// <param name="type">着色器类型</param>
    /// <param name="source">着色器的源代码</param>
    /// <returns>着色器的ID</returns>
    /// <exception cref="Exception">无法编译着色器</exception>
    private static int CompileShader(ShaderType type, string source)
    {
        var shader = GL.CreateShader(type);

        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out var result);
        if (result != (int)Boolean.False) return shader;

        GL.GetShader(shader, ShaderParameter.InfoLogLength, out var length);
        GL.GetShaderInfoLog(shader, length, out length, out var message);
        throw new Exception(message);
    }

    /// <summary>
    ///     从着色器中获取Uniform
    /// </summary>
    /// <param name="name">Uniform名称</param>
    /// <returns>Uniform的ID</returns>
    public int GetUniform(string name)
    {
        if (_uniformLocationCache.ContainsKey(name)) return _uniformLocationCache[name];

        return _uniformLocationCache[name] = GL.GetUniformLocation(_program, name);
    }
}