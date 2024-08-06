using Interaction;
using Player;

namespace Inventory
{
    public class ItemShopHandler : AInteractionHandler
    {
        public override PlayerMode HandledModes => PlayerMode.Organization;

        public override void HandleInteractionInput(InteractionElement element)
        {
            element.GetComponent<ItemShop>().Show();
        }
    }
}