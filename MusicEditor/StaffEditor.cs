using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace MusicEditor
{
    public class StaffEditor
    {
        IncipitViewer incipitViewer1 { get; set; }
        IncipitViewer incipitViewer2 { get; set; }

        public StaffEditor(IncipitViewer v1, IncipitViewer v2)
        {
            incipitViewer1 = v1;
            incipitViewer2 = v2;
        }

        public void AddClef()
        {
            Clef clef = new Clef(ClefType.GClef, 2);
            incipitViewer1.AddMusicalSymbol(clef);
            incipitViewer2.AddMusicalSymbol(clef);
            RefreshStaff();
        }

        public void RefreshStaff()
        {
            incipitViewer1.Refresh();
            incipitViewer2.Refresh();
        }

        public void ClearStaff()
        {
            incipitViewer1.ClearMusicalIncipit();
            incipitViewer2.ClearMusicalIncipit();
            AddClef();
            RefreshStaff();
        }
    }
}
