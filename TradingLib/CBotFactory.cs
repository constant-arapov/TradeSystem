using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Reflection;

using Common;
using Common.Interfaces;
using Common.Utils;

using TradingLib.Interfaces.Components;
using TradingLib.Bots;





namespace TradingLib
{
    public class CBotFactory : CBaseFunctional
    {


		private IDealingServer _dealingServer;

        Dictionary<string, enmStrategysCode> m_dictBotNameCode = new Dictionary<string, enmStrategysCode>()
        {
            { "Tester", enmStrategysCode.StrategyTest  },
            { "Supervisor", enmStrategysCode.StrategySupervisor  },
            { "HighLowContra", enmStrategysCode.StrategyHighLowContra  },
            { "TesterPos", enmStrategysCode.StrategyTesterPos},
            { "TesterExternal", enmStrategysCode.StrategyTesterExternal},
            { "TesterCrossFirst", enmStrategysCode.StrategyTesterCrossFirst},
            { "TesterCrossSecond", enmStrategysCode.StrategyTesterCrossSecond},
            { "TesterLimits", enmStrategysCode.StrategyTesterLimits},
            { "Trader", enmStrategysCode.StrategyTrader}

        };

       
        
     //   delegate void DelegCreateBotFactory (CBaseBot bot) ;


		
        

        public CBotFactory(/*CPlaza2Connector*/IDealingServer dealingServer) : base( (IAlarmable) dealingServer)
        {
			
            _dealingServer = dealingServer;
                       
        }

        public void Error(string message, Exception e)
        {

           ((IAlarmable) _dealingServer).Error(message,e);
        }


       /* private object GeSettingsSupervisor(XmlNode node)
        {

            XmlNodeList xnList = node.SelectNodes("StrategySettings/limits/limit");
            CBotSupervisorSettings bss = new CBotSupervisorSettings();
            foreach (XmlNode lim in xnList)
            {
                string isin = lim.Attributes["isin"].Value.ToString();

                bss.DictInstrumentLimits[isin] = new CBotSupervisorInstrumentLimits();
                bss.DictInstrumentLimits[isin].MaxOpenedPos = Convert.ToInt32(lim.Attributes["MaxOpenedPos"].Value.ToString());
                bss.DictInstrumentLimits[isin].MaxLossVM = Convert.ToDecimal(lim.Attributes["MaxLossVM"].Value.ToString());
            }

            return (object)bss;

        }
        */


        public void CreateOneBotFromConfig(Dictionary<string, object> botConfig, List<Dictionary<string, object>> lstBotsInstrumentsConfig, int num, ref  List<CBotBase> listBots,
                                                                      bool needSynchroOnLoad )
        {

            try
            {
				Log("Start CreateOneBotFromConfig");
                string type = (string) botConfig["type"];

                bool bEnabled = Convert.ToBoolean(botConfig["Enabled"].ToString());
                bool bExternal = Convert.ToBoolean(botConfig["External"].ToString());
                bool bNeedTFAnalyzer = Convert.ToBoolean(botConfig["NeedTFAnalyzer"].ToString());

                decimal maxLossVMClosedTotal = Convert.ToDecimal(botConfig["MaxLossVMClosedTotal"]);
             
                int StockExhId = Convert.ToInt16(botConfig["StockExchId"]);


                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.LoadXml((string)botConfig["StrategySettings"]);


                XmlNode xmlNLStrategySettings = document.SelectSingleNode("StrategySettings");

                Dictionary<string, string> dictSettingsStrategy = new Dictionary<string, string>();
                foreach (XmlNode nd in xmlNLStrategySettings)
                    dictSettingsStrategy[nd.Name] = nd.InnerText;

                enmStrategysCode code = m_dictBotNameCode[type];



                string botName = GenerateBotName(type, num);
              


                Dictionary<string, CBotLimits> dictBotIsinLimits = new Dictionary<string, CBotLimits>();
                Dictionary<string, CTradingSettings> dictBotIsinTradeSettings = new Dictionary<string, CTradingSettings>();






                var res =  lstBotsInstrumentsConfig.FindAll(a => (int)a["BotId"] == num);
                if (res != null)
                {

                    List<string> listIsins = new List<string>();


                    foreach (var instConf in res)
                    {
                        string instrument = (string) instConf["Instrument"];
                        dictBotIsinLimits[instrument] = new CBotLimits( (int) instConf["MaxSendOrderRuntime"], 
                                                                       (int) instConf["MaxPosition"],
                                                                       (int)instConf["MaxAddedOrder"],
                                                                        (decimal)instConf["MaxLossVM"]);

                        long SL = 0;
                        long TP = 0;
                        int lot = 0;

                        if (instConf["StopLoss"] != null)
                            SL = Convert.ToInt32(instConf["StopLoss"]);

                        if (instConf["TakeProfit"] != null)
                            TP = Convert.ToInt32(instConf["TakeProfit"]);

                        if (instConf["Lot"] != null)
                            lot = Convert.ToInt32(instConf["Lot"]);


                        dictBotIsinTradeSettings[instrument] = new CTradingSettings(SL, TP, lot);

                        listIsins.Add(instrument);
                        
                      

                    }



                    CSettingsBot settingsBot = new CSettingsBot(code, bEnabled, listIsins, dictBotIsinLimits,
                                                              dictBotIsinTradeSettings, bExternal, bNeedTFAnalyzer, maxLossVMClosedTotal);


                    object stratSettings = null;
                    settingsBot.StrategySettings = stratSettings;


                    CBotBase bot = null;
                    if (!bExternal)
                    {
                        if (code == enmStrategysCode.StrategyTest)
                            bot = new CBotTester(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyTesterPos)
                            bot = new CBotTesterPos(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyHighLowContra)
                            bot = new CBotHighLowContra(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyTesterCrossFirst)
                            bot = new CBotTesterCrossFirst(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyTesterCrossSecond)
                            bot = new CBotTesterCrossSecond(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyTesterLimits)
                            bot = new CBotTesterLimits(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                        else if (code == enmStrategysCode.StrategyTrader)
                            bot = new CBotTrader(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);

                        Log("Created bot num=" + num + " botName=" + botName);
                        //listBots.Last().Start();
                    }
                    else
                    {
                        // if (code == enmStrategysCode.StrategyTesterExternal)
                        //   bot = CreateExternalBot(num, type, botName, settingsBot, dictSettingsStrategy, typeof(CBotTesterExternal).Assembly);

                    }

                    if (bot != null)
                    {
                        listBots.Add(bot);
                        _dealingServer.DictBots[bot.BotId] = bot;
                        bot.Start();
                        if (needSynchroOnLoad)
                            bot.SynchronizeOnBotReload();
                        //listBots.Last().Start();

                    }
                    Log("End CreateOneBotFromConfig");


                    ;



                }


              

                /*
                XmlNodeList xnList = botXML.SelectNodes("isins/isin");


                List<string> listIsins = new List<string>();
                Dictionary<string, CBotLimits> dictBotIsinLimits = new Dictionary<string, CBotLimits>();
                Dictionary<string, CTradingSettings> dictBotIsinTradeSettings = new Dictionary<string, CTradingSettings>();
                foreach (XmlNode lst in xnList)
                {
                    string isin = lst.InnerText;
                    listIsins.Add(isin);

                    dictBotIsinLimits[isin] = new CBotLimits(lst.Attributes["MaxSendOrderRuntime"].Value.ToString(), lst.Attributes["MaxPosition"].Value.ToString(),
                                                                 lst.Attributes["MaxAddedOrder"].Value.ToString(), lst.Attributes["MaxLossVM"].Value.ToString());


                    long SL = 0;
                    long TP = 0;
                    int lot = 0;
                    if (lst.Attributes["StopLoss"] != null)
                        SL = Convert.ToInt32((lst.Attributes["StopLoss"].Value.ToString()));

                    if (lst.Attributes["TakeProfit"] != null)
                        TP = Convert.ToInt32((lst.Attributes["TakeProfit"].Value.ToString()));

                    if (lst.Attributes["Lot"] != null)
                        lot = Convert.ToInt32((lst.Attributes["Lot"].Value.ToString()));


                    dictBotIsinTradeSettings[isin] = new CTradingSettings(SL, TP, lot);

                }

                object stratSettings = null;
                //if (type == "Supervisor") stratSettings = GeSettingsSupervisor(botXML);



                XmlNode xmlNLStrategySettings = botXML.SelectSingleNode("StrategySettings");
                Dictionary<string, string> dictSettingsStrategy = new Dictionary<string, string>();
                foreach (XmlNode nd in xmlNLStrategySettings)
                    dictSettingsStrategy[nd.Name] = nd.InnerText;



                CSettingsBot settingsBot = new CSettingsBot(code, bEnabled, listIsins, dictBotIsinLimits,
                                                            dictBotIsinTradeSettings, bExternal, bNeedTFAnalyzer,maxLossVMClosedTotal);
                settingsBot.StrategySettings = stratSettings;
                //--- exp ------


                CBotBase bot = null;

                //TO DO refactor it
                //-----------
                if (!bExternal)
                {
                    if (code == enmStrategysCode.StrategyTest)
                        bot = new CBotTester(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                    else if (code == enmStrategysCode.StrategyTesterPos)
                        bot = new CBotTesterPos(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);                
                    else if (code == enmStrategysCode.StrategyHighLowContra)
                        bot = new CBotHighLowContra(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                    else if (code == enmStrategysCode.StrategyTesterCrossFirst)
                        bot = new CBotTesterCrossFirst(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                    else if (code == enmStrategysCode.StrategyTesterCrossSecond)
                        bot = new CBotTesterCrossSecond(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                    else if (code == enmStrategysCode.StrategyTesterLimits)
                        bot = new CBotTesterLimits(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                    else if (code == enmStrategysCode.StrategyTrader)
                        bot = new CBotTrader (num, botName, settingsBot, dictSettingsStrategy, _dealingServer);

					Log("Created bot num="+num+" botName="+botName);
                    //listBots.Last().Start();
                }
                else
                {
                   // if (code == enmStrategysCode.StrategyTesterExternal)
                     //   bot = CreateExternalBot(num, type, botName, settingsBot, dictSettingsStrategy, typeof(CBotTesterExternal).Assembly);

                }

                if (bot != null)
                {
                    listBots.Add(bot);
                    _dealingServer.DictBots[bot.BotId] = bot;
                    bot.Start();
                    if (needSynchroOnLoad)
                        bot.SynchronizeOnBotReload();
                    //listBots.Last().Start();

                }
				Log("End CreateOneBotFromConfig");
                 */
            }
            catch (Exception e)
            {
                Error("CBotFactory.CreateOneBotFromConfig", e);

            }



        }



        





        //
        // if botNum ==0 than create all bots from config
        //else load only specific bot
        //Assume that if botNum ==0 than we reload bot
        private void CreateBotsFromConfig(long botNum =0)
        {

            try
            {
				Log("CreateBotsFromConfig start");


                //new code 2017-08-30
                  List<CBotBase> listBots = _dealingServer.ListBots;

                _dealingServer.DBCommunicator.WaitReadyForOperations();

                _dealingServer.WaitBotConfigLoaded();
                //List<Dictionary<string, object>> lstBotsConfig =  _dealingServer.DBCommunicator.LoadBotsConfig(_dealingServer.StockExchId);


                //List<Dictionary<string, object>> lstBotsInstrumentsConfig = _dealingServer.DBCommunicator.LoadBotInstrumentConfig (_dealingServer.StockExchId);

                lock (_dealingServer.LstBotsConfig)//2018-08-27
                {
                    foreach (var botConfig in _dealingServer.LstBotsConfig)
                    {
                        int num = (int)botConfig["number"];

                        if (botNum == 0 || num == botNum)
                        {
                            bool needSyncronize = false;
                            if (botNum != 0)
                                needSyncronize = true;

                            CreateOneBotFromConfig(botConfig, _dealingServer.LstBotsInstrumentsConfig, num, ref listBots, needSyncronize);
                        }
                    }
                }
                _dealingServer.IsAllBotLoaded = true;

                /*
                //old code
                List<CBotBase> listBots = _dealingServer.ListBots;
                //search number attribute
                foreach (XmlNode botXML in node)
                {
                    string stNum = botXML.Attributes["number"].Value.ToString();
                    int num = Convert.ToInt32(stNum);

                    if (botNum == 0 || num == botNum)
                    {
                        bool needSyncronize = false;
                        if (botNum != 0)
                            needSyncronize = true;

                        CreateOneBotFromConfig(botXML, num, ref listBots, needSyncronize);

                    }
                }
                _dealingServer.IsAllBotLoaded = true;
                 */
				Log("CreateBotsFromConfig end");
            }
            catch (Exception e)
            {
                Error("CBotFactory.CreateBotsFromConfig",e);
            }

        }




        //Removed 2018-04-26
        /*
        //Note: if  not put assembly, it will not work - exception "serialization..."
        private CBotBase CreateExternalBot(int num, string type, string botName, CSettingsBot settingsBot,
                                        Dictionary<string, string> dictSettingsStrategy,  Assembly asm)
                                                              
        {
            CBotBase bot = null;

            try
            {
                List<CBotBase> listBots = _dealingServer.ListBots;
                string stBotPath = Environment.GetEnvironmentVariable("BOT_PATH");


                string fullBotName = "Bot" + type;

                string pathToDll = stBotPath + fullBotName + @"\bin\Debug\" + fullBotName + ".dll";
                string fullClassName = "Plaza2Connector.C" + fullBotName;

                string domainName = CUtil.GetDomainNameByBotName(botName);
                AppDomain domain = AppDomain.CreateDomain(domainName);
                

                bot = (CBotBase)domain.CreateInstanceFromAndUnwrap(pathToDll, fullClassName);
                bot.Init(num, botName, settingsBot, dictSettingsStrategy, _dealingServer);
                bot.AppDomainBot = domain;
                //listBots.Add(bot);
                return bot;

            }
            catch (Exception e)
            {
                Error("CreateExternalBot ",e);
                return bot;

            }



        }
        */
        public void EnableBot(long botId)
        {
            _dealingServer.DictBots[botId].EnableBot();

        }




        public void DisableBot(long botId)
        {
            _dealingServer.DictBots[botId].DisableBot();
                                       
        }

        public void LoadBot(long  botId)
        {
            CreateBots(botId);
          
        }

        public void UnloadBot(long botId)
        {
       /*     if (!_dealingServer.DictBots.ContainsKey(botId)) return;

            CBotBase bot = _dealingServer.DictBots[botId];

            string nm = bot.Name;
            if (bot.IsExternal)
            {
                //string botName = CUtil.GetDomainNameByBotName(nm);
                bot.Dispose();
                //TO DO remove from dict and list

                
               

                _dealingServer.DictBots.Remove(botId);

                for (int i=0; i< _dealingServer.ListBots.Count; i++)
                    if (_dealingServer.ListBots[i].BotId == botId)
                    {
                        //m_plaza2Connector.ListBots[i] = null;
                        _dealingServer.ListBots.RemoveAt(i);
                        break;
                    }                                       
                //bot.Dispose();
               // bot.DisableBot();
                  
             //   m_plaza2Connector.DictBots.Remove(botId);
            }
            
            AppDomain.Unload(bot.AppDomainBot);

         //   WeakReference wr = new WeakReference(bot);

            bot = null;
         
            
            //GC.Collect(10);
          //  GC.WaitForPendingFinalizers();

            */
       

        }



        //if long botNum =0 create all bots
        //else bot with given number

        public void CreateBots(long botNum =0)
        {

            try
            {

				Log("Create bots start");
             
                CreateBotsFromConfig(botNum);
				Log("Create bots end");
            }

            catch (Exception e)
            {
                Error("CBotsFactory.CreateBots", e);

            }    
        }

        private string GenerateBotName(string type,int i)
        {

            string st = String.Format("{0,4}_Bot{1}", i.ToString("D4"),type);
            return st;


        }




        /*
        private string GenBotName(int i)
        {

            string st = String.Format("{0,4}_BotTest", i.ToString("D4"));
            return st;

       
        }
        */



    }
}
