using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;


using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls;
using System.Windows;


using Common.Interfaces;
using System.Globalization;



namespace Common.Collections
{





    class CandleStickSeries2 : BubbleSeries
    {
        /// <summary>
        /// Updates the data point's visual representation.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// 

        public bool IsUserDeals { get; set; }
        public string Isin { get; set; }
        public long BotId { get; set; }


        protected override void UpdateDataPoint(DataPoint dataPoint)
        {
            BubbleDataPoint bubbleDataPoint = (BubbleDataPoint)dataPoint;
            bubbleDataPoint.Width = 50;

            double left =
                (ActualIndependentAxis.GetPlotAreaCoordinate(bubbleDataPoint.ActualIndependentValue)).Value - bubbleDataPoint.Width / 2;

            double PlotAreaHeight = ActualDependentRangeAxis.GetPlotAreaCoordinate(ActualDependentRangeAxis.Range.Maximum).Value;
            double highPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ToDouble(bubbleDataPoint.ActualDependentValue)).Value;
            double lowPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ToDouble(bubbleDataPoint.ActualDependentValue) - bubbleDataPoint.ActualSize).Value;

            if (CanGraph(left) && CanGraph(PlotAreaHeight - highPointY))
            {
                dataPoint.Visibility = Visibility.Visible;
                dataPoint.Height = Math.Abs(highPointY - lowPointY);

                Canvas.SetLeft(bubbleDataPoint, left);
                Canvas.SetTop(bubbleDataPoint, PlotAreaHeight - highPointY);
            }
            else
            {
                dataPoint.Visibility = Visibility.Collapsed;
            }
        }

        public static bool CanGraph(double value)
        {
            return !double.IsNaN(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value) && !double.IsInfinity(value);
        }
        public static double ToDouble(object value)
        {
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

    }







    public class CObservableIdThradeSafeCollection<T, P> : CObservableIdCollection<T, P>, INotifyCollectionChanged, INotifyPropertyChanged where T : IIDable<P>
    {




          public  override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            try
            {

                var notifyCollectionChangedEventHandler = CollectionChanged;

                if (notifyCollectionChangedEventHandler == null)
                    return;

               // System.Delegate[] t = notifyCollectionChangedEventHandler.GetInvocationList();

               // System.Windows.Controls.WeakEventListener <int, int>f;
              //  CandleStickSeries2 target = t[0].Target as CandleStickSeries2;                



                foreach (NotifyCollectionChangedEventHandler handler in notifyCollectionChangedEventHandler.GetInvocationList())
                {
                    var dispatcherObject = handler.Target as DispatcherObject;

                    if (dispatcherObject != null && !dispatcherObject.CheckAccess())
                    {
                        dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, args);
                    }
               //     else
                 //       handler(this, args); // note : this does not execute handler in target thread's context
                }
            }
            catch (Exception e)
                {

                    throw;
                }


        }

    }




}
