using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
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

        private Action _previousSpeciesCallback;
        private Action _nextSpeciesCallback;
        private Action _previousOutfitCallback;
        private Action _nextOutfitCallback;
        private Action _previousColorCallback;
        private Action _nextColorCallback;

        private void Start()
        {
            previousSpeciesButton.onClick.AddListener(() => _previousSpeciesCallback?.Invoke());
            nextSpeciesButton.onClick.AddListener(() => _nextSpeciesCallback?.Invoke());
            previousOutfitButton.onClick.AddListener(() => _previousOutfitCallback?.Invoke());
            nextOutfitButton.onClick.AddListener(() => _nextOutfitCallback?.Invoke());
            previousColorButton.onClick.AddListener(() => _previousColorCallback?.Invoke());
            nextColorButton.onClick.AddListener(() => _nextColorCallback?.Invoke());
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
    }
}