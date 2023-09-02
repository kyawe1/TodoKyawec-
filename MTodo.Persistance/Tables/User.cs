using System;
namespace MTodo.Persistance.Tables
{
	public class User:BaseTable
	{
		public string UserName { set; get; }
		public string Password { set; get; }
		public string Address { set; get; }
		public string Email { set; get; }
		public User()
		{
		}
	}
}

