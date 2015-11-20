using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DataLayer
{
    public static class EfExtensionMethods
    {
        public static IEnumerable<TEntity> AddRange<TEntity>(this IDbSet<TEntity> dbset, IEnumerable<TEntity> entitiesToAdd) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).AddRange(entitiesToAdd);
        }

        public static IEnumerable<TEntity> RemoveRange<TEntity>(this IDbSet<TEntity> dbset, IEnumerable<TEntity> entitiesToDelete) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).RemoveRange(entitiesToDelete);
        }
    }
}
