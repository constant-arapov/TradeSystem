using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Markup;

using Terminal.TradingStructs;
using TradingLib.ProtoTradingStructs;


namespace Terminal.Controls.Market.ChildElements
{
    /// <summary>
    /// Логика взаимодействия для ControlUserPos.xaml
    /// </summary>
    public partial class ControlUserPos : UserControl
    {
        public ControlUserPos()
        {
            InitializeComponent();
           
        }
        public void SetDataContext()
        {
           // DataContext = UserPos;    
        }

        

        public CUserPos  UserPos
        {
            get 
            {
                return (CUserPos)GetValue(UserPosProperty); 
            }
            set 
            {
                SetValue(UserPosProperty, value); 
            }
        }

        protected  override void    OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "UserPos")
                System.Threading.Thread.Sleep(0);
          

 	        base.OnPropertyChanged(e);
        }


        // Using a DependencyProperty as the backing store for UserPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserPosProperty =
            DependencyProperty.Register("UserPos", typeof(CUserPos), typeof(ControlUserPos) , new UIPropertyMetadata(new CUserPos()));

        
        
    }
  

  
}

/*
public class CConverterAmount : MarkupExtension, IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        int amount = (int)value;
        return amount > 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }


    public override object ProvideValue(IServiceProvider serviceProvider)
    {

        if (_converter == null)
            _converter = new CConverterAmount();
        return _converter;

    }

    private static CConverterAmount _converter = null;

}
*/


