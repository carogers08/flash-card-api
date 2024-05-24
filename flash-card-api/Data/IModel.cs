namespace flash_card_api.Data
{
    public interface IModel<T, I>
    {
        public T Load();
        public T GetById();
        public bool Insert();
        public bool Update();
        public bool Delete();
    }
}
