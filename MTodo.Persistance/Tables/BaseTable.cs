using System;
namespace MTodo.Persistance.Tables
{
	public class BaseTable
	{
		public Guid Id { set; get; } = Guid.NewGuid();
		public DateTime createdDate { set; get; } = DateTime.UtcNow;
		public DateTime updatedDate { set; get; } = DateTime.UtcNow;
		public bool isDeleted { set; get; } = false;
		public BaseTable()
		{
		}
	}
}

