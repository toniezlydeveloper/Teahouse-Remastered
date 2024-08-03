using Internal.Variables.Core;
using UnityEngine;

namespace Items.Holders
{
    [CreateAssetMenu(menuName = "Variables/Hand")]
    public class CachedItemHolder : ACachedVariable<object>, IItemHolder, IManageableItemHolder
    {
        public void SetInitial()
        {
        }

        public void Destroy() => Value = null;
    }
}