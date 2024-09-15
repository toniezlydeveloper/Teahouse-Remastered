using Currency;
using Internal.Dependencies.Core;
using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class CustomerEmptyCompletedOrderModifier : IItemModifier
    {
        private ICurrencyHolder _currencyHolder = DependencyInjector.Get<ICurrencyHolder>();
        
        public ModifierType Type => ModifierType.Customer;

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
    
    public class CustomerEmptyOrderModifier : IItemModifier
    {
        private ICurrencyHolder _currencyHolder = DependencyInjector.Get<ICurrencyHolder>();
        
        public ModifierType Type => ModifierType.Customer;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.IsEmpty())
                return false;

            if (!place.TryGet(out Order order))
                return false;

            return !order.WasTaken;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Order>().WasTaken = true;
            place.Refresh();
        }
    }

    public class CustomerCupOrderModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Customer;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Cup cup))
                return false;

            if (!place.TryGet(out Order order))
                return false;

            if (!order.WasTaken)
                return false;
            
            if (!cup.HasWater)
                return false;
            
            return cup.WaterTemperature >= order.MinWaterTemperature && cup.WaterTemperature <= order.MaxWaterTemperature;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Order>().WasCompleted = true;
            player.Refresh(null);
            place.Refresh();
        }
    }
}