namespace FinalSP.Underhood
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mutex mutex = new Mutex(true, "FinalSPApp", out bool isNew);

            if (!isNew)
            {
                Console.WriteLine("App is already running");
                return;
            }

            App app = new App();
            App.Instance = app;

            app.ShowMenu();
        }
    }
}