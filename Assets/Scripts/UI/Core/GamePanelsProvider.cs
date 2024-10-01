using Internal.Flow.UI;
using States;

namespace UI.Core
{
    public class GamePanelsProvider : AUIPanelsProvider
    {
        protected override void AddTranslations()
        {
            AddTranslation<ShopClosedAtDayState, ShopClosedAtDayPanel>();
            AddTranslation<BedroomNightState, BedroomNightPanel>();
            AddTranslation<BedroomDayState, BedroomDayPanel>();
            AddTranslation<ShopOpenedState, ShopOpenedPanel>();
            AddTranslation<CharacterState, CharacterPanel>();
            AddTranslation<TutorialState, TutorialPanel>();
            AddTranslation<MainMenuState, MainMenuPanel>();
            AddTranslation<CallingState, CallingPanel>();
        }
    }
}