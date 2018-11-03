using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashflare_true_info
{

    public class Info
    {
        public Sha256 sha256 = new Sha256();
        public class Sha256
        {
            public double hashrate10GHS = 0;
            public double unitPrice = 0;
            public double hashratePrice = 0;

            public double electricity = 0;
            public double income = 0;
            public double electricityProcent = 0;

            public double cleanperday = 0;
            public double cleanperdayProcent = 0;
            public double cleanperdayProcentOfInvested = 0;
            public double cleanperdayfor10ghs = 0;
            public int capitalPayback = 0;
            public int investedPayback = 0;
            public int doublement = 0;
            public int invdoublement = 0;
            public double cleanPerMonth = 0;

            public double invested = 0;
            public double profitability = 0;

            public string additionalInfo = "";
        }

        public Scrypt scrypt = new Scrypt();
        public class Scrypt
        {
            public double hashrate1MHS = 0;
            public double unitPrice = 0;
            public double hashratePrice = 0;

            public double electricity = 0;
            public double income = 0;
            public double electricityProcent = 0;

            public double cleanperday = 0;
            public double cleanperdayProcent = 0;
            public double cleanperdayProcentOfInvested = 0;
            public double cleanperdayfor1mhs = 0;
            public int capitalPayback = 0;
            public int investedPayback = 0;
            public int doublement = 0;
            public int invdoublement = 0;
            public double cleanPerMonth = 0;

            public double invested = 0;
            public double profitability = 0;

            public string additionalInfo = "";
        }

        public Ethash ethash = new Ethash();
        public class Ethash
        {
            public double hashrate1MHS = 0;
            public double unitPrice = 0;
            public double hashratePrice = 0;

            public double electricity = 0;
            public double income = 0;
            public double electricityProcent = 0;

            public double cleanperday = 0;
            public double cleanperdayProcent = 0;
            public double cleanperdayProcentOfInvested = 0;
            public double cleanperdayfor1mhs = 0;
            public int capitalPayback = 0;
            public int investedPayback = 0;
            public int doublement = 0;
            public int invdoublement = 0;
            public double cleanPerMonth = 0;

            public double invested = 0;
            public double profitability = 0;

            public string additionalInfo = "";
        }

        public General general = new General();
        public class General
        {
            public double price = 0;
            public double invested = 0;
            public double electricity = 0;
            public double income = 0;
            public double dollar_daily = 0;
            public double dollar_monthly = 0;

            public double profit_percent_of_price_daily = 0;
            public double profit_percent_of_invested_daily = 0;
            public double electricity_percent_of_income = 0;

            public int price_doublement_for_days = 0;
            public int capitalPayback = 0;
            public int investedPayback = 0;
            public int invested_doublement_for_days = 0;
            public double profitability = 0;

            public string additionalInfo = "";
            public double takenOutDollars = 0;
        }
    }
}
