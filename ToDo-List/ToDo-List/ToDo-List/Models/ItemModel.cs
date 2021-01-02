using SQLite;
namespace ToDo_List.Models
{
	[Table("ItemModel")]
	public class ItemModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int Num { get; set; }
		public string Name { get; set; }
		public bool Checked { get; set; }
	}
}
