using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TransactionData : ITransactionData
    {
        private StringBuilder _sb;

        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public int RowNumber { get; set; }

        public bool IsValid
        {
            get
            {
                return !(String.IsNullOrEmpty(Code)
                    || String.IsNullOrEmpty(Description)
                    || String.IsNullOrEmpty(Currency)
                    || Amount == null
                    || !CheckCurrency());
            }
        }

        public string ErrorString 
        {
            get
            {
                _sb = new StringBuilder();
                Check(Code, "the code is missing");
                Check(Description, "the description is missing");
                Check(Currency, "the currency is missing");
                Check(Amount, "the amount is missing");
                Check("the currency is invalid");
                return _sb.ToString(); 
            } 
        }

        private bool Check(string str, string error)
        {
            if(String.IsNullOrEmpty(str))
            {
                if (_sb.Length != 0)
                    _sb.Append(",");
                _sb.Append(error);
                return false;
            }
            return true;
        }

        private bool Check(decimal? value, string error)
        {
            if(value == null)
            {
                if (_sb.Length != 0)
                    _sb.Append(",");
                _sb.Append(error);
                return false;
            }
            return true;
        }

        private bool CheckCurrency()
        {
            if (String.IsNullOrEmpty(Currency))
                return true;

            IEnumerable<string> currencySymbols = CultureInfo.GetCultures(CultureTypes.SpecificCultures) //Only specific cultures contain region information
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct()
                .OrderBy(x => x);

            return currencySymbols.Any(c => c == Currency);
        }

        private bool Check(string error)
        {
            if (CheckCurrency())
                return true;

            if (_sb.Length != 0)
                _sb.Append(",");
            _sb.Append(error);

            return false;

        }
    }
}
