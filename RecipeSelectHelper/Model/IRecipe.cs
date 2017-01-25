﻿using System.Collections.Generic;

namespace RecipeSelectHelper.Model
{
    public interface IRecipe
    {
        string Name { get; set; }
        string Description { get; set; }
        string Instruction { get; set; }
        int ID { get; }
        int Value { get; set; }
        List<Ingredient> Ingredients { get; set; }
        List<RecipeCategory> Categories { get; set; }
        string CategoriesAsString { get; }
    }
}