using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media;



namespace Terminal.Graphics
{    
    public class CLevelDrawer   
    {
       
        public CLevelDrawer()
        {
          

        }
        public void DrawLevelRectangle(DrawingContext drwCntxt, Brush brush, Pen pen, double x, double y, double width, double height)
        {
            if (width <= 0 || height <=0)
                return;

            drwCntxt.DrawRectangle(brush, pen, new Rect(x, y, width, height));


        }
        public void DrawLevelLine(DrawingContext drwCntxt, Pen pen, double x1, double y1, double x2, double y2)
        {

            drwCntxt.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

        }

    }
}
