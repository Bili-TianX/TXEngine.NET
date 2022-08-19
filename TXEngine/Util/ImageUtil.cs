using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace TXEngine.Util;

internal static class ImageUtil
{
    public static (byte[] pixels, int width, int height) LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new IOException();
        }

        Image image = Image.Load(File.Open(filename, FileMode.Open, FileAccess.Read));

        if (image is not Image<Rgba32> img)
        {
            throw new NotSupportedException();
        }

        byte[] pixels = new byte[image.PixelType.BitsPerPixel * image.Width * image.Height];
        img.CopyPixelDataTo(pixels);

        return (pixels, image.Width, image.Height);
    }
}