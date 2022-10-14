using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.MyDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.ApiController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private Pizzeria _pizzeria_db;

        public PizzaController()
        {
            _pizzeria_db = new Pizzeria();
        }

        public IActionResult Get()
        {
            List<PizzaModel> pizzaList = _pizzeria_db.Pizzas.ToList();
            //List<PizzaModel> pizzaList = new List<PizzaModel>();
            return Ok(pizzaList);
        }
    }
}
