﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Recipe")]
    public class Recipe : IRecipe
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public StringList Instructions { get; set; }
        [DataMember]
        public int Servings { get; set; }

        public int Value { get; private set; }

        private ValueInformation _ownValue = new ValueInformation();
        public ValueInformation OwnValue => _ownValue ?? (_ownValue = new ValueInformation()); //Needed for deserialization

        [DataMember]
        public List<Ingredient> Ingredients { get; set; }
        [DataMember]
        public List<RecipeCategory> Categories { get; set; }
        [DataMember]
        public List<GroupedRecipeCategory> GroupedCategories { get; set; }

        public string CategoriesAsString => Categories.IsNullOrEmpty() ? string.Empty : string.Join(", ", Categories.ConvertAll(x => x.Name));

        public string GroupedCategoriesAsString
        {
            get
            {
                if (GroupedCategories == null || GroupedCategories.IsEmpty()) return String.Empty;
                List<List<Boolable<RecipeCategory>>> grcs = GroupedCategories.ConvertAll(x => x.GroupedRc);
                var grcNames = new List<String>();
                foreach (List<Boolable<RecipeCategory>> groupedRc in grcs)
                {
                    foreach (Boolable<RecipeCategory> boolableRc in groupedRc)
                    {
                        if (boolableRc.Bool) grcNames.Add(boolableRc.Instance.Name);
                    }
                }
                return string.Join(", ", grcNames);
            }
        }

        public ProgressInfo BpCompositionForAllIngredients
        {
            get
            {
                int amounts = 0, amountNeeded = 0;
                foreach (Ingredient i in Ingredients)
                {
                    amounts += i.AmountSatisfied.ToGrams;
                    amountNeeded += i.AmountNeeded.ToGrams;
                }
                return new ProgressInfo(amounts, amountNeeded, string.Empty);
            }
        }

        private Recipe() { }

        public Recipe(string name, int servings = 1, string description = null, StringList instruction = null, List<Ingredient> ingredients = null, List<RecipeCategory> categories = null, List<GroupedRecipeCategory> groupedCategories = null)
        {
            this.Name = name;
            this.Description = description ?? String.Empty;
            this.Instructions = instruction ?? new StringList();
            this.Servings = servings;
            this.Ingredients = ingredients ?? new List<Ingredient>();
            this.Categories = categories ?? new List<RecipeCategory>();
            this.GroupedCategories = groupedCategories ?? new List<GroupedRecipeCategory>();
            this.Value = 0;

            // Lots of exceptins here if something is wrong. Check also if selections are correct in groupedcategories.
        }

        public void AggregateValue()    
        {
            // This is not combined with value property because I don't want to recalculate until I click "sort".
            int val = OwnValue.GetValue;
            foreach (RecipeCategory recipeCategory in Categories)
            {
                val += recipeCategory.OwnValue.GetValue;
            }

            Dictionary<BoughtProduct, uint> bpAmountsRemaining = CreateDictForBpAmounts();
            // This dictionary will be used to keep track of how much and which boughtProducts our ingredients use 

            foreach (Ingredient ingredient in Ingredients)
            {
                ingredient.AggregateBpValues(bpAmountsRemaining);
                val += ingredient.Value;
            }
            this.Value = val;
        }

        private Dictionary<BoughtProduct, uint> CreateDictForBpAmounts()
        {
            Dictionary<BoughtProduct, uint> bpAmountsRemaining = new Dictionary<BoughtProduct, uint>();
            foreach (Ingredient ingredient in Ingredients.Distinct())
            {
                ingredient.OwnValueCalculator.OrderedBpValues.ToList().ConvertAll(x => x.Bp).ForEach(y => bpAmountsRemaining.Add(y, y.Amount));
            }
            return bpAmountsRemaining;
        }

        public void Reset()
        {
            OwnValue.Reset();
            Ingredients.ForEach(x => x.Reset());
        }
    }
}
