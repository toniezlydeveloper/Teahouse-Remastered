using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public interface IMainMenuPanel : IDependency
    {
        void Present(MainMenuData data);
    }

    public class MainMenuData
    {
        public Func<bool> CanContinueCallback { get; set; }
        public Action InitGameCallback { get; set; }
    }
    
    public class MainMenuPanel : AUIPanel, IMainMenuPanel
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;

        private Func<bool> _canContinueCallback;
        private Action _initGameCallback;
        
        private void Start()
        {
            newGameButton.onClick.AddListener(StartNewGame);
            continueButton.onClick.AddListener(ContinueGame);
            quitButton.onClick.AddListener(Quit);
        }
        public void Present(MainMenuData data)
        {
            Cache(data);
            ToggleContinuation();
        }

        private void StartNewGame()
        {
            RequestTransition<CharacterBoostrapState>();
            InitGame();
        }

        private void ContinueGame() => RequestTransition<BedroomDayBoostrapState>();

        private void Quit() => RequestTransition<QuitState>();

        private void Cache(MainMenuData data)
        {
            _canContinueCallback = data.CanContinueCallback;
            _initGameCallback = data.InitGameCallback;
        }

        private void InitGame() => _initGameCallback?.Invoke();
        
        private void ToggleContinuation() => continueButton.gameObject.SetActive(_canContinueCallback.Invoke());
    }
}