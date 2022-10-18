using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.MyDbContext;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.ApiController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private Pizzeria _pizzeria_db;

        public MessageController()
        {
            _pizzeria_db = new Pizzeria();
        }

        [HttpPost]
        public IActionResult Send(MessageModel message)
        {
            _pizzeria_db.Messages.Add(message);
            _pizzeria_db.SaveChanges();
            return Ok();
        }
    }
}
