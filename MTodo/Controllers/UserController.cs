using Microsoft.AspNetCore.Mvc;
using MTodo.Persistance.Context;

namespace MTodo.Controllers
{
	public class UserController:Controller
	{
		private readonly MTodoContext context;
		public UserController(MTodoContext context)
		{
			this.context = context;
		}
	}
}

