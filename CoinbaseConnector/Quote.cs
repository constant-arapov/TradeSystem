using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace CoinbaseConnector
{
    public class Quote
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;UserId=root;Password=profinvest;database=ctc_exchange;");
        }
    }


}
