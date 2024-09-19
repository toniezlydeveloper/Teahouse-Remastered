using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public interface ICharacterPanel : IDependency
    {
        void Init(CharacterData data);
    }

    public class CharacterData
    {
        public Action PreviousSpeciesCallback { get; set; }
        public Action NextSpeciesCallback { get; set; }
        public Action PreviousOutfitCallback { get; set; }
        public Action NextOutfitCallback { get; set; }
        public Action PreviousColorCallback { get; set; }
        public Action NextColorCallback { get; set; }
    }
    
    public class CharacterPanel : AUIPanel, ICharacterPanel
    {
        [SerializeField] private Button previousSpeciesButton;
        [SerializeField] private Button nextSpeciesButton;
        [SerializeField] private Button previousOutfitButton;
        [SerializeField] private Button nextOutfitButton;
        [SerializeField] private Button previousColorButton;
        [SerializeField] private Button nextColorButton;
        [SerializeField] private Button continueButton;

        private Action _previousSpeciesCallback;
        private Action _nextSpeciesCallback;
        private Action _previousOutfitCallback;
        private Action _nextOutfitCallback;
        private Action _previousColorCallback;
        private Action _nextColorCallback;

        private void Start()
        {
            previousSpeciesButton.onClick.AddListener(RaisePreviousSpeciesCallback);
            nextSpeciesButton.onClick.AddListener(RaiseNextSpeciesCallback);
            previousOutfitButton.onClick.AddListener(RaisePreviousOutfitCallback);
            nextOutfitButton.onClick.AddListener(RaiseNextOutfitCallback);
            previousColorButton.onClick.AddListener(RaisePreviousColorCallback);
            nextColorButton.onClick.AddListener(RaiseNextColorCallback);
            continueButton.onClick.AddListener(RequestTransition<ShopBootstrapState>);
        }

        public void Init(CharacterData data)
        {
            _previousSpeciesCallback = data.PreviousSpeciesCallback;
            _nextSpeciesCallback = data.NextSpeciesCallback;
            _previousOutfitCallback = data.PreviousOutfitCallback;
            _nextOutfitCallback = data.NextOutfitCallback;
            _previousColorCallback = data.PreviousColorCallback;
            _nextColorCallback = data.NextColorCallback;
        }

        private void RaisePreviousSpeciesCallback() => _previousSpeciesCallback?.Invoke();

        private void RaiseNextSpeciesCallback() => _nextSpeciesCallback?.Invoke();

        private void RaisePreviousOutfitCallback() => _previousOutfitCallback?.Invoke();

        private void RaiseNextOutfitCallback() => _nextOutfitCallback?.Invoke();

        private void RaisePreviousColorCallback() => _previousColorCallback?.Invoke();

        private void RaiseNextColorCallback() => _nextColorCallback?.Invoke();
    }
}