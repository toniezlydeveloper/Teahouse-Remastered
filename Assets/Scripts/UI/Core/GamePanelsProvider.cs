using Internal.Flow.UI;
using States;

namespace UI.Core
{
    public class GamePanelsProvider : AUIPanelsProvider
    {
        protected override void AddTranslations()
        {
            AddTranslation<ShopOpenedAtDayState, ShopOpenedPanel>();
            AddTranslation<ShopClosedAtDayState, ShopClosedPanel>();
            AddTranslation<CharacterState, CharacterPanel>();
            AddTranslation<TutorialState, TutorialPanel>();
            AddTranslation<ItemShopState, ItemShopPanel>();
            AddTranslation<MainMenuState, MainMenuPanel>();
            AddTranslation<BedroomState, BedroomPanel>();
        }
    }
}