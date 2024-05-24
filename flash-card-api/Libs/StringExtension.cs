namespace flash_card_api.Libs
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }
    }
}
