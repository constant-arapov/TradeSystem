using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Reflection;


using Common;
using Common.Utils;


using Terminal.Conf;

using Terminal.ViewModels;







namespace Terminal.DataBinding
{
    public static class CDataBinder
    {
        /// <summary>
        /// For each property in property list find property with such name in ViewModel. If value was set use it as initial
        /// values. Then in control class find  dependency property static field with name "PropertyName"Property  (also find in parent UserControl class).
        /// If all OK bind ViewModel property with UserControl dependency property
        /// </summary>
        /// <typeparam name="TypeOfViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="dependencyObject"></param>
        /// <param name="propertyList">List of strings. Name of fields are name of properties. Values (if it was set) are initial values of properties. </param>
        public static void BindFromList<TypeOfViewModel>(TypeOfViewModel viewModel, DependencyObject dependencyObject, object propertyList)                                   
        {
            propertyList.GetType().GetProperties().ToList().ForEach(
                propertyConfig =>
                {



                    try
                    {
                        string propertyName = propertyConfig.Name;

                        var p = viewModel.GetType().GetProperty(propertyConfig.Name);

                        PropertyInfo propertyViewModel = viewModel.GetType().GetProperty(propertyName);

                        if (propertyViewModel == null)
                        {
                            string msg = propertyName + " not found in ViewModel";
                            CKernelTerminal.ErrorStatic(msg);
                            throw new ApplicationException(msg);

                        }



                        //if value was set in config file
                        //set the value to ViewModel property
                        var value = propertyConfig.GetValue(propertyList, null);
                        if (value != null)
                        {
                                                      
                            //SetValue(marketViewModel, propertyViewModel, value);
                            CUtilReflex.SetPropertyValue(viewModel, propertyViewModel, value);

                        }
                       
                        string dpName = propertyConfig.Name + "Property";
                        Type type = dependencyObject.GetType();
                         

                        FieldInfo fieldDP = CUtilReflex.GetDependencyPropertyField(propertyConfig.Name,type);
                        //if not found do find in base FrameWorokElement class
                        if (fieldDP == null)
                        {
                            Type typeFrameWorkElement = type.BaseType.BaseType.BaseType;
                            fieldDP = CUtilReflex.GetField(dpName, typeFrameWorkElement);     

                        }
                       

                        
                        //dependency property field must be in Control
                        if (fieldDP != null) //found
                        {
                                                                

                            DependencyProperty dp = (DependencyProperty)fieldDP.GetValue(dependencyObject);                            

                            /* Bind ViewModel property and DependencyProperty of Control.
                             * If ViewModel property is writable, use two way binding 
                             * 
                             */                              
                            CUtil.SetBinding(viewModel,
                                            propertyName,
                                            dependencyObject,
                                            dp,
                                            twoWayBinding: propertyViewModel.CanWrite);

                        }
                        else //filed == null , field not found, error
                        {
                            string msg = "field not found: " + dpName;
                            CKernelTerminal.ErrorStatic(msg);
                            throw new ApplicationException(msg);                            
                        }

                    }
                    catch (Exception e)
                    {
                        CKernelTerminal.ErrorStatic("BindFromList error", e);

                    }




                });

        }





        public static void BindAllFromViewModel<TypeOfViewModel>(TypeOfViewModel viewModel, DependencyObject dependencyObject)
        {
            viewModel.GetType().GetProperties().ToList().ForEach(
                property =>
                {

                    try
                    {

                        string propertyName = property.Name;

                        string dpName = property.Name + "Property";
                        PropertyInfo propertyViewModel = viewModel.GetType().GetProperty(propertyName);


                        Type typeDependObj = dependencyObject.GetType();

                       

                        FieldInfo fieldDP = CUtilReflex.GetDependencyPropertyField(propertyName, typeDependObj);
                        if (fieldDP == null)
                            throw new ApplicationException("Field " + dpName + " not found in " + typeDependObj.Name);

                        DependencyProperty dp = (DependencyProperty)fieldDP.GetValue(dependencyObject);
     
                         /* Bind ViewModel property and DependencyProperty of Control.
                             * If ViewModel property is writable, use two way binding 
                             * 
                             */                              
                            CUtil.SetBinding(viewModel,
                                            propertyName,
                                            dependencyObject,
                                            dp,
                                            twoWayBinding: propertyViewModel.CanWrite);


                    }
                    catch (Exception e)
                    {
                        CKernelTerminal.ErrorStatic("BindAllFromViewModel", e);
                    }


                }
           );

        }


        private static void LoadOneClassProperties<TypeViewModel>(object classWithProps, TypeViewModel marketViewmodel)
        {

            classWithProps.GetType().GetProperties().ToList().ForEach(
               propertyConfig =>
               {


                   try
                   {
                       var valueConfig = propertyConfig.GetValue(classWithProps, null);
                       if (valueConfig != null)
                           marketViewmodel.GetType().GetProperties().ToList().ForEach(
                            propertyViewMdl =>
                            {



                                if (propertyViewMdl.Name == propertyConfig.Name)
                                {
                                   
                                    //string stVal = val.ToString();
                                    //propertyConfig.SetValue(classWithProps, stVal, null);
                                    //propertyViewMdl.SetValue(marketViewmodel, valueConfig, null);
                                    CUtilReflex.SetPropertyValue(marketViewmodel, propertyViewMdl, valueConfig);



                                }



                            }

                        );
                   }
                   catch (Exception e)
                   {
                       CKernelTerminal.ErrorStatic("LoadOneClassProperties", e);
                   }

               }
               );


        }

        /// <summary>
        /// Call from  MarketViewModel.SetEmptyVewModelInstrParams
        /// </summary>
        /// <param name="instrumentConfig"></param>
        /// <param name="marketViewModel"></param>
        public static void LoadMarketConfig(CInstrumentConfig instrumentConfig, MarketViewModel marketViewModel)
        {

            try
            {
                LoadOneClassProperties(instrumentConfig.MarketProperties.StockProperties, marketViewModel);
                LoadOneClassProperties(instrumentConfig.MarketProperties.DealsProperties, marketViewModel);
                LoadOneClassProperties(instrumentConfig.MarketProperties.ClusterProperties, marketViewModel);
            }
            catch (Exception e)
            {

                CKernelTerminal.ErrorStatic("LoadMarketConfig");
            }

        }


        public static void SaveMarketConfig(CInstrumentConfig instrumentConfig, MarketViewModel marketViewModel)
        {
            try
            {
                UpdateOneClassProperties(instrumentConfig.MarketProperties.StockProperties, marketViewModel);
                UpdateOneClassProperties(instrumentConfig.MarketProperties.DealsProperties, marketViewModel);
                //2018-08-21 protect against file corruption on file saving when cluster was not loaded yet
                if (marketViewModel.TimeFrame!=null)                                       
                    UpdateOneClassProperties(instrumentConfig.MarketProperties.ClusterProperties, marketViewModel);
            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("UpdateMarketConfig", e);
            }

        }

        public static void UpdateTerminalConfig(CTerminalProperties terminalProperties, TerminalViewModel termViewModel)
        {

            try
            {

                UpdateOneClassProperties(terminalProperties.TerminalCommonProperties, termViewModel);
                UpdateOneClassProperties(terminalProperties.TerminalStockProperties, termViewModel);
                UpdateOneClassProperties(terminalProperties.TerminalDealsProperties, termViewModel);
                UpdateOneClassProperties(terminalProperties.TerminalClustersProperties, termViewModel);
				UpdateOneClassProperties(terminalProperties.TerminalGlobalProperties, termViewModel);

            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("UpdateTerminalConfig",e);


            }
        }

        private static void UpdateOneClassProperties<TypeViewModel>(object classWithProps, TypeViewModel marketViewmodel)
        {


            classWithProps.GetType().GetProperties().ToList().ForEach(
                propertyConfig =>
                {


                    try
                    {
                        var value = propertyConfig.GetValue(classWithProps, null);
                        if (value != null)
                            marketViewmodel.GetType().GetProperties().ToList().ForEach(
                             propertyViewMdl =>
                             {



                                 if (propertyViewMdl.Name == propertyConfig.Name)
                                 {
                                     object val =  propertyViewMdl.GetValue(marketViewmodel, null);
                                     string stVal =val.ToString();
                                     propertyConfig.SetValue(classWithProps, stVal, null);


                                 }



                             }

                         );
                    }
                    catch (Exception e)
                    {
                        CKernelTerminal.ErrorStatic("UpdateOneClassProperties", e);
                    }

                }             


                

                );



        }





    }
}
