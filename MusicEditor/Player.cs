using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace MusicEditor
{
    
    public class Player
    {
        IPlayerAction action = null;

        public Thread workingThread;

        public int volume { get; set; }
        public int tempo { get; set; }

        public Player()
        {
            this.volume = 50;
            this.tempo = 80;
        }

        public void Stop(IPlayerAction concreteObj)
        {
            this.action = concreteObj;
            action.Stop();
        }

        public void Play(IPlayerAction concreteAction) //Lambda israiska!  Threading!
        {
            workingThread = new Thread( ()=>
            {
                this.action = concreteAction;
                action.Play();
            });
            workingThread.Start();
        }
    }
}
