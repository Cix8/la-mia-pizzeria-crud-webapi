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
        public IActionResult Get(string? keyWord)
        {
            List<PizzaModel> pizzaList = new List<PizzaModel>();
            if (keyWord != null)
            {
                pizzaList = _pizzeria_db.Pizzas.Where(pizza => pizza.Name.Contains(keyWord)).Include("Category").Include("Ingredients").ToList();
            } else
            {
                pizzaList = _pizzeria_db.Pizzas.Include("Category").Include("Ingredients").ToList();
            }
            //List<PizzaModel> pizzaList = new List<PizzaModel>();
            return Ok(pizzaList);
        }


        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            PizzaModel thisPizza = _pizzeria_db.Pizzas.Where(pizza => pizza.Id == id).Include("Category").Include("Ingredients").First();
            return Ok(thisPizza);
            //return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetEditForm(int id)
        {
            PizzaCategories formData = new PizzaCategories();
            formData.Pizza = _pizzeria_db.Pizzas.Where(p => p.Id == id).Include("Ingredients").First();
            formData.Categories = _pizzeria_db.Categories.ToList();
            formData.Ingredients = _pizzeria_db.Ingredients.ToList();

            return Ok(formData);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PizzaModel pizza)
        {
            PizzaModel searchResult = _pizzeria_db.Pizzas.Where(p => p.Id == id).Include("Ingredients").FirstOrDefault();
            if (searchResult == null)
            {
                return NotFound();
            }
            searchResult.Name = pizza.Name;
            searchResult.Description = pizza.Description;
            searchResult.Image = pizza.Image;
            searchResult.Price = pizza.Price;
            searchResult.CategoryId = pizza.CategoryId;
            searchResult.Ingredients = _pizzeria_db.Ingredients.Where(ing => pizza.Ingredients.Contains(ing)).ToList();
            _pizzeria_db.SaveChanges();
            return Ok();
        }
    }
}
