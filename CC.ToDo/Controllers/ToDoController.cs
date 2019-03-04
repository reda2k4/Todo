using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC.ToDo.Data.LiteDB;
using CC.ToDo.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CC.ToDo.Controllers
{
    [Authorize]
    [ApiController]
    public class ToDoController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private IUserTaskRepository _repository;

        public ToDoController(UserManager<IdentityUser> userManager, IUserTaskRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        [Route("[controller]")]
        public IActionResult Index()
        {
            return View();
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!Guid.TryParse(user?.Id, out var userId))
            {
                return BadRequest("Unable to get user ID");
            }

            var tasks = await _repository.GetToDoListAsync(userId);

            var activeTasks = tasks?.Where(x => !x.IsRemoved);
            return Ok(activeTasks);
        }
        
        // POST api/values
        [HttpPost]
        [Authorize]
        [Route("api/[controller]")]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!Guid.TryParse(user?.Id, out var userId))
            {
                return BadRequest("Unable to get user ID");
            }

            var todoUser = await _repository.GetUserAsync(userId);
            if (todoUser == null)
            {
                todoUser = new User
                {
                    Id = userId,
                    Tasks = new List<ToDoTask>()
                };
            }

            todoUser.Tasks.Add(new ToDoTask
            {
                DateAdded=DateTime.Now,
                DateModified=DateTime.Now,
                Description=value,
                Id = Guid.NewGuid(),
                IsComplete=false,
                IsRemoved=false
            });

            await _repository.StoreAsync(todoUser);

            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        [Route("api/[controller]")]
        public async Task<ActionResult> Put([FromBody] ToDoTask task)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!Guid.TryParse(user?.Id, out var userId))
            {
                return BadRequest("Unable to get user ID");
            }

            var todoUser = await _repository.GetUserAsync(userId);
            if (todoUser == null)
            {
                return BadRequest("User not found");
            }

            var userTask = todoUser.Tasks.FirstOrDefault(x => x.Id == task.Id);

            if (userTask == null)
            {
                return BadRequest($"Task ID {task.Id} not found");
            }

            userTask.Description = task.Description;
            userTask.DateModified = DateTime.Now;
            userTask.IsComplete = task.IsComplete;

            await _repository.StoreAsync(todoUser);

            return Ok();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        [Route("api/[controller]")]
        public async Task<ActionResult> Delete(Guid taskId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!Guid.TryParse(user?.Id, out var userId))
            {
                return BadRequest("Unable to get user ID");
            }

            var todoUser = await _repository.GetUserAsync(userId);
            if (todoUser == null)
            {
                return BadRequest("User not found");
            }

            var userTask = todoUser.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (userTask == null)
            {
                return BadRequest($"Task ID {taskId} not found");
            }

            userTask.IsRemoved = true;

            await _repository.StoreAsync(todoUser);

            return Ok();
        }
    }
}