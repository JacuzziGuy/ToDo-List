using SQLite;
using System.Linq;

namespace ToDo_List.Models
{
    [Table("ItemModel")]
    public class ItemModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
    }
}
