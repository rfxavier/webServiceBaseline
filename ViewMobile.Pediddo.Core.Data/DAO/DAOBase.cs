using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Data.DAO
{
    public abstract class DAOBase<T, IdT> where T : class
    {

        public virtual T GetById(IdT id)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            return db.GetTable<T>().ToList().SingleOrDefault(e => Convert.ToInt32(e.GetType().GetProperties().First().GetValue(e, null)) == Convert.ToInt32(id));
        }
        public virtual List<T> GetAll()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            Table<T> someTable = db.GetTable(typeof(T)) as Table<T>;
            return someTable.ToList<T>();
        }
        public virtual T Save(T entity)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            ITable tab = db.GetTable(entity.GetType());
            tab.InsertOnSubmit(entity);
            db.SubmitChanges();
            return entity;
        }
        public virtual T Update(T newEntity, T originalEntity)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            ITable tab = db.GetTable(newEntity.GetType());
            if (originalEntity == null)
            {
                tab.Attach(newEntity, true);
            }
            else
            {
                tab.Attach(newEntity, originalEntity);
            }
            db.SubmitChanges();
            return newEntity;
        }
        public virtual void Delete(T entity)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            ITable tab = db.GetTable(entity.GetType());
            tab.Attach(entity);
            tab.DeleteOnSubmit(entity);
            db.SubmitChanges();
        }

    }
}
