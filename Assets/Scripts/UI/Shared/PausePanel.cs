using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared
{
    public interface IPausePanel : IDependency
    {
        void Present(PauseData data);
    }
    
    public class PauseData
    {
        public Action ResumeCallback { get; set; }
        public Action SaveCallback { get; set; }
        public bool AllowSaving { get; set; }
        public bool IsPaused { get; set; }
    }
    
    public class PausePanel : AUIPanel, IPausePanel
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        private Action _resumeCallback;
        private Action _saveCallback;
        
        private void Start()
        {
            resumeButton.onClick.AddListener(RaiseResumeCallback);
            saveButton.onClick.AddListener(RaiseSaveCallback);
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            quitButton.onClick.AddListener(Quit);
        }

        public void Present(PauseData data)
        {
            CacheCallbacks(data);
            Toggle(data);

            if (ShouldEnable(data))
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        private void GoToMainMenu()
        {
            RequestTransition<MainMenuBootstrapState>();
            RaiseResumeCallback();
        }

        private void Quit()
        {
            RequestTransition<QuitState>();
            RaiseResumeCallback();
        }

        private void RaiseResumeCallback() => _resumeCallback?.Invoke();

        private void RaiseSaveCallback() => _saveCallback?.Invoke();

        private void Toggle(PauseData data) => saveButton.gameObject.SetActive(data.AllowSaving);

        private void CacheCallbacks(PauseData data)
        {
            _resumeCallback = data.ResumeCallback;
            _saveCallback = data.SaveCallback;
        }

        private bool ShouldEnable(PauseData data) => data.IsPaused;
    }
}