using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
			IEnumerable<TodoListViewModel> models=context.todos.Where(p=> p.isDeleted==false).Select(p=> new TodoListViewModel{
				Id=p.Id.ToString(),
				Task=p.Task,
				isCompleted=p.IsCompleted
			}).ToList();
			return View("Index",models);
		}
		[HttpGet]
		public IActionResult detail(string Id)
		{
			TodoListViewModel? todo=context.todos.Where(q=> q.Id==Guid.Parse(Id)).Select(p=> new TodoListViewModel{
				Task=p.Task,
				isCompleted=p.isDeleted,
				Id=p.Id.ToString()
			}).FirstOrDefault();
			if(todo==null){
				return RedirectToAction("Index","Home");
			}
			return View("Detail",todo);
		}

		[HttpGet]
		public IActionResult insert()
		{
			return View("Add");
		}
		[HttpGet]
		public IActionResult update(string Id)
		{
			TodoCreateViewModel? todo=context.todos.Where(q=> q.Id==Guid.Parse(Id)).Select(p=> new TodoCreateViewModel{
				Task=p.Task,
				isCompleted=p.isDeleted
			}).FirstOrDefault();
			if(todo==null){
				return RedirectToAction("Index","Home");
			}
			return View("Update",todo);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("insert")]
		[Authorize]
		public IActionResult insertAction(TodoCreateViewModel model)
		{
			if(ModelState.IsValid){
				Claim Id=HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				Todo todo = new Todo(){
					Task=model.Task,
					IsCompleted=model.isCompleted,
					CreatedUser=Guid.Parse(Id.Value)
				};
				context.todos.Add(todo);
				context.SaveChanges();
				TempData["SuccessMessage"]="Todo is Not successfully Created!";
			}
			TempData["ErrorMessage"]="Todo is successfully Created!";
			return RedirectToAction("Index", "Todo");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("update")]
		public IActionResult updateAction(TodoCreateViewModel model)
		{
			if(ModelState.IsValid){
				Claim Id=HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				Todo todo = new Todo{
					Task=model.Task,
					IsCompleted=model.isCompleted,
					CreatedUser=Guid.Parse(Id?.Value)
				};
				context.todos.Add(todo);
				context.SaveChanges();
				TempData["SuccessMessage"]="Todo is Not successfully Created!";
			}
			TempData["ErrorMessage"]="Todo is successfully Created!";
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
		public IActionResult finished(string Id,bool finished)
		{
			if(!string.IsNullOrEmpty(Id)){
				User? user=context.users.Find(Guid.Parse(Id));

			}
			return Ok(new { message = "message created successfully." });
		}

	}
}

