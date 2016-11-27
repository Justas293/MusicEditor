using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MusicEditor
{
    [Serializable]
    public class FileStreamer
    {
        public event EventHandler<StreamEventArgs> FileSaved;
        public event EventHandler<StreamEventArgs> FileLoaded;
        public enum ReplaceExistingFile {YES = 1, NO = 0 };

        Stream stream; //git test
        
        public FileStreamer(){ }

        public FileStreamer(EventHandler<StreamEventArgs> OnFileSaved, EventHandler<StreamEventArgs> OnFileLoaded)
        {
            this.FileSaved += OnFileSaved;
            this.FileLoaded += OnFileLoaded;
        }

        public void Save<T>(String filename, T obj, ReplaceExistingFile v)
        {
            String path = Properties.Settings.Default.SaveLoadDirectory + filename + ".sng";
            if (v == ReplaceExistingFile.NO)
            {
                if (File.Exists(@path)) throw new FileAlreadyExistsException("File " + path + " already exists!");
            }
            try
            {
                stream = File.Open(@path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, obj);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Directory was not found!");
                String s= @"E:\Songs\";
                Properties.Settings.Default.SaveLoadDirectory = s;
            }
            if (stream != null) stream.Close();
            if (FileSaved != null) FileSaved(this, new StreamEventArgs(path));
        }

        public T Load<T>(String filename)
        {
            T obj;                                    
            String path = Properties.Settings.Default.SaveLoadDirectory + filename + ".sng";
            try
            {
                stream = File.Open(@path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                obj = (T)bformatter.Deserialize(stream);
                if (FileLoaded != null) FileLoaded(this, new StreamEventArgs(path));
                return obj;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Directory was not found!");
                String s = @"E:\Songs\";
                Properties.Settings.Default.SaveLoadDirectory = s;
                return default(T);
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("File with that name was not found!");
                return default(T);
            }
            
            finally
            {
                if (stream != null) stream.Close();
            }
        }
    }
}
