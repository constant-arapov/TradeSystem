using Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Threading;
using TradeManager.Interfaces.Clients;
using TradeManager.Interfaces.Keys;


namespace TradeManager.DataSyncher
{
    public abstract class CBaseDataSyncher <TModelElement,TVMElement>
    {

     

        public ObservableCollection<TVMElement> CollVM
        {
            get
            {
                return _collectionVm;
            }

        }
        
       // List<ModelElement> _lstModelElements = new List<ModelElement>();
        protected ObservableCollection<TVMElement> _collectionVm = new ObservableCollection<TVMElement>();


        List<string> _lstKeys;

        IClientDataSyncher _client;


        Func<TModelElement, TVMElement> _createVMElement;

        KeysDependenciesTrdMgr _keysDep = new KeysDependenciesTrdMgr();


        protected CollectionViewSource _collViewSourceVM { get; set; }


        public ICollectionView CollViewVM
        {
            get
            {
                return _collViewSourceVM.View;
            }
        }

        private Dispatcher _guiDisp;




        public CBaseDataSyncher(IClientDataSyncher client, EnmCodeKeys code, Func<TModelElement, TVMElement> createVMElement,
                                 Dispatcher dispatcher)
        {
            _client = client;
            _lstKeys = _keysDep.GetKeys(code); //lstKeys;           
            _createVMElement = createVMElement;

            _collViewSourceVM = new CollectionViewSource();
            _collViewSourceVM.Source = CollVM;
            _guiDisp = dispatcher;
           
            AddSorting();
        }


        public void AddSorting()
        {
            _lstKeys.ForEach( el => 
                {

                    _collViewSourceVM.SortDescriptions.Add(new SortDescription(el, ListSortDirection.Ascending));

                });
        }


        public void UpdateFilterStockExchId()
        {
        
            _collViewSourceVM.Filter += new FilterEventHandler(FilterStockExchId);

        }



        private void FilterStockExchId(object sender, FilterEventArgs e)
        {
            IKey_StockExch keyStockExch = (IKey_StockExch)  e.Item;

            e.Accepted = _client.IsStockExchSelected(keyStockExch.StockExchId);
            
            /*if (keyStockExch.StockExchId == 4)
                e.Accepted = true;
            else 
                e.Accepted = false;*/
        }





        public void Update(int stockExchId,
							List<TModelElement> inpList)
        {

            try
            {
                _guiDisp.Invoke(new Action(() =>
                    {
                        CheckUpdateOrAdd(inpList);
                        CheckDelete(stockExchId, inpList);
                    }
                ));
               
            }
            catch (Exception e)
            {
                _client.Error("Error in CDataSyncher.Update",e);
            }
        }


        private void CheckUpdateOrAdd(List<TModelElement> inpList)
        {
         

            foreach (var modelElement in inpList)
            {
                bool bFoundEqKeys = false;
                for (int i = 0; i < _collectionVm.Count; i++)
                {

                    List<PropertyInfo> sourceProperties = modelElement.GetType().GetProperties().ToList();
                    List<PropertyInfo> destProperties = _collectionVm[i].GetType().GetProperties().ToList();

                    int cntEqKeysFound = 0;

                    //bool bNotEqKeyFnd = false;

                  
                    
                    //compare properties of two current elements
                    foreach (var srcPropInfo in sourceProperties)    
                    {
                        foreach (var dstPropInfo in destProperties)
                        {
                            if (_lstKeys.Contains(srcPropInfo.Name) &&
                                     srcPropInfo.Name == dstPropInfo.Name)
                            {
                                if (CUtilReflex.IsEqualValues(srcPropInfo.GetValue(modelElement,null), 
                                                              dstPropInfo.GetValue( _collectionVm[i],null)))
                                {
                                    cntEqKeysFound++;
                                    //all of  keys are matched
                                    if (cntEqKeysFound == _lstKeys.Count)
                                    {
                                        _collectionVm[i] = _createVMElement(modelElement); 

                                        //keys found
                                        //no need to iterate more properties
                                        bFoundEqKeys = true;
                                        break;
                                    }                                         
                                 }                                                                    
                              }                    
                        }
                        //keys found no need to iterate more 
                        if (bFoundEqKeys)
                            break;                                               
                     }

                  

                                                                                                                                         
                    }

                  //we iterated all _lstModelElements and not found element from source 
                  //so we need to add element to _lstModelElemets                    
                    if (!bFoundEqKeys)                    
                        _collectionVm.Add( _createVMElement.Invoke((modelElement)));

                    

                }

        }



        private void CheckDelete(int stockExchId, List<TModelElement> inpList)
        {
            if (stockExchId == 0)
                _client.Error("CBaseDataSyncher. CheckDelete. StockExchId == 0");

			//Enumerate all elements in collectionVM. If element is not
			// exist in inpList (for the same StockExchId) do delete it from collectionVm
            for (int i = 0; i < _collectionVm.Count; i++)
            {
				List<PropertyInfo> destProperties = _collectionVm[i].GetType().GetProperties().ToList();
				PropertyInfo dstPrpInf =  destProperties.Find(dstPrp => dstPrp.Name == KeysDependenciesTrdMgr.StockExchId);
				if ( (int)dstPrpInf.GetValue(_collectionVm[i], null) != stockExchId)
					continue;

                 bool bFoundEqKeys = false;
                foreach (var modelElement in inpList)
                {

                    List<PropertyInfo> sourceProperties = modelElement.GetType().GetProperties().ToList();
                   

                    int cntEqKeysFound = 0;

                    //bool bNotEqKeyFnd = false;



                    //compare properties of two current elements
                    foreach (var srcPropInfo in sourceProperties)
                    {
                        foreach (var dstPropInfo in destProperties)
                        {
                            if (_lstKeys.Contains(srcPropInfo.Name) &&
                                     srcPropInfo.Name == dstPropInfo.Name)
                            {
                                if (CUtilReflex.IsEqualValues(srcPropInfo.GetValue(modelElement, null),
                                                              dstPropInfo.GetValue(_collectionVm[i], null)))
                                {
                                    cntEqKeysFound++;
                                    //all of  keys are matched
                                    if (cntEqKeysFound == _lstKeys.Count)
                                    {
                                       
                                        //keys found
                                        //no need to iterate more properties
                                        bFoundEqKeys = true;
                                      
                                    }
                                }
                            }
                        }
                        //keys found no need to iterate more 
                        if (bFoundEqKeys)
                            break;
                    }

                }
                //we iterated all _lstModelElements and not found element from source 
                //so we need to add element to _lstModelElemets                    
                if (!bFoundEqKeys)
                    _collectionVm.RemoveAt(i);

             }

              

         

        }

    


       
    }

  

   
    public  class KeysDependenciesTrdMgr
    {
        public static string StockExchId  = "StockExchId";
        static string BotId = "BotId";
        static string Instrument = "Instrument";

    /*    public List<string> Keys_StockExchId = new List<string>() { StockExchId };
        public List<string> Keys_StockExchId_BotId = new List<string> { StockExchId, BotId };
        public List<string> Keys_StcokExchIid_Instrument = new List<string> { StockExchId, Instrument };
        public List<string> Keys_StockEchId_BotId_Instrument = new List<string> { StockExchId, BotId, Instrument };
        */
        Dictionary<EnmCodeKeys, List<string>> _dictCodeListKeys = new Dictionary<EnmCodeKeys, List<string>>
        {
            
            {EnmCodeKeys._01_StockExchId,                 new List<string>() { StockExchId } },
            {EnmCodeKeys._02_StockExchId_BotId,           new List<string> { StockExchId, BotId }},
            {EnmCodeKeys._03_StockExchId_Instrument,      new List<string> { StockExchId, Instrument }},
            {EnmCodeKeys._04_StockEchId_BotId_Instrument, new List<string>{ StockExchId, BotId, Instrument }}

            
             
        };

        
        public List<string> GetKeys(EnmCodeKeys code)
        {
            return _dictCodeListKeys[code];
        }
        



    }


    public enum EnmCodeKeys : sbyte
    {

        _01_StockExchId = 1,
        _02_StockExchId_BotId,
        _03_StockExchId_Instrument,
        _04_StockEchId_BotId_Instrument



    }



}
