using System.Drawing;
using TXEngine.Graphics;

namespace EngineTest;

internal static class Program
{
    public static void Main()
    {
        Window window = new(1280, 720, "Hello");
        window.OnWindowClosed += () => window.Close();

        int w = window.Size.X;
        int h = window.Size.Y;
        CircleShape[] shapes = new CircleShape[32000];
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i] = new CircleShape((float)Random.Shared.NextDouble() * w, (float)Random.Shared.NextDouble() * h,
                ((float)Random.Shared.NextDouble() * 100) + 1,
                Color.FromArgb((int)(Random.Shared.NextDouble() * 255),
                (int)(Random.Shared.NextDouble() * 255),
                    (int)(Random.Shared.NextDouble() * 255),
                    (int)(Random.Shared.NextDouble() * 255)));
        }

        GC.Collect(int.MaxValue, GCCollectionMode.Forced, true, true);
        while (window.Open)
        {
            window.PollEvents();

            window.Clear();

            foreach (CircleShape shape in shapes)
            {
                window.Draw(shape);
            }

            window.Display();
        }

        foreach (CircleShape shape in shapes)
        {
            shape.Dispose();
        }

        window.Dispose();
    }
}