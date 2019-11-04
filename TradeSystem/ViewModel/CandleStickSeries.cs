using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System;
using System.Globalization;

namespace TradeSystem.ViewModel
{
    class CandleStickSeries : BubbleSeries
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
            bubbleDataPoint.Width =  50;

            double left =
                (ActualIndependentAxis.GetPlotAreaCoordinate(bubbleDataPoint.ActualIndependentValue)).Value - bubbleDataPoint.Width /2;

            double PlotAreaHeight = ActualDependentRangeAxis.GetPlotAreaCoordinate(ActualDependentRangeAxis.Range.Maximum).Value;
            double highPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ToDouble(bubbleDataPoint.ActualDependentValue)).Value;
            double lowPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ToDouble(bubbleDataPoint.ActualDependentValue) - bubbleDataPoint.ActualSize ).Value;

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
}
