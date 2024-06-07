using Newtonsoft.Json;
using System.Text.Json;

namespace flash_card_api.Data.Models.Entities
{
    public class FlashCard : ModelAbstract
    {
        protected override string _selectSql { get; }
        protected override string _insertSql { get; }
        protected override string _updateSql { get; }
        protected override string _deleteSql { get; }

        public FlashCard()
        {
            _selectSql = "SELECT [id],[key],[value],[success_chance],[chance_category] FROM FlashCard";
            _insertSql = "INSERT INTO [FlashCard] ([id],[key],[value],[success_chance],[chance_category]) VALUES (:";
            _insertSql = "SELECT [id],[key],[value],[success_chance],[chance_category] FROM FlashCard";
            _insertSql = "SELECT [id],[key],[value],[success_chance],[chance_category] FROM FlashCard";
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("success_chance")]
        public decimal SuccessChance { get; set; }
        public ChanceCategory ChanceCategory { get; set; }
    }
}
