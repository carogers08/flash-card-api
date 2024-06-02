using System.Data;

namespace flash_card_api.Data
{
    public interface IModel
    {
        public string GetById(int id);
        public string Insert(IModel obj);
        public string Update(IModel obj);
        public string Delete(int id);
    }
}
