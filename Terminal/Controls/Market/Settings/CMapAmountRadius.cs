using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.Controls.Market.Settings
{
    //TODO from config, DP etc
    //contract - must be more then zero
    public class CMapAmountRadius : List <CAmountRadius>
    {

        private int _minAmount;
        private double _minRadius;

        public CMapAmountRadius(int minAmount,  double minRadius) : base()
        {
            _minAmount = minAmount;
            _minRadius = minRadius;


           Add (1,      7 );
           Add (10,    10.0 );
           Add (100,   14.0 );
           Add (1000,  18.0 );
           Add (10000, 21.0 );
           Add (100000, 25.0);
  

        }

        public void Add(int amonunt, double radius)
        {
            Add(new CAmountRadius(amonunt, radius));


        }

        public double GetRadius(int amount)
        {

            if (amount < _minAmount)
                return _minRadius;


            int i;

            for (i = 0; i < Count; i++)
                if (amount < this[i].Amount)
                    if (i>0)
                        return this[i - 1].Radius;
                    else //i==0
                        return this[i].Radius;

            return this[i - 1].Radius;//for last element
        }




    }

    public class CAmountRadius 
    {
        public int Amount;
        public double Radius;

        public CAmountRadius(int amount, double radius)
        {
            Amount = amount;
            Radius = radius;
        }


    }


}
