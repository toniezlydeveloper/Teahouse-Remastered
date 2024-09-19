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
        public Action<Color> ColorCallback { get; set; }
    }
    
    public class CharacterPanel : AUIPanel, ICharacterPanel
    {
        [SerializeField] private Button previousSpeciesButton;
        [SerializeField] private Button nextSpeciesButton;
        [SerializeField] private Button previousOutfitButton;
        [SerializeField] private Button nextOutfitButton;

        private Action _previousSpeciesCallback;
        private Action _nextSpeciesCallback;
        private Action _previousOutfitCallback;
        private Action _nextOutfitCallback;
        private Action<Color> _colorCallback;

        private void Start()
        {
            previousSpeciesButton.onClick.AddListener(() => _previousSpeciesCallback?.Invoke());
            nextSpeciesButton.onClick.AddListener(() => _nextSpeciesCallback?.Invoke());
            previousOutfitButton.onClick.AddListener(() => _previousOutfitCallback?.Invoke());
            nextOutfitButton.onClick.AddListener(() => _nextOutfitCallback?.Invoke());
        }

        public void Init(CharacterData data)
        {
            _previousSpeciesCallback = data.PreviousSpeciesCallback;
            _nextSpeciesCallback = data.NextSpeciesCallback;
            _previousOutfitCallback = data.PreviousOutfitCallback;
            _nextOutfitCallback = data.NextOutfitCallback;
            _colorCallback = data.ColorCallback;
        }

        // This method is invoked by outside UnityEvent callback.
        public void ChangeColor(Color color) => _colorCallback?.Invoke(color);
    }
}