using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using System.Windows.Forms;

using System.Xml;
using System.IO;


using Common;
using Common.Utils;
using Terminal;
using Terminal.Common;


namespace zTestTermDataConsist
{
    public class TestTermDataConsist
    {


        private string _stDataBindingPath;

        private string _contentViewModel;

        private XmlNode _xmlNodeRoot;

        private string _stMarketViewModelType;
        private bool _isTestPassed = true;

        /* private string _conentStockProperties;
         private string _contentDealsProperties;
         private string _contentMarketProperties;
         */
        /*  private XmlNode _xmlStockProperties;
          private XmlNode _xmlDealsProperies;
          private XmlNode _xmlClusterProperties;
          */

        private List<Pair> _lstProperties = new List<Pair>()
        {
            new Pair ( "StockProperties","ControlStock"),
            new Pair( "DealsProperties", "ControlDeals"),
            new Pair( "ClusterProperties", "ControlClusters"),
            

        };






        public TestTermDataConsist()
        {
            /*

            string stTerminalRootPath = @"..\..\..\..\Terminal\";


            string stViewModPath = String.Format(@"{0}ViewModels\MarketViewModelPCH.cs", stTerminalRootPath);

            _stMarketViewModelType = "Terminal.ViewModels.MarketViewModel, Terminal"; ;



            //FileStream fs = File.OpenRead(stViewModPath);
            //fs.

            _contentViewModel = File.ReadAllText(stViewModPath);

            _stDataBindingPath = String.Format(@"{0}\DataBinding", stTerminalRootPath);




            //string fileUndefPath = Environment.GetEnvironmentVariable("CONFIG_PATH") + @"\Terminal\undefined.xml";
            string fileUndefPath = Terminal.CKernelTerminal.GetInstruemntPath(Literals.Undefind);

            System.Xml.XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileUndefPath);
            }
            catch (Exception e)
            {
                Error(e.Message);


            }

            _xmlNodeRoot = document.SelectSingleNode("CInstrumentConfig").SelectSingleNode("MarketProperties");
            */

        }






        public void DoTest()
        {
            _lstProperties.ForEach
                (
                    element =>
                    {
                        TestOneElement(element);

                    }


                );
            if (_isTestPassed)
                PrintSuccessBanner();

            //TestConfigFileEqViewModel();

        }



        private void TestOneElement(Pair element)
        {
            XmlNode xmlNode = _xmlNodeRoot.SelectSingleNode(element.ConfigItem);

            TestConfigFileEqViewModel(xmlNode);
            TestConfigFileEqPropertyClass(xmlNode, element.ConfigItem);


            TestPropertyClassEqControl(element);

            TestPropertClassEqViewModel(element);
            /**
             * Note: if ((PopertyClass == Control) && (PropertyClass == ViewModel)) =>
             *          Control == ViewModel
             * 
             **/




            //TestViewModelEqControlStock();

         
        }

        private void TestPropertClassEqViewModel(Pair pair)
        {
            string partOfConfig = pair.ConfigItem;
            //string stType = "Terminal.Controls.Market.Contri";
            var propertiesViewModel = CUtilReflex.GetPropertiesList(_stMarketViewModelType);
            if (propertiesViewModel != null)
                Thread.Sleep(0);

            var propertiesConfig = CUtilReflex.GetPropertiesList(GetPropertyTypeSt(partOfConfig));

            propertiesConfig.ForEach(propertyConfig =>
            {
                if (propertiesViewModel.FirstOrDefault(a => a.Name == propertyConfig.Name) == null)
                    Error(String.Format("ViewModel  doesn't contain property {0}", propertyConfig.Name));


            }


                );




        }

        private void TestPropertyClassEqControl(Pair pair)
        {

            string partOfConfig = pair.ConfigItem;
            string control = pair.Control;


            var propertiesControl = CUtilReflex.GetPropertiesList(GetControlTypeSt(control));

            var fieldsControl = CUtilReflex.GetFiedsList(GetControlTypeSt(control));




            var propertiesConfig = CUtilReflex.GetPropertiesList(GetPropertyTypeSt(partOfConfig));

            propertiesConfig.ForEach(property =>
            {
                string propertyName = property.Name;
                string dependencyPropName = propertyName + "Property";

                if (propertiesControl.FirstOrDefault(a => a.Name == propertyName) == null)
                    Error(String.Format("Control {0} doesn't contain property {1}  ",
                                         control, propertyName));

                if (fieldsControl.FirstOrDefault(a => a.Name == dependencyPropName) == null)
                    Error(String.Format("Control {0} doesn't contain dependency property {1}  ",
                                         control, dependencyPropName));

            }
            );



        }



        private string GetControlTypeSt(string stControl)
        {
            return String.Format("Terminal.Controls.Market.{0}, Terminal", stControl);

        }


        private string GetPropertyTypeSt(string configItem)
        {
            return String.Format("Terminal.DataBinding.C{0}, Terminal", configItem);


        }



        private void TestConfigFileEqPropertyClass(XmlNode xmlNode, string partOfConfigFile)
        {


            var properties = CUtilReflex.GetPropertiesList(GetPropertyTypeSt(partOfConfigFile));



            //if element in config doesn't exists in property
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                var res = properties.FirstOrDefault(el => el.Name == node.Name);
                if (res == null)
                    Error(String.Format("Element {0} doesn't exist in property class {1}", node.Name, GetPropertyTypeSt(partOfConfigFile)));


            }


        }

        private void TestConfigFileEqViewModel(XmlNode xmlNode)
        {
            TestOneNode(xmlNode);

        }


        private void TestOneNode(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {

                string expr = String.Format(@"[\s]{0}[\s\n]", node.Name);

                if (!CUtilRegular.Contains(expr, _contentViewModel))
                    Error("View model not contains " + node.Name);
            }

        }


        private void PrintSuccessBanner()
        {

            string msg = "================================================================================\n";
            msg +=       "======================   BINDING TEST PASSED   =================================\n";
            msg +=       "================================================================================\n";
            Console.Write(msg);

        }


        private void Error(string msg)
        {
            _isTestPassed = false;
            MessageBox.Show(msg);
            Console.WriteLine(msg);           
           // throw new ApplicationException(msg);
            //Console.ReadKey();


        }



    }
}
