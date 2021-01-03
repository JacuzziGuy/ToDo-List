using SQLite;
using System;

namespace ToDo_List.DB
{
	public class Constants
	{
		public static SQLiteConnection DataBasePath()
		{
			return new SQLiteConnection($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/ToDoList.sqlite");
		}
	}
}
