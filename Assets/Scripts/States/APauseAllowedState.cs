using System.Collections.Generic;
using Internal.Flow.States;
using Saving;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    public abstract class APauseAllowedState : AState
    {
        private InputActionReference _pauseInput;
        private IPausePanel _pausePanel;
        private bool _isPaused;
        
        protected abstract List<FileSaveType> TypesToSave { get; }

        protected APauseAllowedState(InputActionReference pauseInput, IPausePanel pausePanel)
        {
            _pauseInput = pauseInput;
            _pausePanel = pausePanel;
        }

        protected void HandlePause()
        {
            if (!ReceivedPauseInput())
            {
                return;
            }

            TogglePause();
        }

        private void TogglePause()
        {
            ToggleState();
            PresentPause();
            ToggleTime();
        }

        private void SaveGame()
        {
            foreach (FileSaveType type in TypesToSave)
            {
                SavingController.Save(PersistenceType.Volatile, type);
            }

            SavingController.Override(PersistenceType.Persistent, PersistenceType.Volatile);
        }

        private bool ReceivedPauseInput() => _pauseInput.action.triggered;

        private void ToggleState() => _isPaused = !_isPaused;

        private void PresentPause() => _pausePanel.Present(new PauseData
        {
            ResumeCallback = TogglePause,
            SaveCallback = SaveGame,
            AllowSaving = HasSomethingToSave(),
            IsPaused = GetState()
        });

        private void ToggleTime() => Time.timeScale = _isPaused ? 0f : 1f;

        private bool HasSomethingToSave() => TypesToSave.Count > 0;

        private bool GetState() => _isPaused;
    }
}