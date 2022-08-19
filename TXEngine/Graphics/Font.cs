using TXEngine.OpenGL;

namespace TXEngine.Graphics;

/// <summary>
///     字体
/// </summary>
public class Font : IDisposable
{
    /// <summary>
    ///     字符纹理缓存
    ///     Key: 大小、字符
    /// </summary>
    private readonly Dictionary<(int size, char c), CharacterTexture> _characterTextureMap;

    private string _filename;

    public Font(string filename)
    {
        _filename = filename;
        _characterTextureMap = new Dictionary<(int size, char c), CharacterTexture>();
    }

    /// 字体文件路径
    public string Filename
    {
        get => _filename;
        set
        {
            _filename = value;
            if (!File.Exists(Filename)) throw new IOException();
        }
    }

    /// 空格宽度
    internal int SpaceWidth { get; set; }

    public void Dispose()
    {
        foreach (var texture in _characterTextureMap.Values) texture.Dispose();
    }

    /// <summary>
    ///     获取字符纹理表
    /// </summary>
    /// <param name="source">文本</param>
    /// <param name="size">大小</param>
    /// <returns></returns>
    internal unsafe Dictionary<char, CharacterTexture> GetTextures(string source, int size)
    {
        // 初始化FontInfo
        stbtt_fontinfo info;
        fixed (byte* ptr = File.ReadAllBytes(Filename))
        {
            info = new stbtt_fontinfo();
            stbtt_InitFont(info, ptr, stbtt_GetFontOffsetForIndex(ptr, 0));
        }

        Dictionary<char, CharacterTexture> textures = new(); // 纹理表
        var scale = stbtt_ScaleForPixelHeight(info, size); // 比例

        // 统计需要加载的字符纹理
        HashSet<char> chars = new(source);
        // 若存在制表符/空格，则计算空格大小
        if (chars.Contains(' ') || chars.Contains('\t'))
        {
            int[] arg1 = new int[1], arg2 = new int[1];
            fixed (int* var1 = arg1, var2 = arg2)
            {
                stbtt_GetCodepointHMetrics(info, ' ', var1, var2);
                SpaceWidth = (int)(arg1[0] * scale);
            }
        }

        // 移除空白字符
        chars.Remove(' ');
        chars.Remove('\t');
        chars.Remove('\n');

        int[] w = new int[1], h = new int[1], xoff = new int[1], yoff = new int[1];
        foreach (var c in chars)
            if (_characterTextureMap.ContainsKey((size, c)))
            {
                textures[c] = _characterTextureMap[(size, c)];
            }
            else
            {
                // 纹理数据
                var pixels = new byte[size * size];

                // 加载数据
                fixed (int* wPtr = w, hPtr = h, xPtr = xoff, yPtr = yoff)
                {
                    var tmp = stbtt_GetCodepointBitmap(info, 0, scale, c,
                        wPtr, hPtr, xPtr, yPtr);

                    Marshal.Copy(new IntPtr(tmp), pixels, 0, pixels.Length);
                    stbtt_FreeBitmap(tmp, null);
                }

                // 创建纹理
                var texture = new CharacterTexture
                {
                    Offset = new Vector2(xoff[0], yoff[0]),
                    Advance = w[0] + xoff[0]
                };
                texture.LoadFromMemory(pixels, w[0], h[0]);
                _characterTextureMap[(size, c)] = textures[c] = texture;
            }

        GC.Collect();
        return textures;
    }
}