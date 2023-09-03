using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTodo.Persistance.Context;
using MTodo.Persistance.Tables;

namespace MTodo.Controllers
{
	[Authorize]
	public class TodoController : Controller
	{
		private readonly MTodoContext context;
		public TodoController(MTodoContext context)
		{
			this.context = context;
		}
		public IActionResult index()
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<TodoListViewModel> models = Enumerable.Empty<TodoListViewModel>();
			if (claim != null)
			{
				models = context.todos.AsNoTracking().Where(p => p.isDeleted == false && p.CreatedUser == Guid.Parse(claim)).Select(p => new TodoListViewModel
				{
					Id = p.Id.ToString(),
					Task = p.Task,
					isCompleted = p.IsCompleted
				}).ToList();
				return View("Index", models);
			}
			return View("Index", models);
		}
		[HttpGet]
		public IActionResult detail(string Id)
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<TodoListViewModel> models = Enumerable.Empty<TodoListViewModel>();
			TodoListViewModel? todo = null;
			if (claim != null)
			{
				todo = context.todos.Where(q => q.Id == Guid.Parse(Id)).Select(p => new TodoListViewModel
				{
					Task = p.Task,
					isCompleted = p.isDeleted,
					Id = p.Id.ToString()
				}).FirstOrDefault();
				if (todo != null)
				{
					return View("Detail", todo);
				}
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult insert()
		{
			return View("Add");
		}
		[HttpGet]
		public IActionResult update(string Id)
		{
			string? claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			TodoCreateViewModel? todo = null;
			if (claim != null)
			{
				todo = context.todos.Where(q => q.Id == Guid.Parse(Id) && Guid.Parse(claim) == q.CreatedUser).Select(p => new TodoCreateViewModel
				{
					Task = p.Task,
					isCompleted = p.isDeleted
				}).FirstOrDefault();
				if (todo != null)
				{
					return View("Update", todo);
				}
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("insert")]
		[Authorize]
		public IActionResult insertAction(TodoCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Claim Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				Todo todo = new Todo()
				{
					Task = model.Task,
					IsCompleted = model.isCompleted,
					CreatedUser = Guid.Parse(Id.Value)
				};
				context.todos.Add(todo);
				context.SaveChanges();
				TempData["SuccessMessage"] = "Todo is Not successfully Created!";
			}
			TempData["ErrorMessage"] = "Todo is successfully Created!";
			return RedirectToAction("Index", "Todo");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("update")]
		public IActionResult updateAction(string Id, TodoCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Claim? claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (claim != null)
				{
					Todo? todo = context.todos.Where(p => p.isDeleted == false && p.Id == Guid.Parse(Id) && p.CreatedUser == Guid.Parse(claim.Value)).FirstOrDefault();
					if (todo != null)
					{
						todo.Task = model.Task;
						todo.IsCompleted = model.isCompleted;
						context.todos.Update(todo);
						context.SaveChanges();
						TempData["SuccessMessage"] = "Todo is Not successfully Updated!";
					}
				}
			}
			TempData["ErrorMessage"] = "Todo is successfully Created!";
			return RedirectToAction("Index", "Todo");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult delete()
		{
			return View("Update");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult finished(string Id, bool finished)
		{
			if (!string.IsNullOrEmpty(Id))
			{
				User? user = context.users.Find(Guid.Parse(Id));
			}
			return Ok(new { message = "message created successfully." });
		}

	}
}

