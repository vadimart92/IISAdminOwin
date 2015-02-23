using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IISAdmin.Interfaces {

	public interface IEntity<TKey> {
		TKey Id {
			get;
			set;
		}
	}

	public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey> {
		void Create(TEntity entity);
		TEntity Get(TKey key);
		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
		void Update(TEntity entity);
		void Delete(TKey id);
	}
}
