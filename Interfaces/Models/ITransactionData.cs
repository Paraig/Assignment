using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Models
{
    public interface ITransactionData
    {
        long Id { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        string Currency { get; set; }
        decimal? Amount { get; set; }
        int RowNumber { get; set; }

        string ErrorString { get; }
        bool IsValid { get; } 
    }
}
