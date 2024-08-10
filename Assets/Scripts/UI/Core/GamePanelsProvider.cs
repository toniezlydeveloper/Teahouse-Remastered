using Internal.Flow.UI;
using States;

namespace UI.Core
{
    public class GamePanelsProvider : AUIPanelsProvider
    {
        protected override void AddTranslations()
        {
            AddTranslation<ShopOpenedAtDayState, ShopOpenedPanel>();
            AddTranslation<ShopClosedState, ShopClosedPanel>();
            AddTranslation<TradeItemsState, ItemShopPanel>();
            AddTranslation<BedroomState, BedroomPanel>();
            AddTranslation<GardenState, GardenPanel>();
        }
    }
}