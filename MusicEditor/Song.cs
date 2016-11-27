using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicEditor
{
    [Serializable]
    public class Song: IPlayerAction
    {
        [field: NonSerialized]
        public event EventHandler SongPlayed;
        public List<MyNote> phrase;
        public String phraseString;
        public bool stopFlag = false;

        public Song()
        {
            phrase = new List<MyNote>();
        }

        public void AddNote(MyNote note)
        {
            phrase.Add(note);
        }

        public void RemoveLastNote()
        {
            phrase.Remove(phrase.Last());
        }

        public String SongToString()
        {
            foreach(MyNote n in phrase)
            {
                phraseString += n.name + " ";
            }
            return phraseString;
        }
        public void Play()
        {
            System.Threading.Monitor.Enter(this.phrase);
            try
            {
                for (int i = 0; i < phrase.Count(); i++)
                {
                    if (stopFlag) break;
                    this.phrase.ElementAt(i).notePlayer.Play();
                    System.Threading.Thread.Sleep((int)(this.phrase.ElementAt(i).NoteToDuration() * 1000));
                    this.phrase.ElementAt(i).notePlayer.Stop();
                }
            }
            catch(System.ArgumentOutOfRangeException)
            { Stop(); }
            catch (IndexOutOfRangeException)
            { Stop(); }
            catch (ArgumentNullException)
            { Stop(); }
            finally
            {
                System.Threading.Monitor.Exit(this.phrase);
                stopFlag = false;
                if (SongPlayed != null) SongPlayed(this, new EventArgs());
            }            
        }

        public void Stop()
        {
            stopFlag = true;
        }
    }
}
