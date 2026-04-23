namespace FinalSP.Underhood
{
    class App
    {
        public static App Instance;

        private bool isPaused = false;
        private object locker = new object();
        private CancellationTokenSource cts = new CancellationTokenSource();

        private Progress progress = new Progress();

        public void ShowMenu()
        {
            string folderPath = "";
            List<string> words = new List<string>();

            while (true)
            {
                Console.WriteLine("||| Prohibited Words Scanner |||");
                Console.WriteLine("1. Start scan");
                Console.WriteLine("2. Pause");
                Console.WriteLine("3. Resume");
                Console.WriteLine("4. Stop");
                Console.WriteLine("5. Exit");
                Console.Write("Choose option: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("Enter folder path: ");
                    folderPath = Console.ReadLine();

                    words = ReadForbiddenWords();

                    var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                        .Where(f => !f.Contains("Output"))
                        .ToList();

                    App.Instance.SetTotal(files.Count);

                    cts = new CancellationTokenSource();

                    Task.Run(() =>
                    {
                        Scanner.ScanFolder(folderPath, words, cts.Token);
                    });
                }
                else if (input == "2")
                {
                    isPaused = true;
                    Console.WriteLine("Paused");
                }
                else if (input == "3")
                {
                    lock (locker)
                    {
                        isPaused = false;
                        Monitor.PulseAll(locker);
                    }
                    Console.WriteLine("Resumed");
                }
                else if (input == "4")
                {
                    cts.Cancel();
                    Console.WriteLine("Stopped");
                }
                else if (input == "5")
                {
                    Console.WriteLine("Ty for using!");
                    break;
                }
            }
        }

        private List<string> ReadForbiddenWords()
        {
            Console.WriteLine("Enter forbidden words (separate them with commas):");
            string input = Console.ReadLine();

            List<string> words = new List<string>();

            if (!string.IsNullOrWhiteSpace(input))
            {
                string[] parts = input.Split(',');

                foreach (var part in parts)
                {
                    string word = part.Trim();

                    if (!string.IsNullOrEmpty(word))
                        words.Add(word);
                }
            }

            return words;
        }

        public void WaitIfPaused()
        {
            lock (locker)
            {
                while (isPaused)
                    Monitor.Wait(locker);
            }
        }

        public void IncreaseProgress()
        {
            progress.processedFiles++;
            progress.Print();
        }

        public bool IsStopped()
        {
            return cts.IsCancellationRequested;
        }

        public void SetTotal(int total)
        {
            progress.totalFiles = total;
        }
    }
}