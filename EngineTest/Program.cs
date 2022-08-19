using OpenTK.Mathematics;
using System.Drawing;
using TXEngine.Audio;
using TXEngine.Graphics;
using TXEngine.OpenGL;

namespace EngineTest;

internal static class Program
{
    private static void Test1()
    {
        Window window = new(1280, 720, "Hello");
        window.OnWindowClosed += () => window.Close();

        ImageTexture texture = new();
        texture.LoadFromFile("wall.png");
        var circle = new RectangleShape(100, 100, 50, 50, Color.Yellow, texture);
        circle.Size = new Vector2(500, 50);
        circle.Position = new Vector2(500, 500);
        circle.Color = Color.Red;
        circle.Texture = null;


        while (window.Open)
        {
            window.PollEvents();
            window.Clear(Color.Aqua);
            window.Draw(circle);
            window.Display();
        }

        circle.Dispose();
        window.Dispose();
    }

    public static void Test2()
    {
        Window window = new(1280, 720, "Hello")
        {
            AudioEnabled = true
        };
        window.OnWindowClosed += () => window.Close();
        var sound = new Sound();
        sound.LoadFromFile("button_click.wav");
        sound.Play();

        while (window.Open)
        {
            window.PollEvents();
            window.Clear(Color.Aqua);
            window.Display();
        }

        sound.Dispose();
        window.Dispose();
    }

    private static void Test3()
    {
        Window window = new(800, 500, "Hello")
        {
            AudioEnabled = true
        };

        window.OnWindowClosed += () => window.Close();
        window.OnWindowResized += (width, height) => Console.WriteLine($"{width}, {height}");

        Font font = new(
            "D:\\Users\\Bili_TianX\\Source\\Repos\\NET\\TXEngine\\TXEngine\\bin\\Debug\\net6.0\\simsun.ttc");
        Text text = new(@"abc", 0, 100, font);
        text.Source = "Who You ARe??????";

        while (window.Open)
        {
            window.PollEvents();

            window.Clear();
            window.Draw(text);
            window.Display();
        }

        text.Dispose();
        font.Dispose();
        window.Dispose();
    }

    public static void Main()
    {
        Test3();
    }
}