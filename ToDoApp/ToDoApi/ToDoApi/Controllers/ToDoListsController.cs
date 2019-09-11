using System;
using System.Collections.Generic;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoApi.Authorization;
using ToDoApi.DTOs;
using ToDoApi.Exceptions;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("/api/to-do-lists")]
    [ApiController]
    public class ToDoListsController : Controller
    {
        private readonly ToDoListsService _service;
        private readonly ILogger<ToDoListsController> _logger;

        public ToDoListsController(ToDoListsService service, ILogger<ToDoListsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET api/to-do-lists/3
        [HttpGet("{id}")]
        [Authorize("read:to-do-list")]
        [ProducesResponseType(typeof(ToDoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ToDoListDTO> GetToDoList(Guid id)
        {
            _logger.LogInformation("ToDoList.GetToDoList() executed!");

            ToDoList result = _service.GetToDoList(id, User.GetEmail());

            if (result == null)
                return NotFound();

            return Ok(new ToDoListDTO(result));
        }

        // GET api/to-do-lists
        [HttpGet]
        [Authorize("read:to-do-list")]
        [ProducesResponseType(typeof(List<ToDoListDTO>), StatusCodes.Status200OK)]
        public ActionResult<List<ToDoListDTO>> GetToDoLists()
        {
            _logger.LogInformation("ToDoList.GetToDoLists() executed!");

            return Ok(_service.GetToDoLists(User.GetEmail()));
        }

        // DELETE api/to-do-lists/3
        [HttpDelete("{id}")]
        [Authorize("delete:to-do-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteToDoList(Guid id)
        {
            _logger.LogInformation("ToDoList.DeleteToDoList() executed!");

            try
            {
                _service.DeleteToDoList(id, User.GetEmail());
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // GET api/to-do-lists/search/list1
        [HttpGet("search")]
        [Authorize("read:to-do-list")]
        [ProducesResponseType(typeof(List<ToDoListDTO>), StatusCodes.Status200OK)]
        public ActionResult<List<ToDoListDTO>> SearchToDoLists([FromQuery] string title)
        {
            _logger.LogInformation("ToDoList.SearchToDoLists() executed!");

            return Ok(_service.SearchToDoLists(title, User.GetEmail()));
        }

        // POST api/to-do-lists
        [HttpPost]
        [Authorize("write:to-do-list")]
        [ProducesResponseType(typeof(ToDoListDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CreateToDoList([FromBody] ToDoListDTO list)
        {
            _logger.LogInformation("ToDoList.CreateToDoList() executed!");

            if (!ModelState.IsValid)
                return BadRequest();

            return CreatedAtAction(nameof(GetToDoList), new { id = list.Id }, new ToDoListDTO(_service.CreateToDoList(list, User.GetEmail())));
        }

        // PUT api/to-do-lists
        [HttpPut]
        [Authorize("write:to-do-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult UpdateToDoList([FromBody] ToDoListDTO list)
        {
            _logger.LogInformation("ToDoList.UpdateToDoList() executed!");

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                _service.UpdateToDoList(list, User.GetEmail());
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // GET api/to-do-lists/list1/to-do-items/item1
        [HttpGet("{listId}/to-do-items/{itemId}")]
        [Authorize("read:to-do-item")]
        [ProducesResponseType(typeof(ToDoItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ToDoItemDTO> GetToDoItem(Guid listId, Guid itemId)
        {
            _logger.LogInformation("ToDoList.GetToDoItem() executed!");

            try
            {
                return Ok(_service.GetToDoItem(listId, itemId, User.GetEmail()));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // GET api/to-do-lists/list1/to-do-items
        [HttpGet("{listId}/to-do-items")]
        [Authorize("read:to-do-item")]
        [ProducesResponseType(typeof(List<ToDoItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<ToDoItemDTO>> GetToDoItems(Guid listId)
        {
            _logger.LogInformation("ToDoList.GetToDoItems() executed!");

            try
            {
                return Ok(_service.GetToDoItems(listId, User.GetEmail()));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // DELETE api/to-do-lists/list1/to-do-items/item1
        [HttpDelete("{listId}/to-do-items/{itemId}")]
        [Authorize("delete:to-do-item")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteToDoItem(Guid listId, Guid itemId)
        {
            _logger.LogInformation("ToDoList.DeleteToDoItems() executed!");

            try
            {
                _service.DeleteToDoItem(listId, itemId, User.GetEmail());
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // POST api/to-do-lists/list1/to-do-items
        [HttpPost("{listId}/to-do-items")]
        [Authorize("write:to-do-item")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ToDoItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult CreateToDoItem([FromRoute]Guid listId, [FromBody] ToDoItemDTO item)
        {
            _logger.LogInformation("ToDoList.CreateToDoItem() executed!");

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                ToDoItem createdItem = _service.CreateToDoItem(listId, item, User.GetEmail());
                return CreatedAtAction(nameof(GetToDoItem), new { listId, itemId = createdItem.Id }, new ToDoItemDTO(createdItem));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // PUT api/to-do-lists/list1/to-do-items
        [HttpPut("{listId}/to-do-items")]
        [Authorize("write:to-do-item")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult UpdateToDoItem([FromRoute]Guid listId, [FromBody] ToDoItemDTO item)
        {
            _logger.LogInformation("ToDoList.UpdateToDoItem() executed!");

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                _service.UpdateToDoItem(listId, item, User.GetEmail());
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // PUT api/to-do-lists/3/update-position/
        [HttpPut("{listId}/position/{position}")]
        [Authorize("write:to-do-list")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<ToDoListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult UpdateToDoListPosition([FromRoute]Guid listId, [FromRoute]int position)
        {
            _logger.LogInformation("ToDoList.UpdateToDoListPosition() executed!");

            try
            {
                return Ok(_service.UpdateToDoListPosition(listId, position, User.GetEmail()));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidPositionException)
            {
                return BadRequest();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // PUT api/to-do-lists/list1/to-do-items/update-position
        [HttpPut("{listId}/to-do-items/{itemId}/position/{position}")]
        [Authorize("write:to-do-item")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<ToDoItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult UpdateToDoItemPosition([FromRoute]Guid listId, [FromRoute] Guid itemId, [FromRoute] int position)
        {
            _logger.LogInformation("ToDoList.UpdateToDoItemPosition() executed!");

            try
            {
                return Ok(_service.UpdateToDoItemPosition(listId, itemId, position, User.GetEmail()));
            }
            catch (InvalidPositionException)
            {
                return BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // POST api/to-do-lists/list1/to-do-list-shares
        [HttpPost("{listId}/to-do-list-shares")]
        [Authorize("write:to-do-list-share")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult CreateToDoListShare([FromRoute]Guid listId)
        {
            _logger.LogInformation("ToDoList.CreateToDoListShare() executed!");

            try
            {
                ToDoListShare createdShare = _service.CreateToDoListShare(listId, User.GetEmail());
                return CreatedAtAction(nameof(GetToDoListShare), new { shareId = createdShare.Id }, createdShare.Id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        // GET api/to-do-lists/to-do-list-shares/share1
        [HttpGet("to-do-list-shares/{shareId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ToDoListDTO), StatusCodes.Status200OK)]
        public ActionResult GetToDoListShare([FromRoute]Guid shareId)
        {
            _logger.LogInformation("ToDoList.GetToDoListShare() executed!");

            try
            {
                return Ok(_service.GetToDoListShare(shareId));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}