namespace TXEngine.Audio;

internal static class ALUT
{
    public static bool Initialized { get; private set; }

    public static unsafe void Init()
    {
        if (Initialized) return;

        if (alutInit(null, null) == 0) throw new Exception("Unable to Init GLUT");

        AL.Listener(ALListener3f.Position, 0, 0, 0);
        AL.Listener(ALListener3f.Velocity, 0, 0, 0);

        Initialized = true;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Exit();
    }

    public static void Exit()
    {
        if (!Initialized) return;

        if (alutExit() == 0) throw new Exception("Unable to Exit GLUT");

        Initialized = false;
    }

    [DllImport("alut.dll", CharSet = CharSet.Ansi)]
    public static extern unsafe int alutInit(int* argcp, char** argv);

    [DllImport("alut.dll", CharSet = CharSet.Ansi)]
    public static extern int alutExit();

    [DllImport("alut.dll", CharSet = CharSet.Ansi)]
    public static extern unsafe void* alutLoadMemoryFromFile(
        string filename,
        out int format,
        out int size,
        out float frequency
    );
}