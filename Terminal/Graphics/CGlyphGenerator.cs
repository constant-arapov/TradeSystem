using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows;


namespace Terminal.Graphics
{
    public class CGlyphGenerator
    {

        private Dictionary<char, ushort> _mapCharGl = new Dictionary<char, ushort>();
        private GlyphTypeface _glyphTypeface;



        public CGlyphGenerator()
        {
           
           _glyphTypeface = new GlyphTypeface(new Uri(@"C:\Windows\Fonts\Verdana.TTF"));


        }

        

        private ushort[] GetArrayIndices(string word)
        {
            ushort[] usbuff = new ushort[word.Length];
            ushort glyphIndex;

            for (int i = 0; i < word.Length; i++)
            {
                _glyphTypeface.CharacterToGlyphMap.TryGetValue(word[i], out glyphIndex);
                usbuff[i] = glyphIndex;

            }

            return usbuff;
        }

        private double[] GetArrayAdvanceWidth(string word, int fontSize)
        {

            double[] arr = new double[word.Length];
            int i = 0;

            //TODO from DP/config
            double kdHsW = 0.6;

            foreach (var v in word)
            {
                arr[i++] = kdHsW * fontSize;    //6;
            }



            return arr;
        }




        public GlyphRun GetGlyph(double x, double y, int fontSize, string word)
        {



            ushort[] arrIndices = GetArrayIndices(word);                
            double[] arrAdvanceWidth = GetArrayAdvanceWidth(word, fontSize);



            GlyphRun gr = new GlyphRun(
               _glyphTypeface,
               0,       // Bi-directional nesting level
               false,   // isSideways
                fontSize,      // pt size
               arrIndices,//new ushort[] { 20, 19, 19 },   // glyphIndices
               new Point(x, y),           // baselineOrigin
               arrAdvanceWidth,//new double[] { 7.0, 7.0, 7.0 },  // advanceWidths
               null,    // glyphOffsets
               null,    // characters
               null,    // deviceFontName
               null,    // clusterMap
               null,    // caretStops
               null);   // xmlLanguage


           
            

            return gr;




        }





    }
}
