using System;
using System.Linq;
using Internal.Flow.States;
using Saving;
using UI.Core;
using UnityEngine;
using Utilities;

namespace States
{
    public class MainMenuState : AState
    {
        private IMainMenuPanel _mainMenuPanel;
        private TextAsset[] _initialSaves;

        public MainMenuState(IMainMenuPanel mainMenuPanel, TextAsset[] initialSaves)
        {
            _mainMenuPanel = mainMenuPanel;
            _initialSaves = initialSaves;
        }

        public override void OnEnter() => InitPanel();

        protected override void AddConditions() => AddCondition<BedroomNightBoostrapState>(() => DevelopmentConfig.Instance.ShouldSkipMainMenu);
        
        private void Init()
        {
            MoveInitialFiles();
            SavingController.Override(PersistenceType.Persistent, PersistenceType.Initial);
            SavingController.Override(PersistenceType.Volatile, PersistenceType.Initial);
        }

        private void MoveInitialFiles()
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                TextAsset saveFile = _initialSaves.FirstOrDefault(asset => asset.name.Contains(saveType.ToString()));

                if (saveFile == null)
                {
                    continue;
                }

                SavingController.Save(PersistenceType.Initial, saveType, saveFile.text);
            }
        }

        private void InitPanel() => _mainMenuPanel.Present(new MainMenuData
        {
            CanContinueCallback = () => SavingController.HasFile(PersistenceType.Persistent, FileSaveType.Character),
            InitGameCallback = Init
        });
    }
}