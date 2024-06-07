using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using flash_card_api.Libs;
using Newtonsoft.Json;

namespace flash_card_api.Data
{
    public abstract class ModelAbstract : IModel
    {
        protected abstract string _selectSql { get; }
        protected abstract string _insertSql { get; }
        protected abstract string _updateSql { get; }
        protected abstract string _deleteSql { get; }

        public string GetById(int id)
        {
            return _selectSql + " WHERE [Id] = " + id;
        }

        public string Delete(int id)
        {
            return _deleteSql + "WHERE [Id] = " + id;
        }

        public string Insert(object obj)
        {
            string sql = _insertSql;
            var properties = obj.GetPropertyNames();

            foreach (var property in properties)
            {
                var value = obj.GetType().GetProperty(property.Key).GetValue(obj);
                sql.Replace(":" + property.Key, value == null ? "NULL" : value.ToString());
            }

            return sql;
        }

        public string Update(object obj)
        {
            string sql = _updateSql;
            var properties = obj.GetPropertyNames();

            foreach (var property in properties)
            {
                var value = obj.GetType().GetProperty(property.Key).GetValue(obj);
                sql.Replace(":" + property.Key, value == null ? "NULL" : value.ToString());
            }

            return sql;
        }
    }
}
