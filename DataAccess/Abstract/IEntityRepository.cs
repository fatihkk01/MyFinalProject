using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    //Generic Constraint
    //class : referans tip
    //T ya IEntity olabilir ya da IEntityden implemente edilmiş bir class olabilir.
    //IEntity : IEntity olabilir ya da IEntity implemente eden bir nesne olabilir.
    //new() : newlenebilir olmalı (IEntity koymayı engellemek için)
    public interface IEntityRepository<T> where T:class,IEntity,new() 
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T,bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
