using Internal.Variables.Core;
using Items.Implementations;
using UnityEngine;

namespace Items.Holders
{
    [CreateAssetMenu(menuName = "Variables/Hand")]
    public class CachedItemHolder : ACachedVariable<IItem>, IItemHolder, IManageableItemHolder
    {
        public string Name => "Hand";
        
        public void SetInitial()
        {
        }

        public void Destroy() => Value = null;
    }
}