namespace Visualizer
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    //Usercontrol order rectangle with textblock
    public class OrderStatic : UserControl, IComponentConnector
    {
        private bool _contentLoaded;
        internal Border OrderStatic_Border;
        internal Grid OrderStatic_Grid;
        internal TextBlock OrderStatic_TxtBlck;
        public double Price;

        public event Action<OrderStatic> TryingTo_RemoveOrder;

        public OrderStatic(double _Height)
        {
            this.InitializeComponent();
            base.Height = _Height;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/orderstatic.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.OrderStatic_Grid = (Grid) target;
                    return;

                case 2:
                    this.OrderStatic_Border = (Border) target;
                    this.OrderStatic_Border.MouseDown += new MouseButtonEventHandler(this.TxtBlck_RemoveOrder_MouseDown);
                    return;

                case 3:
                    this.OrderStatic_TxtBlck = (TextBlock) target;
                    this.OrderStatic_TxtBlck.MouseDown += new MouseButtonEventHandler(this.TxtBlck_RemoveOrder_MouseDown);
                    return;

                case 4:
                    ((TextBlock) target).MouseDown += new MouseButtonEventHandler(this.TxtBlck_RemoveOrder_MouseDown);
                    return;
            }
            this._contentLoaded = true;
        }
        // event handler for special textblock 
        //for remove order
        private void TxtBlck_RemoveOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == MouseButton.Middle) || (sender is TextBlock))
            {
                this.TryingTo_RemoveOrder(this);
            }
        }
    }
}

