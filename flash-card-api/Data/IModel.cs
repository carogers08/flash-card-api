using System.Data;

namespace flash_card_api.Data
{
    public interface IModel
    {
        public string GetById(int id);
        public string Insert(object obj);
        public string Update(object obj);
        public string Delete(int id);
    }
}
