using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RiftAuthenticator.WP7
{
    public class ConfigMapFile : System.Collections.Generic.Dictionary<string, object>
    {
        public string FileName { get; private set; }

        private static System.IO.IsolatedStorage.IsolatedStorageFile CreateIsolatedStorageFile()
        {
            return System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
        }

        public ConfigMapFile(string fileName)
        {
            FileName = fileName;
        }

        internal static bool HasFile(System.IO.IsolatedStorage.IsolatedStorageFile storage, string fileName)
        {
            var fileNames = storage.GetFileNames();
            int foundFileNameIndex = -1;
            for (int i = 0; i != fileNames.Length; ++i)
            {
                if (fileNames[i] == fileName)
                {
                    foundFileNameIndex = i;
                    break;
                }
            }
            return foundFileNameIndex != -1;
        }

        public void Reset()
        {
            Clear();
            using (var storage = CreateIsolatedStorageFile())
            {
                if (HasFile(storage, FileName))
                {
                    storage.DeleteFile(FileName);
                }
            }
        }

        public void Load()
        {
            Clear();
            using (var storage = CreateIsolatedStorageFile())
            {
                if (HasFile(storage, FileName))
                {
                    using (var stream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, storage))
                    {
                        Library.PlatformUtils.Android.MapFile.ReadMap(stream, this);
                        stream.Close();
                    }
                }
            }
        }

        public void Save()
        {
            using (var storage = CreateIsolatedStorageFile())
            {
                using (var stream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, storage))
                {
                    Library.PlatformUtils.Android.MapFile.WriteMap(stream, this);
                    stream.Close();
                }
            }
        }
    }
}
