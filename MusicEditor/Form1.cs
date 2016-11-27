using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using PSAMControlLibrary;
using Microsoft.VisualBasic;


namespace MusicEditor
{
    public partial class PianoForm : Form
    {
        long time;
        public bool wasSign;
        long duration;
        public int spaceCounter;
        public Song daina;
        public Song temp;
        Player grotuvas;
        public DialogResult result1;
        public Thread myThread;
        public StaffEditor staffeditor;
        public NotePainter notepainter;
        Lazy<FileStreamer> _filestreamer; //Lazy
        List<Button> KeyboardControls;

        #region Creating notes for keyboard
        MyNote F2note = new MyNote("F2");
        MyNote Gb2note = new MyNote("Gb2");
        MyNote G2note = new MyNote("G2");
        MyNote Ab2note = new MyNote("Ab2");
        MyNote A2note = new MyNote("A2");
        MyNote Bb2note = new MyNote("Bb2");
        MyNote B2note = new MyNote("B2");
        MyNote C3note = new MyNote("C3");
        MyNote Db3note = new MyNote("Db3");
        MyNote D3note = new MyNote("D3");
        MyNote Eb3note = new MyNote("Eb3");
        MyNote E3note = new MyNote("E3");
        MyNote F3note = new MyNote("F3");
        MyNote Gb3note = new MyNote("Gb3");
        MyNote G3note = new MyNote("G3");
        MyNote Ab3note = new MyNote("Ab3");
        MyNote A3note = new MyNote("A3");
        MyNote Bb3note = new MyNote("Bb3");
        MyNote B3note = new MyNote("B3");
        MyNote C4note = new MyNote("C4");
        MyNote Db4note = new MyNote("Db4");
        MyNote D4note = new MyNote("D4");
        MyNote Eb4note = new MyNote("Eb4");
        MyNote E4note = new MyNote("E4");
        MyNote F4note = new MyNote("F4");
        MyNote Gb4note = new MyNote("Gb4");
        MyNote G4note = new MyNote("G4");
        MyNote Ab4note = new MyNote("Ab4");
        MyNote A4note = new MyNote("A4");
        MyNote Bb4note = new MyNote("Bb4");
        MyNote B4note = new MyNote("B4");
        #endregion

        public PianoForm()
        {
            InitializeComponent();
            this.Size = new Size(Properties.Settings.Default.WindowWidth, Properties.Settings.Default.WindowHeight); 
        }

        private void PianoForm_Load(object sender, EventArgs e) //Subscribe to event // 2 Lambda //anonymous method
        {
            notepainter = new NotePainter(this);
            _filestreamer = new Lazy<FileStreamer>(() => new FileStreamer(
                OnFileSaved:
                delegate(object s, StreamEventArgs arr){ MessageBox.Show("File saved in:" + arr.filePath); }, //anonymous method
                OnFileLoaded:
                (s, arr) => MessageBox.Show("File successfully loaded!")
                ));

            staffeditor = new StaffEditor(incipitViewer1, incipitViewer2);
            grotuvas = new Player();
            daina = new Song();
            spaceCounter = 0;
            wasSign = true;
            staffeditor.AddClef();
            stopPlayingbutton.Visible = false;
            daina.SongPlayed += SongPlayedHandler;
            //List of keyboard controls
            KeyboardControls = this.Controls.OfType<Button>().ToList();
            KeyboardControls.Remove(Savebutton);
            KeyboardControls.Remove(Loadbutton);
            KeyboardControls.Remove(undoButton);
            KeyboardControls.Remove(stopPlayingbutton);
        }

        private void SongPlayedHandler(object sender, EventArgs e) //Thread safety  Ivykio apdorojimas //anonymous method
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { SongPlayedHandler(sender, e); });  //anonymous method
                return;
            }
            
            newSongbutton.Visible = true;
            Playbutton.Visible = true;
            stopPlayingbutton.Visible = false;
            KeyboardActions.Perform(delegate { foreach (var k in KeyboardControls) k.Enabled = true; });
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public void PerformMouseUpActions(Control sender)
        {
            String subStr;
            char[] arr = sender.Name.ToCharArray();
            duration = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - time;
            if (sender.Name.Length > 5)
            {
                subStr = sender.Name.Substring(0, 3);
            }
            else subStr = sender.Name.Substring(0, 2);
            if (arr[1] == 'b') wasSign = true;

            MyNote note = new MyNote(subStr, duration);
            daina.AddNote(note);
            notepainter.DrawNote(note);
        }
        #region Keyboard mouse events

        private void F2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(F2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void F2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(F2note);
            PerformMouseUpActions((Control)sender);
        }

        private void Gb2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Gb2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Gb2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Gb2note);
            PerformMouseUpActions((Control)sender);
        }

        private void G2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(G2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void G2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(G2note);
            PerformMouseUpActions((Control)sender);
        }

        private void Ab2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Ab2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Ab2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Ab2note);
            PerformMouseUpActions((Control)sender);
        }

        private void A2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(A2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void A2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(A2note);
            PerformMouseUpActions((Control)sender);
        }

        private void Bb2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Bb2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Bb2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Bb2note);
            PerformMouseUpActions((Control)sender);
        }

        private void B2key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(B2note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void B2key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(B2note);
            PerformMouseUpActions((Control)sender);
        }

        private void C3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(C3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void C3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(C3note);
            PerformMouseUpActions((Control)sender);
        }

        private void Db3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Db3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Db3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Db3note);
            PerformMouseUpActions((Control)sender);
        }

        private void D3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(D3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void D3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(D3note);
            PerformMouseUpActions((Control)sender);
        }

        private void Eb3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Eb3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Eb3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Eb3note);
            PerformMouseUpActions((Control)sender);
        }

        private void E3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(E3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void E3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(E3note);
            PerformMouseUpActions((Control)sender);
        }

        private void F3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(F3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void F3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(F3note);
            PerformMouseUpActions((Control)sender);
        }

        private void Gb3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Gb3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Gb3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Gb3note);
            PerformMouseUpActions((Control)sender);
        }

        private void G3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(G3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void G3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(G3note);
            PerformMouseUpActions((Control)sender);
        }

        private void Ab3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Ab3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Ab3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Ab3note);
            PerformMouseUpActions((Control)sender);
        }

        private void A3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(A3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void A3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(A3note);
            PerformMouseUpActions((Control)sender);
        }

        private void Bb3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Bb3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Bb3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Bb3note);
            PerformMouseUpActions((Control)sender);
        }

        private void B3key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(B3note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void B3key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(B3note);
            PerformMouseUpActions((Control)sender);
        }

        private void C4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(C4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void C4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(C4note);
            PerformMouseUpActions((Control)sender);
        }

        private void Db4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Db4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Db4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Db4note);
            PerformMouseUpActions((Control)sender);
        }

        private void D4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(D4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void D4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(D4note);
            PerformMouseUpActions((Control)sender);
        }

        private void Eb4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Eb4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Eb4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Eb4note);
            PerformMouseUpActions((Control)sender);
        }

        private void E4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(E4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void E4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(E4note);
            PerformMouseUpActions((Control)sender);
        }

        private void F4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(F4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void F4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(F4note);
            PerformMouseUpActions((Control)sender);
        }

        private void Gb4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Gb4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Gb4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Gb4note);
            PerformMouseUpActions((Control)sender);
        }

        private void G4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(G4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void G4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(G4note);
            PerformMouseUpActions((Control)sender);
        }

        private void Ab4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Ab4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Ab4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Ab4note);
            PerformMouseUpActions((Control)sender);
        }

        private void A4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(A4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void A4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(A4note);
            PerformMouseUpActions((Control)sender);
        }

        private void Bb4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(Bb4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Bb4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(Bb4note);
            PerformMouseUpActions((Control)sender);
        }

        private void B4key_MouseDown(object sender, MouseEventArgs e)
        {
            grotuvas.Play(B4note);
            time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void B4key_MouseUp(object sender, MouseEventArgs e)
        {
            //grotuvas.Stop(B4note);
            PerformMouseUpActions((Control)sender);
        }
        #endregion

        private void newSongbutton_MouseClick(object sender, MouseEventArgs e)
        {
            daina.phrase.Clear();
            stopPlayingbutton.Visible = false;
            staffeditor.ClearStaff();
            spaceCounter = 0;
        }

        private void stopPlayingbutton_MouseClick(object sender, MouseEventArgs e)
        {
            grotuvas.Stop(daina);
            newSongbutton.Visible = true;
            Playbutton.Visible = true;
            stopPlayingbutton.Visible = false;
            KeyboardActions.Perform(delegate { foreach (var k in KeyboardControls) k.Enabled = true; });
        }

        private void Playbutton_MouseClick(object sender, MouseEventArgs e) //Anonymous method
        {
            KeyboardActions.Perform(delegate { foreach (var k in KeyboardControls) k.Enabled = false; });
            newSongbutton.Visible = false;
            Playbutton.Visible = false;
            stopPlayingbutton.Visible = true;
            grotuvas.Play(daina);
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            grotuvas.Stop(daina);
            if (spaceCounter > 0)
            {
                if (spaceCounter > 45)
                {
                    incipitViewer2.RemoveLastMusicalSymbol();
                    incipitViewer2.Refresh();
                    spaceCounter -= daina.phrase.Last().NoteToDuration().FloatToSpace();
                    daina.RemoveLastNote();
                }
                else
                {
                    incipitViewer1.RemoveLastMusicalSymbol();
                    incipitViewer1.Refresh();
                    spaceCounter -= daina.phrase.Last().NoteToDuration().FloatToSpace();
                    daina.RemoveLastNote();
                }
            }
        }

        private void PianoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            grotuvas.Stop(daina);
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            String value = "Enter file name here...";
            String filename = null;
            if (InputBox("Save...", "File name:", ref value) == DialogResult.OK)
            {
                filename = value;
                try
                {
                    _filestreamer.Value.Save<Song>(filename, daina, FileStreamer.ReplaceExistingFile.NO);
                }
                catch(FileAlreadyExistsException ex)
                {
                    result1 = MessageBox.Show(ex.Message + " Do you want to replace it?",
                    "Important Question",
                    MessageBoxButtons.YesNo);
                }
                finally
                {
                    if (result1 == DialogResult.Yes)
                    {
                        _filestreamer.Value.Save<Song>(filename, daina, FileStreamer.ReplaceExistingFile.YES);
                    }
                }
            }            
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {      
            String value = "Enter file name here...";
            String filename = null;
            if (InputBox("Open...", "File name:", ref value) == DialogResult.OK)
            {
                filename = value;
                temp = _filestreamer.Value.Load<Song>(filename);
                
                if (temp != null)
                {
                    daina = temp;
                    daina.SongPlayed += SongPlayedHandler;
                    staffeditor.ClearStaff();
                    spaceCounter = 0;
                    notepainter.DrawSong(daina);
                } 
                daina.SongPlayed += SongPlayedHandler;
            }       
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            incipitViewer1.DrawViewer(g, true);
        }

        private void Printbutton_Click(object sender, EventArgs e)
        {
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDocument1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void setDirectoryButton_Click(object sender, EventArgs e)
        {
            String value = "Enter directory here...";
            if (InputBox("Set directory...", "Directory:", ref value) == DialogResult.OK)
            {
                Properties.Settings.Default.SaveLoadDirectory = value;
                Properties.Settings.Default.Save();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
