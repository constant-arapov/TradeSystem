using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseConnector
{
    class CDBLayer
    {

        public void UpdateBestBidAsk(decimal bid, decimal ask)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                //TODO query !
                Quote quote = db.Quotes.First();

                quote.Bid = bid;
                quote.Ask = ask;

                db.SaveChanges();


            }

        }
    }
}
