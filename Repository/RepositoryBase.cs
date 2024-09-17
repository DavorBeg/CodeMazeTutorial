using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected RepositoryContext _RepositoryContext;
        protected RepositoryBase(RepositoryContext RepositoryContext)
        {
            _RepositoryContext = RepositoryContext;	
        }




        public IQueryable<T> FindAll(bool trackerChanges)
		{
			return trackerChanges ? _RepositoryContext.Set<T>() : _RepositoryContext.Set<T>().AsNoTracking();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackerChanges)
		{
			return trackerChanges ? _RepositoryContext.Set<T>().Where(expression) : _RepositoryContext.Set<T>().Where(expression).AsNoTracking();
		}

		public void Create(T entity) => _RepositoryContext.Set<T>().Add(entity);

		public void Delete(T entity) => _RepositoryContext.Set<T>().Remove(entity);

		public void Update(T entity) => _RepositoryContext.Set<T>().Update(entity);

	}
}
