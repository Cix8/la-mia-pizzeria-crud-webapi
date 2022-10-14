﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using la_mia_pizzeria_static.MyValidations;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il nome non può avere più di 30 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [Column(TypeName = "text")]
        [DescriptionValidation]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [Column(TypeName = "text")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [PriceValidation]
        public float Price { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
        public PizzaModel()
        {
            Ingredients = new List<IngredientModel>();
        }

        public PizzaModel(string name, string description, string image, float price)
        {
            Name = name;
            Description = description;
            Image = image;
            Price = price;
        }

        public string IngToString()
        {
            string result = "";
            for(int i = 0; i < Ingredients.Count; i++)
            {
                if(i != (Ingredients.Count - 1))
                {
                    result += $"{Ingredients[i].Name}, ";
                } else
                {
                    result += $"{Ingredients[i].Name}";
                }
            }
            return result;
        }
    }
}
