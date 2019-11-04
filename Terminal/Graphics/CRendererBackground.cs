using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace Terminal.Graphics
{
    public class CRendererBackground
    {

        FrameworkElement _frameWorkElement;
        RenderTargetBitmap _bitmap;
        DrawingVisual _drawingVisual;
        Image _defaultImg;
        System.Windows.Threading.Dispatcher _disp;

        public CRendererBackground(FrameworkElement frameWorkElement, DrawingVisual drawingViusal, 
                                        System.Windows.Threading.Dispatcher disp)
        {

            _frameWorkElement = frameWorkElement;
            _drawingVisual = drawingViusal;
            _disp = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            _disp = disp;

        }

        public CRendererBackground(FrameworkElement frameWorkElement, DrawingVisual drawingViusal, Image defaultImage,
                                     System.Windows.Threading.Dispatcher disp)
            : this(frameWorkElement, drawingViusal, disp)
        {

            _defaultImg = defaultImage;

        }




        public void Render(Image img)
        {

            if (_frameWorkElement.ActualWidth > 1.0 && _frameWorkElement.ActualHeight > 1.0)
            {
                //2017-03-26 add ceiling
                _bitmap = new RenderTargetBitmap((int)Math.Ceiling(_frameWorkElement.ActualWidth), (int)Math.Ceiling(_frameWorkElement.ActualHeight),
                                                    96.0, 96.0, PixelFormats.Pbgra32);


                

                _bitmap.Render(_drawingVisual);

                 _bitmap.Freeze();

                _disp.Invoke(new Action(() =>
                 {
                     if (img != null)
                         img.Source = _bitmap;
                     else
                         _defaultImg.Source = _bitmap;
                 }
                ));


            }
        }

        public void Render()
        {

            if (_defaultImg == null)
                throw (new ApplicationException("Default image was not set"));

            Render(_defaultImg);

        }





    }
}
