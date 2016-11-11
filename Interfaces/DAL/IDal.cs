using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DAL
{
    public interface IDal
    {
        int AddTransaction(ITransactionData data);
        IEnumerable<ITransactionData> LoadTransactionData();
        ITransactionData LoadTransactionData(long id);
        int UpdateTransactionData(ITransactionData data);
        int DeleteTransactionData(long Id);
    }
}
