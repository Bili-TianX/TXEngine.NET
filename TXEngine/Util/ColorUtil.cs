using System.Drawing;

namespace TXEngine.Util;

internal static class ColorUtil
{
    /// <summary>
    ///     为OpenGL标准化颜色
    /// </summary>
    /// <param name="color">颜色</param>
    /// <returns>包含四个float（RGBA）的元组，且每个值介于[0, 1]之间</returns>
    public static (float r, float g, float b, float a) Normalize(this Color color)
    {
        return (
            color.R / 255.0f,
            color.G / 255.0f,
            color.B / 255.0f,
            color.A / 255.0f
        );
    }
}