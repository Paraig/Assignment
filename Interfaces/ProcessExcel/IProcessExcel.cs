using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ProcessExcel
{
    public interface IProcessExcel
    {
        int ProcessExcelFile(string fileName);

        IList<ITransactionData> FailedTransactions { get; }
    }
}
