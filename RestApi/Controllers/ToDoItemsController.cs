using Data.Models;
using Data.Models.DTO;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestApi.Controllers
{
    /// <summary>
    /// Controller used to handle HTTP requests for ToDoItem Rest API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoItemDbService toDoItemDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemsController"/> class.
        /// </summary>
        /// <param name="dbService">Database service</param>
        public ToDoItemsController(IToDoItemDbService dbService)
        {
            toDoItemDbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        }

        /// <summary>
        /// GET /api/todoitems
        /// </summary>
        /// <returns>List of ToDoItem</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ToDoItemDto>> GetToDoItems()
        {
            return HandleErrors<IEnumerable<ToDoItemDto>>(() => toDoItemDbService.GetToDoItems().ToList());
        }

        /// <summary>
        /// GET /api/todoitems/{id}
        /// </summary>
        /// <param name="id">ToDoItem identifier</param>
        /// <returns>ToDoItem based on specified ID</returns>
        [HttpGet("{id:long}")]
        public ActionResult<ToDoItemDto> GetToDoItem(long id)
        {
            return HandleErrors(() => toDoItemDbService.GetToDoItem(id));
        }

        /// <summary>
        /// POST /api/todoitems
        /// </summary>
        /// <param name="toDoItem">Request body that contains ToDoItem for creation</param>
        /// <returns>Created ToDoItem</returns>
        [HttpPost]
        public ActionResult<ToDoItemDto> CreateToDoItem(ToDoItem? toDoItem)
        {
            return HandleErrors(() => toDoItemDbService.CreateToDoItem(toDoItem));
        }

        /// <summary>
        /// Handle errors in each REST API endpoint
        /// </summary>
        /// <param name="cb">Callback function</param>
        /// <returns>ActionResult depending on Type</returns>
        private ActionResult<T> HandleErrors<T>(Func<T> cb)
        {
            try
            {
                return Ok(cb());
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }
    }
}