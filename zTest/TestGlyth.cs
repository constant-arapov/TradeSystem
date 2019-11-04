using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;


using Terminal.Graphics;

namespace zTest
{


    public class TestGlyth
    {

        public void Test()
        {



            CGlyphGenerator gg = new CGlyphGenerator();

            GlyphRun gr = gg.GetGlyph(0, 0, 10, "123.0");


        }



    }
}
