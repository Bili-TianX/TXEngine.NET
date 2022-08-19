using TXEngine.Core;
using static TXEngine.Audio.ALUT;

namespace TXEngine.Audio;

/// <summary>
///     声音
/// </summary>
public class Sound : IDisposable, IFileLoader
{
    /// <summary>
    ///     缓冲对象
    /// </summary>
    private readonly int _buffer;

    /// <summary>
    ///     声音源
    /// </summary>
    private readonly int _source;

    /// <summary>
    ///     创建一个声音对象
    /// </summary>
    /// <exception cref="Exception">如果ALUT未初始化</exception>
    public Sound()
    {
        if (!Initialized)
        {
            throw new Exception("Please Enable Audio first!");
        }

        _buffer = AL.GenBuffer();
        _source = AL.GenSource();
    }

    /// <summary>
    ///     清理资源
    /// </summary>
    /// <exception cref="Exception">如果ALUT未初始化</exception>
    public void Dispose()
    {
        if (!Initialized)
        {
            throw new Exception("Please Enable Audio first!");
        }

        AL.DeleteSource(_source);
        AL.DeleteBuffer(_buffer);
    }

    /// <summary>
    ///     从文件中加载声音
    /// </summary>
    /// <param name="filename">文件路径（wav格式）</param>
    /// <exception cref="Exception">如果ALUT未初始化</exception>
    /// <exception cref="IOException">文件不存在</exception>
    public unsafe void LoadFromFile(string filename)
    {
        if (!Initialized)
        {
            throw new Exception("Please Enable Audio first!");
        }

        if (!File.Exists(filename))
        {
            throw new IOException("Audio File does not exists");
        }

        void* data = alutLoadMemoryFromFile(filename, out int format, out int size, out float frequency);

        if (data == null)
        {
            throw new Exception("Unable to load Audio File");
        }

        AL.BufferData(_buffer, (ALFormat)format, data, size, (int)frequency);

        AL.Source(_source, ALSourcef.Gain, 1);
        AL.Source(_source, ALSourcef.Pitch, 1);
        AL.Source(_source, ALSource3f.Position, 0, 0, 0);
        AL.Source(_source, ALSourcei.Buffer, _buffer);
    }

    /// <summary>
    ///     播放声音
    /// </summary>
    /// <exception cref="Exception">如果ALUT未初始化</exception>
    public void Play()
    {
        if (!Initialized)
        {
            throw new Exception("Please Enable Audio first!");
        }

        AL.SourcePlay(_source);
    }
}