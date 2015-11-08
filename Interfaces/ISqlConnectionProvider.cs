using System;
using System.Data.SqlClient;

namespace IISAdmin.Interfaces {
    public interface ISqlConnectionProvider {
        SqlConnection GetOpenSqlConnection();
        SqlConnection GetOpenSqlConnection(SqlCredential credential);
        T ExecuteAction<T>(Func<SqlConnection, T> action, SqlCredential credential = null);
        void ExecuteAction(Action<SqlConnection> action, SqlCredential credential = null);
    }
}