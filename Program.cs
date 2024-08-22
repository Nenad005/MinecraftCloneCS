using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using static System.Console;

namespace BasicOpenTK
{
    class Program
    {
        static void Main()
        {
            WriteLine("Pozdrav svete!!!");
            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = NativeWindowSettings.Default;

            // gws.IsMultiThreaded = true;
            gws.RenderFrequency = 60;
            gws.UpdateFrequency = 60;

            nws.API = ContextAPI.OpenGL;
            nws.APIVersion = Version.Parse("4.1.0");
            nws.Size = new Vector2i(1280, 720);
            nws.Title = "TROUGAO NA EKRANU VAUUUUUUUUUUUUUUUUU !!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            nws.StartVisible = false;
            nws.WindowBorder = WindowBorder.Resizable;
            nws.Profile = ContextProfile.Core;
            // nws.WindowBorder = WindowBorder.Fixed;

            Application WIN = new Application(gws, nws);
            WIN.Run();
            WIN.Dispose();
            // Pause();
        }

        static void Pause()
        {
            Write("Press any key to continue . . .");
            ReadKey(true);
        }
    }
}
