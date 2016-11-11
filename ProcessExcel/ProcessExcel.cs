using Interfaces.DAL;
using Interfaces.Models;
using Interfaces.ProcessExcel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public class ProcessExcel : IProcessExcel
    {
        IDal _dal;
        public ProcessExcel(IDal dal)
        {
            _dal = dal;
            FailedTransactions = new List<ITransactionData>();
        }

        public int ProcessExcelFile(string fileName)
        {
            var book = new LinqToExcel.ExcelQueryFactory(fileName);
            book.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

            int count = 0;
            int rowNumber = 1;

            var tranactionData =
                from row in book.Worksheet("Sheet1")
                let item = new TransactionData
                {
                    Code = row["Account"].Cast<string>(),
                    Description = row["Description"].Cast<string>(),
                    Currency = row["Currency Code"].Cast<string>(),
                    Amount = row["Amount"].Cast<decimal>(),
                }
                select item;

            foreach (var datum in tranactionData)
            {
                datum.RowNumber = rowNumber++;
                if (datum.IsValid)
                {
                    _dal.AddTransaction(datum);
                    count++;
                }
                else
                {
                    FailedTransactions.Add(datum);
                }
            }

            return count;

        }

        public IList<ITransactionData> FailedTransactions
        {
            get; private set;
        }
    }
}
