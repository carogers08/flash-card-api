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

        public T GetById(int id)
        {
            string sql = _selectSql + " WHERE [Id] = " + id;
            T response = default(T);

            try
            {
                DataTable dt = _dbio.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    response = Load(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                _dbio.LogError(sql, ex.Message);
            }

            return response;
        }

        // calebx - these don't belong in this file. It would require you to add a dbio to every object you create. Find a different place for this stuff
        public bool Insert(T obj)
        {

        }

        public T Load(DataRow row) 
        {
            T response = new T();
            var properties = RetrievalUsingReflection(response);

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
        public static Dictionary<string, string> RetrievalUsingReflection(object obj)
        {
            Dictionary<string, string> kvPairs = new Dictionary<string, string>();

            foreach (var property in obj.GetType().GetProperties())
            {
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
