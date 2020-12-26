using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ToDo_List.Models;

namespace ToDo_List
{
	public partial class MainPage : ContentPage
	{
		List<ItemModel> items = new List<ItemModel>();
		public MainPage()
		{
			InitializeComponent();
		}
		private void Button_Clicked(object sender, EventArgs e)
		{
			items.Add(new ItemModel { Id = items.Count,Text = entry.Text });
		}
	}
}
