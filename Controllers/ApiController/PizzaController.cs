using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.MyDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult Get()
        {
            List<PizzaModel> pizzaList = new List<PizzaModel>();
            //List<PizzaModel> pizzaList = new List<PizzaModel>();
            return Ok(pizzaList);
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            List<PizzaModel> pizzaList;
            using (Pizzeria ctx = new Pizzeria())
            {
                pizzaList = ctx.Pizzas.ToList();
            }
            foreach(PizzaModel pizza in pizzaList)
            {
                using (Pizzeria ctx = new Pizzeria())
                {
                    pizza.Category = ctx.Categories.Where(x => x.Id == pizza.CategoryId).FirstOrDefault();
                }
                using (Pizzeria ctx = new Pizzeria())
                {
                    pizza.Ingredients = ctx.Ingredients.FromSqlRaw($"SELECT i.Id, i.Name FROM Ingredients as i INNER JOIN IngredientModelPizzaModel as ingp ON i.Id = ingp.IngredientsId INNER JOIN Pizzas as p ON ingp.PizzasId = p.Id WHERE p.Id = {pizza.Id}").ToList();
                }
            }
            return Ok(pizzaList);
        }
    }
}
