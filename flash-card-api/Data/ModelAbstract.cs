using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace flash_card_api.Data
{
    public abstract class ModelAbstract<T> : IModel<T, int> where T: new()
    {
        protected abstract DatabaseService _dbio { get; }
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

        public string Insert(T obj)
        {
            string sql = _insertSql;
            var properties = GetPropertyNames(obj);

            foreach (var property in properties)
            {
                var value = obj.GetType().GetProperty(property.Key).GetValue(obj);
                sql.Replace(":" + property.Key, value == null ? "NULL" : value.ToString());
            }

            return sql;
        }

        public string Update(T obj)
        {
            string sql = _updateSql;
            var properties = GetPropertyNames(obj);

            foreach (var property in properties)
            {
                var value = obj.GetType().GetProperty(property.Key).GetValue(obj);
                sql.Replace(":" + property.Key, value == null ? "NULL" : value.ToString());
            }

            return sql;
        }

        public T Load(DataRow row) 
        {
            T response = new T();
            var properties = GetPropertyNames(response);

            foreach (DataColumn c in row.Table.Columns)
            {
                string propName = properties[c.ColumnName];
                PropertyInfo pi = response.GetType().GetProperty(propName);
                if (pi != null)
                {
                    pi.SetValue(response, row[c], null);
                }
            }

            return response;
        }

        /*** Helper Methods ***/
        public static Dictionary<string, string> GetPropertyNames(object obj)
        {
            Dictionary<string, string> kvPairs = new Dictionary<string, string>();

            foreach (var property in obj.GetType().GetProperties())
            {
                // Ignore attributes that are not mapped to the DB
                if (property.GetType() == typeof(NotMappedAttribute))
                    continue;

                var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
                var jsonPropertyName = jsonProperty?.PropertyName;

                if (jsonPropertyName != null)
                    kvPairs.TryAdd(jsonPropertyName, property.Name);
                else
                    kvPairs.TryAdd(property.Name, property.Name);
            }

            return kvPairs;
        }
    }
}
