using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using PSAMControlLibrary;

namespace MusicEditor
{
    [Serializable]
    public class MyNote: IPlayerAction
    {
        public String name { get; set; }
        public long duration { get; set; }
        public SoundPlayer notePlayer;

        public MyNote(String name, long dur = 6500)
        {
            this.name = name;
            this.duration = dur;
            notePlayer = new SoundPlayer(@Properties.Settings.Default.PianoSoundsDirectory + name + ".wav");
        }

        public void Play()
        {
            try
            {
                notePlayer.Play();
            }
            catch (System.IO.FileNotFoundException) { }
            catch (System.InvalidOperationException) { }          
        }

        public void Stop()
        {
            System.Threading.Thread.Sleep(100);
            notePlayer.Stop();
        }
    }
}
