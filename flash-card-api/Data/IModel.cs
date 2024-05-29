namespace flash_card_api.Data
{
    public interface IModel<T, I>
    {
        public T Load();
        public string GetById();
        public string Insert();
        public string Update();
        public string Delete();
    }
}
