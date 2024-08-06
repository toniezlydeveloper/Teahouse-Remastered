using Internal.Dependencies.Core;
using Items.Implementations;
using UI.Shared;
using UnityEngine;

namespace Currency
{
    public interface ICurrencyHolder : IDependency
    {
        void Add(Order order);
    }
    
    public class CurrencyHolder : MonoBehaviour, ICurrencyHolder
    {
        private ICurrencyPanel _currencyPanel;
        private int _amount;
        
        public void Add(Order order)
        {
            ChangeAmount(order);
            Present();
        }

        private void ChangeAmount(Order order) => _amount += 100;

        private void Present()
        {
            _currencyPanel ??= DependencyInjector.Get<ICurrencyPanel>();
            _currencyPanel.Present(_amount);
        }
    }
}