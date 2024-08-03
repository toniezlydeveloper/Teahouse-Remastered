using Items.Holders;
using UnityEngine;

namespace Items.Presenters
{
    public class PlayerItemPresenter : AItemPresenter
    {
        [SerializeField] private CachedItemHolder itemHolder;

        private void OnEnable() => AddCallback(itemHolder);

        private void OnDisable() => RemoveCallback(itemHolder);
    }
}