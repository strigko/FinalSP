namespace FinalSP.Underhood
{
    class Progress
    {
        public int totalFiles;
        public int processedFiles;

        public void Print()
        {
            int percent = totalFiles == 0 ? 0 : (processedFiles * 100 / totalFiles);

            int barSize = 20;
            int filled = percent * barSize / 100;

            string bar = new string('#', filled) + new string('-', barSize - filled);

            Console.Write($"\r[{bar}] {percent}% ({processedFiles}/{totalFiles})");
        }
    }
}