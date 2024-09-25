using Internal.Flow.UI;
using Saving;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public class MainMenuPanel : AUIPanel
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            continueButton.gameObject.SetActive(SavingController.HasFile(PersistenceType.Persistent, FileSaveType.Character));
            newGameButton.onClick.AddListener(StartNewGame);
            continueButton.onClick.AddListener(ContinueGame);
            quitButton.onClick.AddListener(Quit);
        }

        private void StartNewGame()
        {
            SavingController.ClearPersistent();
            SavingController.ClearVolatile();
            RequestTransition<CharacterBoostrapState>();
        }

        private void ContinueGame() => RequestTransition<BedroomDayBoostrapState>();

        private void Quit() => RequestTransition<QuitState>();
    }
}