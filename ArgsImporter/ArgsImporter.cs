using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArgsImport
{
    public static class ArgsImporter
    {
        public static event Action onArgsChanged;

        private static List<string> args = new List<string>();
        private static long argsSize;
        private static string mutexName;
        private static string memoryFileName;
        private static Mutex mutex;
        private static Thread catchArgsThread;
        private static MemoryMappedFile memoryFile;

        public static bool? IsMainApp
        {
            get;
            private set;
        } = null;

        public static void Initialize(string memoryName, long argsSize)
        {
            ArgsImporter.argsSize = argsSize;
            mutexName = memoryName + "_argsMutex";
            memoryFileName = memoryFileName + "_argsFile";

     

            mutex = new Mutex(true, mutexName, out bool t);
            IsMainApp = t;

            if (IsMainApp == true)
            {
                memoryFile = MemoryMappedFile.CreateNew(memoryFileName, argsSize);
                mutex.ReleaseMutex();
                

                catchArgsThread = new Thread(() => CatchArgsLoop(memoryFile));
                catchArgsThread.Start();
            }
        }
        public static void AddArg(string arg)
        {
            mutex.WaitOne();

            using (MemoryMappedFile memoryFile = MemoryMappedFile.OpenExisting(memoryFileName))
            using (MemoryMappedViewStream stream = memoryFile.CreateViewStream(0, argsSize))
            {
                while (stream.ReadByte() > 0) ;
                stream.Position--;

                byte[] data = Encoding.UTF8.GetBytes($"{arg}|");
                stream.Write(data, 0, data.Length);
            }

            mutex.ReleaseMutex();
        }
        public static string[] GetArgs()
        {
            return args.ToArray();
        }
        public static void Dispose()
        {
            if (IsMainApp == true)
            {
                catchArgsThread.Abort();
                mutex.Close();
                memoryFile.Dispose();
            }
        }

        private static void CatchArgsLoop(MemoryMappedFile memoryFile)
        {
            List<string> addArgs = new List<string>();

            using (MemoryMappedViewStream stream = memoryFile.CreateViewStream())
                while (true)
                {
                    stream.Position = 0;

                    Thread.Sleep(1000);
                    mutex.WaitOne();

                    int bt = stream.ReadByte();

                    while (bt > 0)
                    {
                        List<byte> data = new List<byte>();

                        while (bt > 0 && bt != '|')
                        {
                            data.Add((byte)bt);
                            bt = stream.ReadByte();
                        }

                        addArgs.Add(Encoding.UTF8.GetString(data.ToArray()));

                        bt = stream.ReadByte();
                    }

                    if (addArgs.Count > 0)
                    {

                        lock (args)
                            args.AddRange(addArgs);

                        lock (onArgsChanged)
                            onArgsChanged?.Invoke();

                        ClearMemoryStream(stream);
                        addArgs.Clear();
                    }

                    mutex.ReleaseMutex();
                }
        }
        private static void ClearMemoryStream(MemoryMappedViewStream stream)
        {
            stream.Position = 0;
            while (stream.ReadByte() > 0)
            {
                stream.Position--;
                stream.WriteByte(0);
            }
        }
    }
}

