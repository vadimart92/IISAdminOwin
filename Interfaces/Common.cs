using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace IISAdmin.Interfaces
{
	public interface IEntity<TKey>
	{
		TKey Id {
			get;
			set;
		}
	}

	public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
	{
		void Create(TEntity entity);

		TEntity Get(TKey key);

		IEnumerable<TEntity> GetTopThousand();

		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

		void Update(TKey key, Action<TEntity> action);

		void Delete(TKey key);
	}

    public interface IQueryableRepository<TEntity, in TKey> where TEntity : IEntity<TKey> {
        IQueryable Get();
    }

	public interface ISqlConnectionProvider {
		SqlConnection GetOpenSqlConnection();
		SqlConnection GetOpenSqlConnection(SqlCredential credential);
		T ExecuteAction<T>(Func<SqlConnection, T> action, SqlCredential credential = null);
		void ExecuteAction(Action<SqlConnection> action, SqlCredential credential = null);
	}
}