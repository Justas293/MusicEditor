using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;
using System.Windows.Forms;

namespace MusicEditor
{
    public class NotePainter
    {
        IncipitViewer incipitViewer1 { get; set; }
        IncipitViewer incipitViewer2 { get; set; }
        PianoForm form;

        public NotePainter(PianoForm form)
        {
            this.form = form;
            this.incipitViewer1 = form.incipitViewer1;
            this.incipitViewer2 = form.incipitViewer2;
        }
        
        public void DrawNote(MyNote note)
        {
            Note n = null;
            char[] symbols = note.name.ToCharArray();
            Key k = new Key(0);

            NoteStemDirection noteStem = NoteStemDirection.Up;
            //check note stem
            if (note.NoteToOctave() > 4) noteStem = NoteStemDirection.Down;

            form.spaceCounter += note.NoteToDuration().FloatToSpace();

            n = new Note(note.name.Substring(0, 1), note.NoteToSign(), note.NoteToOctave(), note.NoteToDuration().FloatToMusicalDuration(),
            noteStem, NoteTieType.None,
            new List<NoteBeamType>() { NoteBeamType.Single });
            n.NumberOfDots = note.NoteToDuration().FloatToDot();
            
            if (form.wasSign)
            {
                if (incipitViewer1.IsNaturalSignNeeded(n, k))
                {
                    n.HasNatural = true;
                    form.wasSign = false;
                }
                if (incipitViewer2.IsNaturalSignNeeded(n, k))
                {
                    n.HasNatural = true;
                    form.wasSign = false;
                }
            }

            if (form.spaceCounter > 45)
            {
                incipitViewer2.AddMusicalSymbol(n);
                incipitViewer2.Refresh();
                if (form.spaceCounter > 90)
                {
                    form.result1 = MessageBox.Show("If you continue to write notes, you won`t be able to see and edit last two rows of your song. Do you want to continue writing?",
                    "Important Question",
                    MessageBoxButtons.YesNo);
                    if (form.result1 == DialogResult.Yes)
                    {
                        form.staffeditor.ClearStaff();
                        form.staffeditor.RefreshStaff();
                        form.spaceCounter = 0;
                    }
                    if (form.result1 == DialogResult.No)
                    {
                        incipitViewer2.RemoveLastMusicalSymbol();
                        form.staffeditor.RefreshStaff();
                        form.daina.RemoveLastNote();
                    }
                }
            }
            else
            {
                incipitViewer1.AddMusicalSymbol(n);
                incipitViewer1.Refresh();
            }
        }

        public void DrawSong(Song song)
        {
            foreach(MyNote n in song.phrase)
            {
                DrawNote(n);
            }
        }
    }
}
