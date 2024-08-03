using System;
using System.Collections.Generic;
using Internal.Flow.States;
using UnityEngine;

namespace Internal.Flow.UI
{
    // ReSharper disable once InconsistentNaming
    public abstract class AUIPanelsProvider : MonoBehaviour
    {
        [field:SerializeField] public GameObject PanelsParent { get; set; }
        
        private Dictionary<Type, Type> _stateTypeByPanelTypes = new();
        
        public Dictionary<Type, AUIPanel> PanelsByStateType { get; } = new();

        private void Awake()
        {
            DontDestroyOnLoad();
            AddTranslations();
            GetPanels();
        }

        protected abstract void AddTranslations();
        
        protected void AddTranslation<TState, TPanel>() where TState : AState where TPanel : AUIPanel =>
            _stateTypeByPanelTypes.Add(typeof(TPanel), typeof(TState));

        private void DontDestroyOnLoad() => DontDestroyOnLoad(PanelsParent);
        
        private void GetPanels()
        {
            foreach (AUIPanel panel in PanelsParent.GetComponentsInChildren<AUIPanel>())
            {
                Type stateType = _stateTypeByPanelTypes[panel.GetType()];
                PanelsByStateType.Add(stateType, panel);
            }
        }
    }
}