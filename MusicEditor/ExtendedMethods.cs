using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace MusicEditor
{
    public static class ExtendedMethods
    {
        public static float NoteToDuration(this MyNote note)
        {
            float len = 0;
            long dur = note.duration;
            if (dur > 5500) len = 6.0F;
            if (dur > 3500 && dur < 5499) len = 4.0F;
            if (dur > 2250 && dur < 3499) len = 3.0F;
            if (dur > 1750 && dur < 2249) len = 2.0F;
            if (dur > 1250 && dur < 1749) len = 1.5F;
            if (dur > 875 && dur < 1249) len = 1.0F;
            if (dur > 625 && dur < 874) len = 0.750F;
            if (dur > 437 && dur < 624) len = 0.500F;
            if (dur > 300 && dur < 436) len = 0.375F;
            if (dur > 1 && dur < 299) len = 0.250F;

            return len;
        }

        public static int NoteToOctave(this MyNote note)
        {
            int octave;

            char[] symbols = note.name.ToCharArray();

            if (symbols[1] == 'b')
            {
                octave =3 + (int)Char.GetNumericValue(symbols[2]) -2;
            }
            else
            {
                octave =3 + (int)Char.GetNumericValue(symbols[1]) -2;
            }

            return octave;
        }

        public static int NoteToSign(this MyNote note)
        {
            int sign;
            char[] symbols = note.name.ToCharArray();

            if (symbols[1] == 'b')
            {
                sign = -1;
            }
            else
            {
                sign = 0;
            }

            return sign;

        }
        
        public static MusicalSymbolDuration FloatToMusicalDuration(this float dur)
        {
            MusicalSymbolDuration musicalDur = MusicalSymbolDuration.Quarter;
            if (dur == 6.0F || dur == 4.0F) musicalDur = MusicalSymbolDuration.Whole;

            if (dur == 3.0F || dur == 2.0F) musicalDur = MusicalSymbolDuration.Half;

            if (dur == 1.5F || dur == 1.0F) musicalDur = MusicalSymbolDuration.Quarter;
            
            if (dur == 0.750F || dur == 0.500F) musicalDur = MusicalSymbolDuration.Eighth;

            if (dur == 0.375F || dur == 0.250F) musicalDur = MusicalSymbolDuration.Sixteenth;
            
            return musicalDur;
        }

        public static int FloatToSpace(this float dur)
        {
            int space = 0;
            if (dur == 6.0F || dur == 4.0F) space = 5;

            if (dur == 3.0F) space = 4;

            if (dur == 2.0F || dur == 0.375F || dur == 0.750F) space = 3;

            if (dur == 1.5F) space = 2;

            if (dur == 1.0F || dur == 0.500F || dur == 0.250F) space = 1;

            return space;
        }

        public static int FloatToDot(this float dur)
        {
            if (dur == 6.0F || dur == 3.0F || dur == 1.5F || dur == 0.750F || dur == 0.375F)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
