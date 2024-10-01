using System;
using System.Collections.Generic;
using System.Linq;
using Currency;
using Internal.Dependencies.Core;
using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class SeatEmptyCompletedOrderModifier : IItemModifier
    {
        private ICurrencyHolder _currencyHolder = DependencyInjector.Get<ICurrencyHolder>();
        
        public ModifierType Type => ModifierType.Seat;

        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.IsEmpty())
                return false;

            if (!place.TryGet(out Order order))
                return false;

            return order.WasCompleted;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.TryGet(out Order order);
            _currencyHolder.Add(order);
            order.WasCollected = true;
            
            player.Refresh(new Cup
            {
                IsDirty = true
            });
            place.Refresh();
        }
    }
    
    public class SeatEmptyOrderModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Seat;

        private static readonly List<Type> IngredientTypes = new()
        {
            typeof(WaterType),
            typeof(HerbType),
            typeof(FlowerType),
            typeof(TeabagType)
        };
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.TryGet(out Order order))
                return false;

            return !order.WasTaken;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Order>().WasTaken = true;
            
            foreach (Type ingredientType in IngredientTypes)
            {
                place.CastTo<Order>().HeldAddIns.Add(Enum.GetValues(ingredientType).OfType<Enum>().Skip(1).OrderBy(_ => Guid.NewGuid()).FirstOrDefault());
            }
            
            place.Refresh();
        }
    }

    public class SeatCupOrderModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Seat;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Cup cup))
                return false;

            if (cup.IsDirty)
                return false;

            if (!place.TryGet(out Order order))
                return false;

            if (!order.WasTaken)
                return false;

            if (order.WasCompleted)
                return false;
            
            List<Enum> orderNotInCup = order.HeldAddIns.Except(cup.HeldAddIns).ToList();
            List<Enum> cupNotInOrder = cup.HeldAddIns.Except(order.HeldAddIns).ToList();
            return !orderNotInCup.Any() && !cupNotInOrder.Any();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Order>().WasCompleted = true;
            player.Refresh(null);
            place.Refresh();
        }
    }
}