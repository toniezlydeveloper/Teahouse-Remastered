using System;
using System.Collections.Generic;
using System.Linq;

namespace Items.Implementations
{
    public class Order : IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public bool WasCompleted { get; set; }
        public bool WasCollected { get; set; }
        public bool WasTaken { get; set; }

        public string Name => "Customer";

        private static readonly List<Type> IngredientTypes = new()
        {
            typeof(WaterType),
            typeof(HerbType),
            typeof(FlowerType),
            typeof(TeabagType)
        };

        public Order()
        {
            foreach (Type ingredientType in IngredientTypes)
            {
                HeldAddIns.Add(Enum.GetValues(ingredientType).OfType<Enum>().Skip(1).OrderBy(_ => Guid.NewGuid()).FirstOrDefault());
            }
        }
    }
}