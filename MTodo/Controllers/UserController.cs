using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTodo.Persistance.Context;
using MTodo.Persistance.Tables;

namespace MTodo.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly MTodoContext context;
		public UserController(MTodoContext context)
		{
			this.context = context;
		}

		[HttpGet]
		public IActionResult profile()
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (claim != null)
			{
				ProfileViewModel? model = context.users.Where(p => p.Id == Guid.Parse(claim)).Select(p => new ProfileViewModel
				{
					Name = p.UserName,
					Email = p.Email,
					Address = p.Address
				}).FirstOrDefault();
				return View("profile", model);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult edit()
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (claim != null)
			{
				ProfileViewModel? model = context.users.Where(p => p.Id == Guid.Parse(claim)).Select(p => new ProfileViewModel
				{
					Name = p.UserName,
					Email = p.Email,
					Address = p.Address
				}).FirstOrDefault();
				return View("edit", model);
			}
			return RedirectToAction("Index", "Home");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Edit")]
		public IActionResult editAction(ProfileViewModel model)
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (claim != null)
			{
				User? user = context.users.Where(p => p.Id == Guid.Parse(claim)).FirstOrDefault();
				if (user != null)
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.Address = model.Address;
					context.users.Update(user);
					context.SaveChanges();
					return RedirectToAction("profile", "User");
				}
			}
			return View("edit", model);
		}
		[HttpGet]
		public IActionResult Home()
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<TodoListViewModel> models=Enumerable.Empty<TodoListViewModel>();
			if (claim != null)
			{
				models = context.todos.Where(p => p.createdDate.ToLocalTime().Date == DateTime.Now.Date && p.isDeleted == false && p.CreatedUser==Guid.Parse(claim)).Select(profile => new TodoListViewModel
				{
					Task = profile.Task,
					isCompleted = profile.IsCompleted
				});
			}
			return View(models);
		}
	}
}

