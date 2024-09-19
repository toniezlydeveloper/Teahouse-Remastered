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
        private CharacterModel _characterModel;
        private SpeciesModel _speciesModel;
        private List<Species> _allSpecies;
        private Species _species;
        private string _outfit;
        private Color _color;

        private void Awake()
        {
            _allSpecies = ((Species[])Enum.GetValues(typeof(Species))).ToList();
            
            _characterPanel.Init(new CharacterData
            {
                PreviousSpeciesCallback = SelectPreviousSpecies,
                NextSpeciesCallback = SelectNextSpecies,
                PreviousOutfitCallback = SelectPreviousOutfit,
                NextOutfitCallback = SelectNextOutfit,
                PreviousColorCallback = SelectPreviousColor,
                NextColorCallback = SelectNextColor
            });
        }

        private void Start()
        {
            SelectSpecies(config.DefaultSpecies);
            RefreshModel();
        }

        private void SelectPreviousSpecies()
        {
            SelectSpecies(SelectPrevious(_species));
            RefreshModel();
        }

        private void SelectNextSpecies()
        {
            SelectSpecies(SelectNext(_species));
            RefreshModel();
        }

        private void SelectPreviousOutfit() => SelectOutfit(SelectPrevious(_speciesModel.Outfits, ref _outfit));

        private void SelectNextOutfit() => SelectOutfit(SelectNext(_speciesModel.Outfits, ref _outfit));

        private void SelectPreviousColor() => SelectColor(SelectPrevious(_speciesModel.Colors, ref _color));

        private void SelectNextColor() => SelectColor(SelectNext(_speciesModel.Colors, ref _color));

        private void SelectSpecies(Species species)
        {
            if (IsShowingModelAlready())
            {
                Destroy(GetModelObject());
            }

            SelectModel(GetModel(species));
            Cache(species);
            RefreshModel();
        }

        private void RefreshModel()
        {
            SelectOutfit(_speciesModel.Outfits[_speciesModel.DefaultOutfitIndex]);
            SelectColor(_speciesModel.Colors[_speciesModel.DefaultColorIndex]);
        }

        private void SelectOutfit(string outfit)
        {
            ToggleModels(outfit);
            Cache(outfit);
        }

        private void SelectModel(SpeciesModel model)
        {
            Cache(Instantiate(model.Prefab, spawnPoint));
            Cache(model);
        }

        private void SelectColor(Color color)
        {
            ColorRenderers(color);
            Cache(color);
        }

        private GameObject GetModelObject() => _characterModel.gameObject;

        private bool IsShowingModelAlready() => _characterModel != null;

        private SpeciesModel GetModel(Species species) => config.SpeciesModels.First(model => model.Type == species);

        private void ColorRenderers(Color color) => _characterModel.ColorRenderers(color);

        private void ToggleModels(string outfit) => _characterModel.ToggleOutfits(outfit);

        private void Cache(CharacterModel characterModel) => _characterModel = characterModel;

        private void Cache(SpeciesModel speciesModel) => _speciesModel = speciesModel;
        
        private void Cache(Species species) => _species = species;

        private void Cache(string outfit) => _outfit = outfit;

        private void Cache(Color color) => _color = color;

        private TValue SelectPrevious<TValue>(List<TValue> values, ref TValue value)
        {
            int index = (values.IndexOf(value) + values.Count - 1) % values.Count;
            value = values[index];
            return value;
        }

        private TValue SelectNext<TValue>(List<TValue> values, ref TValue value)
        {
            int index = (values.IndexOf(value) + values.Count + 1) % values.Count;
            value = values[index];
            return value;
        }
        
        private Species SelectPrevious(Species type)
        {
            int index = (_allSpecies.IndexOf(type) + _allSpecies.Count - 1) % _allSpecies.Count;
            return _allSpecies[index];
        }

        private Species SelectNext(Species type)
        {
            int index = (_allSpecies.IndexOf(type) + _allSpecies.Count + 1) % _allSpecies.Count;
            return _allSpecies[index];
        }
    }
}