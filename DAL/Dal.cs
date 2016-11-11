using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Interfaces.Models;
using Models;
using Interfaces.DAL;
using System.Configuration;

namespace DAL
{
    public class Dal : IDal
    {
        private SqlConnection _connection;
        private SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                return _connection;
            }
        }

        private void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public int AddTransaction(ITransactionData data)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandType = CommandType.Text;

                command.CommandText = String.Format("INSERT INTO TransactionData (Account, Description, Currency, Amount) VALUES ('{0}', '{1}', '{2}', {3})",
                    data.Code, data.Description, data.Currency, data.Amount);

                return command.ExecuteNonQuery();
            }
            catch(Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return -1;
        }

        public IEnumerable<ITransactionData> LoadTransactionData()
        {
            var list = new List<ITransactionData>();

            try
            {
                var cmd = Connection.CreateCommand();

                cmd.CommandText = "SELECT Id, Account, Description, Currency, Amount FROM TransactionData";

                cmd.CommandType = CommandType.Text;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var transactionData = new TransactionData
                    {
                        Id = reader.GetInt64(0),
                        Code = GetStringValue(reader, 1),
                        Description = GetStringValue(reader, 2),
                        Currency = GetStringValue(reader, 3),
                        Amount = reader.GetDecimal(4)
                    };
                    list.Add(transactionData);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return list;
        }

        public ITransactionData LoadTransactionData(long id)
        {
            try
            {
                var cmd = Connection.CreateCommand();

                cmd.CommandText = String.Format("SELECT Id, Account, Description, Currency, Amount FROM TransactionData WHERE Id={0}", id);

                cmd.CommandType = CommandType.Text;

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var transactionData = new TransactionData
                    {
                        Id = reader.GetInt64(0),
                        Code = GetStringValue(reader, 1),
                        Description = GetStringValue(reader, 2),
                        Currency = GetStringValue(reader, 3),
                        Amount = reader.GetDecimal(4)
                    };
                    return transactionData;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return null;
        }

        public int UpdateTransactionData(ITransactionData data)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandType = CommandType.Text;

                command.CommandText = String.Format("UPDATE TransactionData SET Account = '{0}', Description = '{1}', Currency = '{2}', Amount = {3} WHERE Id={4}",
                    data.Code, data.Description, data.Currency, data.Amount, data.Id);

                return command.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return -1;

        }

        public int DeleteTransactionData(long Id)
        {
            try
            {
                var command = Connection.CreateCommand();

                command.CommandType = CommandType.Text;

                command.CommandText = String.Format("DELETE FROM TransactionData WHERE Id={0}", Id);

                return command.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return -1;

        }

        private string GetStringValue(SqlDataReader reader, int index)
        {

            if (!reader.IsDBNull(index))
                return reader.GetString(index);
            return string.Empty;
        }

    }
}
