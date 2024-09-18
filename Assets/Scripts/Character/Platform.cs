using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Dependencies.Core;
using UI.Core;
using UnityEngine;

namespace Character
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private CharactersConfig config;
        [SerializeField] private Transform spawnPoint;

        private ICharacterPanel _characterPanel = DependencyInjector.Get<ICharacterPanel>();
        private GameObject _characterModel;
        private GameObject _outfitModel;
        private Species _species;
        private Outfit _outfit;

        private void Awake() => _characterPanel.Init(new CharacterData
        {
            PreviousSpeciesCallback = SelectPreviousSpecies,
            NextSpeciesCallback = SelectNextSpecies,
            PreviousOutfitCallback = SelectPreviousOutfit,
            NextOutfitCallback = SelectNextOutfit,
            ColorCallback = SelectColor,
        });

        private void Start()
        {
            SelectSpecies(config.DefaultSpecies);
            SelectOutfit(config.DefaultOutfit);
            SelectColor(config.DefaultColor);
        }

        private void SelectPreviousSpecies() => SelectSpecies(SelectPrevious(_species));

        private void SelectNextSpecies() => SelectSpecies(SelectNext(_species));

        private void SelectPreviousOutfit() => SelectOutfit(SelectPrevious(_outfit));

        private void SelectNextOutfit() => SelectOutfit(SelectNext(_outfit));

        private void SelectColor(Color color)
        {
            foreach (Renderer characterRenderer in _characterModel.GetComponentsInChildren<Renderer>())
            {
                characterRenderer.material.color = color;
            }
        }

        private void SelectSpecies(Species species)
        {
            SpeciesModel model = config.SpeciesModels.First(model => model.Type == species);

            if (_characterModel != null)
            {
                Destroy(_characterModel);
            }

            Instantiate(model.Prefab, spawnPoint);
            
            _species = species;
        }

        private void SelectOutfit(Outfit outfit)
        {
            OutfitModel model = config.OutfitModels.First(model => model.Type == outfit);

            if (_outfitModel != null)
            {
                Destroy(_outfitModel);
            }

            Instantiate(model.Prefab, spawnPoint);
            
            _outfit = outfit;
        }

        private TType SelectPrevious<TType>(TType type) where TType : Enum
        {
            List<TType> values = ((TType[])Enum.GetValues(typeof(TType))).ToList();
            int index = (values.IndexOf(type) + values.Count - 1) % values.Count;
            return values[index];
        }

        private TType SelectNext<TType>(TType type) where TType : Enum
        {
            List<TType> values = ((TType[])Enum.GetValues(typeof(TType))).ToList();
            int index = (values.IndexOf(type) + values.Count + 1) % values.Count;
            return values[index];
        }
    }
}