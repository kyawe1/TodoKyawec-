using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace MTodo.Persistance.Tables
{
	public class Todo:BaseTable
	{
		public string Task { set; get; }
		public bool IsCompleted { set; get; }
		public Guid CreatedUser { set; get; }
		[ForeignKey("CreatedUser")]
		public virtual User? User { set; get; }
		public Todo()
		{
		}
	}
}

