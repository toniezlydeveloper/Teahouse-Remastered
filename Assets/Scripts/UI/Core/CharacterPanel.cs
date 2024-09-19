using System;
using Character;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using States;
using TMPro;
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
        public Func<Species> SelectedSpeciesCallback { get; set; }
        public Func<string> SelectedOutfitCallback { get; set; }
        public Func<Color> SelectedColorCallback { get; set; }
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
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI speciesTextContainer;
        [SerializeField] private TextMeshProUGUI outfitTextContainer;
        [SerializeField] private Image colorImage;

        private Func<Species> _selectedSpeciesCallback;
        private Func<string> _selectedOutfitCallback;
        private Func<Color> _selectedColorCallback;
        private Action _previousSpeciesCallback;
        private Action _nextSpeciesCallback;
        private Action _previousOutfitCallback;
        private Action _nextOutfitCallback;
        private Action _previousColorCallback;
        private Action _nextColorCallback;

        private const string SpeciesTextBase = "Species: {0}";
        private const string OutfitTextBase = "Outfit: {0}";

        private void Start()
        {
            previousSpeciesButton.onClick.AddListener(RaisePreviousSpeciesCallback);
            nextSpeciesButton.onClick.AddListener(RaiseNextSpeciesCallback);
            previousOutfitButton.onClick.AddListener(RaisePreviousOutfitCallback);
            nextOutfitButton.onClick.AddListener(RaiseNextOutfitCallback);
            previousColorButton.onClick.AddListener(RaisePreviousColorCallback);
            nextColorButton.onClick.AddListener(RaiseNextColorCallback);
            continueButton.onClick.AddListener(RequestTransition<ShopBootstrapState>);
            backButton.onClick.AddListener(RequestTransition<MainMenuBootstrapState>);
        }

        private void Update()
        {
            if (_selectedSpeciesCallback == null)
            {
                return;
            }
            
            speciesTextContainer.text = string.Format(SpeciesTextBase, _selectedSpeciesCallback.Invoke().ToString().Replace("3D_", ""));
            outfitTextContainer.text = string.Format(OutfitTextBase, _selectedOutfitCallback.Invoke().Replace("3D_", ""));
            colorImage.color = _selectedColorCallback.Invoke();
        }

        public void Init(CharacterData data)
        {
            _selectedSpeciesCallback = data.SelectedSpeciesCallback;
            _selectedOutfitCallback = data.SelectedOutfitCallback;
            _selectedColorCallback = data.SelectedColorCallback;
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