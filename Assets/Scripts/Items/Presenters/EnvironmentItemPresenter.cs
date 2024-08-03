using Items.Holders;

namespace Items.Presenters
{
    public class EnvironmentItemPresenter : AItemPresenter
    {
        private void OnEnable()
        {
            if (!TryGetComponent(out IItemHolder itemHolder))
                return;
            
            AddCallback(itemHolder);
        }

        private void OnDisable()
        {
            if (!TryGetComponent(out IItemHolder itemHolder))
                return;
            
            RemoveCallback(itemHolder);
        }
    }
}