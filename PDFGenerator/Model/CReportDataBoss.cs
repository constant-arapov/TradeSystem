using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PDFGenerator.Model;

namespace PDFGenerator.View
{
    public class CReportDataBoss
    {

        public DateTime DtSessionBegin { get; set; }
        public DateTime DtSessionEnd { get; set; }


        public List<List<string>> AccountsBeginEnd = new List<List<string>>();
        public List<string> HeaderAccountsBeginEnd = new List<string>();

		public List<List<string>> AllAccountsMoney = new List<List<string>>();
		public List<string> HeaderAllAccountsMoney = new List<string>();

        public List<string> HeaderWalletChange = new List<string>();
        public List<List<string>> AllWalletChange = new List<List<string>>();

        public List<List<string>> AllAccountsSumsOfSession = new List<List<string>>();
        public List<string> HeaderAllAccountsSumsOfSession = new List<string>();

        public CReportDataBoss()
        {

        }

		public void GenAllAccountsMoney(List<Dictionary<string, object>> inpAccountsBeginEnd)
		{

			HeaderAllAccountsMoney = new List<string>(){ "Счет", "Ф.И.О.","Средства" };
			decimal total = 0;
			inpAccountsBeginEnd.ForEach(row =>
											{
												AllAccountsMoney.Add(
												new List<string>()
												{													
													row["id"].ToString(),
													row["name"].ToString(),
													((decimal)row["money_avail"]).ToString("n2")

												}
												);
												total += (decimal)row["money_avail"];
											}								
										);


			AllAccountsMoney.Add(new List<string>
								{
									
									"",
									"Итого:",
									total.ToString("n2")
								}
							);
		}


        public void GenWalletChange(List<Dictionary<string, object>> inpGenWalletChange)
        {

            HeaderWalletChange = new List<string>() { "Дата", "Средства", "Валюта" };

            inpGenWalletChange.ForEach(
                                    row =>
                                    {
                                        AllWalletChange.Add(
                                            new List<string>()
                                            {
                                                row["Dt"].ToString(),
                                                Convert.ToDecimal(row["Balance"]).ToString("N02"),
                                                row["Currency"].ToString()

                                            });

                                    }
                                    );



            //must be two rows only
            if (inpGenWalletChange.Count == 2)
            {
                decimal c0 = Convert.ToDecimal(inpGenWalletChange[0]["Balance"]);
                decimal c1 = Convert.ToDecimal(inpGenWalletChange[1]["Balance"]);

                decimal dc = c1 - c0;
                AllWalletChange.Add(new List<string>() { "Изменение:", dc.ToString("N02"), "" });

            }
            
               
            




        }




        public void GetAllAccountsSumsOfSession(List<Dictionary<string,object>>  inpAllAccountsSumOfSession)
        {


            HeaderAllAccountsSumsOfSession = new List<string>() { "Счет", "Ф.И.О.","Маржа", "Единая комиссия", "Финрез" };

            decimal sumRubClean = 0;
            decimal sumFeeTotal = 0;
            decimal sumClosedRub = 0;


            inpAllAccountsSumOfSession.ForEach(row =>
                                             {
                                                 AllAccountsSumsOfSession.Add(
                                                 new List<string>()
												{
													row["account_trade_Id"].ToString(),
													row["name"].ToString(),
													((decimal)row["VMClosedRUBClean"]).ToString("n2"),
                                                    ((decimal)row["FeeTotal"]).ToString("n2"),
                                                    ((decimal)row["VMClosedRUB"]).ToString("n2")



												}
                                                 );

                                                 sumRubClean += (decimal)row["VMClosedRUBClean"];
                                                 sumFeeTotal += (decimal)row["FeeTotal"];
                                                 sumClosedRub += (decimal)row["VMClosedRUB"];
                                             }
                                             );


            AllAccountsSumsOfSession.Add(new List<string>() {  
																"",
																 "Итого:",
                                                                sumRubClean.ToString("n2"),
                                                                sumFeeTotal.ToString("n2"), 
                                                                sumClosedRub.ToString("n2")
                                                              }
                                        );








        }


        public void GenAccountsBeginEnd(List<Dictionary<string, object>> inpAccountsBeginEnd)
        {

            SortedDictionary<int, Dictionary<string,string>> data = new SortedDictionary<int, Dictionary<string,string>>();
			decimal endPeriodSummary = 0;

            inpAccountsBeginEnd.ForEach(row =>
            {
                int account_trade_id = (int)row["account_trade_id"];
                if (!data.ContainsKey(account_trade_id))
                    data[account_trade_id] = new Dictionary<string, string>();

                data[account_trade_id]["account_trade_id"] = account_trade_id.ToString();
				data[account_trade_id]["name"] = row["name"].ToString();
					

                if ((string)row["beginend"] == "period_begin")                
                    data[account_trade_id]["Money_begin_preiod"] = ((decimal)row["Money_before"]).ToString("n2");
				else if ((string)row["beginend"] == "period_end")
				{
					data[account_trade_id]["Money_end_preiod"] = ((decimal)row["Money_after"]).ToString("n2");
					endPeriodSummary += ((decimal)row["Money_before"]);
				}

            });
            
            

          //  List<Dictionary<string, string>> outData = new List<Dictionary<string, string>>();
            foreach (var kvp in data)
            {
                
                AccountsBeginEnd.Add(new List <string> 
                                        {
                                         kvp.Value["account_trade_id"],
										 kvp.Value["name"],
                                         kvp.Value["Money_begin_preiod"],
                                         kvp.Value["Money_end_preiod"]
                                        });
            }

		
            
        //   Comparison<Dictionary<string,string> comp= new Comparison<Dictionary<string,string>


        }

		


        public void SetSessionBeginEnd(Dictionary<string, object> data)
        {
            DtSessionBegin = (DateTime)data["DtBegin"];
            DtSessionEnd = (DateTime)data["DtEnd"];




        }


        //System.Comparison<Dictionary<string,string>> 

    }
}
