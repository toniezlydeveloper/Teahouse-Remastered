using Internal.Flow.UI;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public class MainMenuPanel : AUIPanel
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(RequestTransition<CharacterBoostrapState>);
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }
    }
}