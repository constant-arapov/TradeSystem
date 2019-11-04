using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

//using Terminal.Common;


namespace InstallTerminal
{
    public class CConfigSynchro 
    {
        IClientConfigSynchro _client;

      
 

        
        private XmlDocument _xmlDefaultInstrumentStandard = new XmlDocument();
        private XmlDocument _xmlTerminalConfigStandard = new XmlDocument();

        public  CConfigSynchro(IClientConfigSynchro client)
                               
        {
            _client = client;
           
          
         

        }



        

       
        /// <summary>
        /// Entry point to class
        /// </summary>
        public void UpdateConfigs()
        {

            try
            {

                //TODO if not exist create it
                if (!Directory.Exists(_client.PathConfigDir))
                {
                    _client.CallbackErrorExit("Каталог с конфигурацией не найден. " + _client.PathConfigDir);
                }



                SynchTerminalConig();


                CheckDefaultInstrumentConfigIsCorrect();

                
                IEnumerable<string> filesConfig = Directory.EnumerateFiles(_client.PathInstrumentsConfDir);
                SynchAllInstrumentConfig(filesConfig);

                if (_client.IsOverwriteInstrumentsConfig)
                {
                    CopyTerminalConfig();

                }


            }
            catch (Exception e)
            {
                _client.CallbackErrorExit("Ошибка обновления конфигурации. "+ e.Message);
            }

        }


        private void CopyTerminalConfig()
        {
            //Directory.GetFiles()
            //PathTerminalConfigDir()
             foreach (var filePath in  Directory.GetFiles(_client.PathTerminalConfInstrumentsSource))
             {
                int ind = filePath.LastIndexOf('\\');
                string fileName = filePath.Substring(ind+1, filePath.Length - ind-1);
                string destPath = String.Format(@"{0}\{1}", _client.PathInstrumentsConfDir,fileName);
                File.Copy(filePath, destPath, overwrite:true);
             }
        }



        private void SynchTerminalConig()
        {

        

                OutMessage("Синхронизация конфига терминала");


                _xmlTerminalConfigStandard.Load(_client.PathTerminalConfFileSource);


                XmlNode terminalConfig = _xmlTerminalConfigStandard.SelectSingleNode("CTerminalConfig");
                if (terminalConfig == null)
                    _client.CallbackErrorExit("Эталонный конфигурационный файл терминала некорректен");


              


                XmlDocument xmlDocTerminalDestConf = new XmlDocument();
                xmlDocTerminalDestConf.Load(_client.PathTerminalConfFile);


                string nodeName = "CTerminalConfig";
                XmlNode terminalConf = xmlDocTerminalDestConf.SelectSingleNode(nodeName);
                XmlNode terminalConfStandard = _xmlTerminalConfigStandard.SelectSingleNode(nodeName);

                SynchNodeTree(xmlDocTerminalDestConf, terminalConfStandard, terminalConf);
                xmlDocTerminalDestConf.Save(_client.PathTerminalConfFile);

         
                       

        }

        
       
      


        private void CheckDefaultInstrumentConfigIsCorrect()
        {
            OutMessage("Проверка конфига инструмента по умолчанию");

            string path = _client.PathDefaultInstrumentConfFileSource;


            _xmlDefaultInstrumentStandard.Load(path);
            XmlNode xmlNode = _xmlDefaultInstrumentStandard.SelectSingleNode("CInstrumentConfig");
            if (xmlNode == null)
                _client.CallbackErrorExit("Эталонный конфигурационный файл инструмента  по-умолчанию некорректен");

        }

      




        private void SynchAllInstrumentConfig(IEnumerable<string> filesConfig)
        {
            OutMessage("Синхронизируем конфиги всех инструментов");
            foreach (var file in filesConfig)
            {
                // if (CUtilRegular.Contains(@"[\s][\s]", file))
                string content = File.ReadAllText(file);
                if (content.Contains("CInstrumentConfig"))
                    SynchOneInstrumentConfig(file);





            }


        }


        /// <summary>
        /// For each user's intrument files.
        ///  If node which is exists in etalon file, not exists in  current file, 
        /// do add this node to current file.
        /// </summary>
        /// <param name="file"></param>
        private void SynchOneInstrumentConfig(string file)
        {

            try
            {

                XmlDocument xmlDocInstr = new XmlDocument();
                xmlDocInstr.Load(file);


                string nodeName = "CInstrumentConfig";

                XmlNode instrumetConfigDefaultInstr = _xmlDefaultInstrumentStandard.SelectSingleNode(nodeName);
                XmlNode instrumetConfigCurrInstr = xmlDocInstr.SelectSingleNode(nodeName);

                SynchNodeTree(xmlDocInstr, instrumetConfigDefaultInstr, instrumetConfigCurrInstr);


                nodeName = "MarketProperties";
                XmlNode marketPropertiesDefaultInstr = instrumetConfigDefaultInstr.SelectSingleNode(nodeName);
                XmlNode marketPropertiesConfigCurrInstr = instrumetConfigCurrInstr.SelectSingleNode(nodeName);

                nodeName = "StockProperties";
                XmlNode stockPropertiesDefaultInstr = marketPropertiesDefaultInstr.SelectSingleNode(nodeName);
                XmlNode stockPropertiesConfigCurrInstr = marketPropertiesConfigCurrInstr.SelectSingleNode(nodeName);
                SynchNodeTree(xmlDocInstr, stockPropertiesDefaultInstr, stockPropertiesConfigCurrInstr);

                nodeName = "DealsProperties";
                XmlNode dealsPropertiesDefaultInstr = marketPropertiesDefaultInstr.SelectSingleNode(nodeName);
                XmlNode dealsPropertiesConfigCurrInstr = marketPropertiesConfigCurrInstr.SelectSingleNode(nodeName);
                SynchNodeTree(xmlDocInstr, dealsPropertiesDefaultInstr, dealsPropertiesConfigCurrInstr);

                nodeName = "ClusterProperties";
                XmlNode clusterPropertiesDefaultInstr = marketPropertiesDefaultInstr.SelectSingleNode(nodeName);
                XmlNode clusterPropertiesConfigCurrInstr = marketPropertiesConfigCurrInstr.SelectSingleNode(nodeName);
                SynchNodeTree(xmlDocInstr, dealsPropertiesDefaultInstr, dealsPropertiesConfigCurrInstr);

                //2018-03-13 to fix - make decimals WorkAmount
                nodeName = "WorkAmounts";
                XmlNode workAmounts = instrumetConfigCurrInstr.SelectSingleNode(nodeName);
              
                for (int i=0;i<5; i++)
                    RenameXMLNode(xmlDocInstr, workAmounts.ChildNodes[0], "decimal");
              
                   

                xmlDocInstr.Save(file);
            }
            catch (Exception e)
            {
                _client.CallbackErrorExit("Ошибка обновления конфигурационного файла " + file + " "+ e.Message);

            }


        }

        public static void RenameXMLNode(XmlDocument doc, XmlNode oldRoot, string newname)
        {
            XmlNode newRoot = doc.CreateElement(newname);

            foreach (XmlNode childNode in oldRoot.ChildNodes)
            {
                newRoot.AppendChild(childNode.CloneNode(true));
            }
            XmlNode parent = oldRoot.ParentNode;
          
            parent.RemoveChild(oldRoot);
            parent.AppendChild(newRoot);
        }



        /// <summary>
        /// If node which is exists in etalon file, not exists in  current file, 
        /// do add this node to current file.
        /// 
        /// Call from:
        /// 1) SynchOneInstrumentConfig
        /// 2) SynchTerminalConig
        /// 
        /// 
        /// </summary>
        /// <param name="xmlDocToSynch">XMLDocument of file we need sychronise to</param>
        /// <param name="nodeStandard">Node from "standard", "etalon file"</param>
        /// <param name="nodeToChange">Root node in which we append not existed node if need</param>
        private void SynchNodeTree(XmlDocument xmlDocToSynch, XmlNode nodeStandard, XmlNode nodeToChange)
        {
            foreach (XmlNode nodeDefaultConf in nodeStandard.ChildNodes)
            {

                bool bFound = false;
                foreach (XmlNode nodeCurrConf in nodeToChange.ChildNodes)
                    if (nodeCurrConf.Name == nodeDefaultConf.Name)
                    {
                        bFound = true;
                        break;
                    }


                if (!bFound)
                {
                    XmlNode temp = xmlDocToSynch.ImportNode(nodeDefaultConf, true);
                    nodeToChange.AppendChild(temp);

                }



            }

        }

        private void OutMessage(string msg)
        {
            _client.OutMessage("---" + msg);
        }



    }
}
