using SQLite;
using System.Drawing;

namespace ToDo_List.Models
{
    [Table("ItemModel")]
    public class ItemModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public int Importance { get; set; }
        public Color ImportanceColor
        {
            get
            {
                switch (Importance)
                {
                    case 1:
                        return Color.FromArgb(255, 235, 52, 52);
                    case 2:
                        return Color.FromArgb(255, 235, 120, 52);
                    case 3:
                        return Color.FromArgb(255, 235, 200, 52);
                    case 4:
                        return Color.FromArgb(255, 195, 235, 52);
                    case 5:
                        return Color.FromArgb(255, 110, 235, 52);
                    default:
                        return Color.FromArgb(255, 181, 181, 181);
                }
            }
        }
        public bool Checked { get; set; }
    }
}
