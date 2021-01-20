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
        public string Title { get { return Text.Split(' ').Length != 1 ? $"{Text.Split(' ')[0]} {Text.Split(' ')[1]}" : Text.Split(' ')[0]; } }
        public bool Checked { get; set; }
    }
}
