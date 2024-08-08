using Internal.Dependencies.Core;
using Items.Implementations;
using UI.Shared;
using UnityEngine;

namespace Currency
{
    public interface ICurrencyHolder : IDependency
    {
        bool TrySpend(int amount);
        void Add(Order order);
    }
    
    public class CurrencyHolder : MonoBehaviour, ICurrencyHolder
    {
        [SerializeField] private int initialAmount;
        
        private ICurrencyPanel _currencyPanel;
        private int _amount;

        private void Start() => Invoke(nameof(Init), 0.5f);

        public bool TrySpend(int amount)
        {
            if (!TryRemove(amount))
                return false;
            
            Present();
            return true;
        }

        public void Add(Order order)
        {
            ChangeAmount(order);
            Present();
        }

        private void Init()
        {
            SetInitialAmount();
            Present();
        }

        private void SetInitialAmount() => _amount = initialAmount;

        private bool TryRemove(int amount)
        {
            if (_amount < amount)
                return false;

            _amount -= amount;
            return true;
        }

        private void ChangeAmount(Order order) => _amount += 100;

        private void Present()
        {
            _currencyPanel ??= DependencyInjector.Get<ICurrencyPanel>();
            _currencyPanel.Present(_amount);
        }
    }
}