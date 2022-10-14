﻿using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.MyDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize]
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;
        private Pizzeria _pizzeria_db;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
            _pizzeria_db = new Pizzeria();
        }

        private void PizzaSeeder()
        {
            PizzaModel newPizza = new PizzaModel("Margherita", "La classica pizza margherita napoletana", "pizza-margherita.jfif", 5.99F);
            _pizzeria_db.Add(newPizza);
            _pizzeria_db.SaveChanges();

            newPizza = new PizzaModel("Capricciosa", "La pizza capricciosa è una pizza tipica della cucina italiana caratterizzata da un condimento di pomodoro, mozzarella, prosciutto cotto, funghi, olive verdi e nere, e carciofini", "pizza-capricciosa.jfif", 7.99F);
            _pizzeria_db.Add(newPizza);
            _pizzeria_db.SaveChanges();

            newPizza = new PizzaModel("Salame Piccante", "Pizza base margherita con aggiunta di salame piccante", "pizza-salame.jfif", 6.99F);
            _pizzeria_db.Add(newPizza);
            _pizzeria_db.SaveChanges();
        }

        private void Store(PizzaModel pizza)
        {
            _pizzeria_db.Pizzas.Add(pizza);
            _pizzeria_db.SaveChanges();
        }

        public PizzaModel FindBy(int id)
        {
            return _pizzeria_db.Pizzas.Find(id);
        }

        public IActionResult Index()
        {
            List<PizzaModel> pizzaList = new List<PizzaModel>();
            pizzaList = _pizzeria_db.Pizzas.OrderBy(pizza => pizza.Id).ToList<PizzaModel>();
            //if (pizzaList.Count == 0)
            //{
            //    this.PizzaSeeder();
            //    pizzaList = _pizzeria_db.Pizzas.OrderBy(pizza => pizza.Id).ToList<PizzaModel>();
            //}
            return View(pizzaList);
        }

        public IActionResult Details(int id)
        {
            PizzaModel thisPizza = _pizzeria_db.Pizzas.Where(pizza => pizza.Id == id).Include("Category").Include("Ingredients").First();
            return View("Show", thisPizza);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PizzaCategories pizzaCategories = new PizzaCategories();
            List<CategoryModel> categories = _pizzeria_db.Categories.ToList();
            List<IngredientModel> ingredients = _pizzeria_db.Ingredients.ToList();
            pizzaCategories.Categories = categories;
            pizzaCategories.Ingredients = ingredients;
            return View(pizzaCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _pizzeria_db.Categories.ToList();
                model.Ingredients = _pizzeria_db.Ingredients.ToList();
                return View("Create", model);
            }
            model.Pizza.Ingredients = _pizzeria_db.Ingredients.Where(ing => model.selectedIng.Contains(ing.Id)).ToList();
            this.Store(model.Pizza);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            PizzaModel thisPizza = _pizzeria_db.Pizzas.Where(pizza => pizza.Id == id).Include("Ingredients").First();
            if (thisPizza != null)
            {
                PizzaCategories pizzaCategories = new PizzaCategories();
                pizzaCategories.Pizza = thisPizza;
                pizzaCategories.Categories = _pizzeria_db.Categories.ToList();
                pizzaCategories.Ingredients = _pizzeria_db.Ingredients.ToList();
                return View(pizzaCategories);
            }
            return NotFound("Non siamo riusciti a trovare la pizza selezionata...");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories model)
        {
            model.Pizza.Id = id;

            if (!ModelState.IsValid)
            {
                model.Categories = _pizzeria_db.Categories.ToList();
                model.Ingredients = _pizzeria_db.Ingredients.ToList();
                return View("Update", model);
            }

            //model.Pizza.Ingredients = _pizzeria_db.Ingredients.Where(ing => model.selectedIng.Contains(ing.Id)).ToList();
            //_pizzeria_db.Pizzas.Update(model.Pizza);

            PizzaModel pizza = _pizzeria_db.Pizzas.Where(pizza => pizza.Id == id).Include("Ingredients").First();
            pizza.Name = model.Pizza.Name;
            pizza.Description = model.Pizza.Description;
            pizza.Image = model.Pizza.Image;
            pizza.Price = model.Pizza.Price;
            pizza.CategoryId = model.Pizza.CategoryId;
            pizza.Ingredients = _pizzeria_db.Ingredients.Where(ing => model.selectedIng.Contains(ing.Id)).ToList();

            _pizzeria_db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            PizzaModel pizza = this.FindBy(id);

            if(pizza != null)
            {
                _pizzeria_db.Remove(pizza);
                _pizzeria_db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                return NotFound("Non siamo riusciti a trovare la pizza che intendi eliminare");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}