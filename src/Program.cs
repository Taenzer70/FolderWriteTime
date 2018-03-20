using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace FolderWriteTime
{
    class Program
    {
        static string fileName;
        static int fileSize;
        static Timer timer;

        static void Main(string[] args)
        {
            fileSize = 1024;

            if (args.Length == 0)
            {
                fileName = Path.GetTempFileName();
            }
            else
            {
                fileName = args[0];
            }

            Console.WriteLine("TimeStamp;Elapsed");
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(CheckWriteTime);
            timer.Interval = 5000;
            timer.Enabled = true;
            Console.ReadLine();
            timer.Enabled = false;
        }

        private static void CheckWriteTime(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            FileStream stream = GetFileStream();

            for (int i = 0; i < fileSize; i++)
            {
                stream.WriteByte((byte)(i % 16));
            }
            stream.Close();

            stopwatch.Stop();

            File.Delete(fileName);
            Console.WriteLine(DateTime.Now.ToString() + ';' + stopwatch.Elapsed.ToString());
            timer.Start();
        }

        static FileStream GetFileStream(bool notBuffered = true) => notBuffered ? new FileStream(fileName, FileMode.Create) : new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, fileSize);
    }
}
