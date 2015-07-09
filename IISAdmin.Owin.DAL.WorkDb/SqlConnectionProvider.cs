using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL {

	public abstract class SqlConnectionProvider : ISqlConnectionProvider {

		#region Fields: Private

		private readonly string _connectionString;
		private readonly SqlCredential _credential;

		#endregion

		protected SqlConnectionProvider(string connectionString, SqlCredential credential) {
			Contract.Assert(!string.IsNullOrWhiteSpace(connectionString), "!string.IsNullOrWhiteSpace(_connectionString)");
			_connectionString = connectionString;
			_credential = credential;
		}

		#region Члены ISqlConnectionProvider

		public SqlConnection GetOpenSqlConnection() {
			var connection = new SqlConnection(_connectionString, _credential);
			OpenConnection(connection);
			return connection;
		}

		public SqlConnection GetOpenSqlConnection(SqlCredential credential) {
			var connection = new SqlConnection(_connectionString, credential);
			OpenConnection(connection);
			return connection;
		}

		public T ExecuteAction<T>(Func<SqlConnection, T> action, SqlCredential credential = null) {
			T result;
			using (var connection = GetOpenSqlConnection(credential)) {
				try {
					result = action(connection);
				}
				catch (SqlException ex) {
					if (IsDeadlockException(ex)) {
						result = action(connection);
					} else {
						throw;
					}
				}
			}
			return result;
		}

		public void ExecuteAction(Action<SqlConnection> action, SqlCredential credential = null) {
			using (var connection = GetOpenSqlConnection(credential)) {
				try {
					action(connection);
				}
				catch (SqlException ex) {
					if (IsDeadlockException(ex)) {
						action(connection);
					} else {
						throw;
					}
				}
			}
		}

		private bool IsDeadlockException(SqlException exception) {
			return exception.Number == 1205 //(ER_LOCK_WAIT_TIMEOUT) Lock wait timeout exceeded
					|| exception.Number == 1213; //(ER_LOCK_DEADLOCK) Deadlock found when trying to get lock
		}

		private void OpenConnection(SqlConnection sqlConnection) {
			if (sqlConnection.State == ConnectionState.Broken) {
				sqlConnection.Close();
				sqlConnection.Open();
			} else if (sqlConnection.State != ConnectionState.Open) {
				sqlConnection.Open();
			}
			Contract.Ensures(sqlConnection.State == ConnectionState.Open);
		}

		#endregion
	}
}
