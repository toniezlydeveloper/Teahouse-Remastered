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
        private SpeciesModel _speciesModel;
        private GameObject _characterModel;
        private Species _species;
        private string _outfit;
        private Color _color;

        private void Awake() => _characterPanel.Init(new CharacterData
        {
            PreviousSpeciesCallback = SelectPreviousSpecies,
            NextSpeciesCallback = SelectNextSpecies,
            PreviousOutfitCallback = SelectPreviousOutfit,
            NextOutfitCallback = SelectNextOutfit,
            PreviousColorCallback = SelectPreviousColor,
            NextColorCallback = SelectNextColor
        });

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
                Destroy(_characterModel);
            }

            SelectModel(GetModel(species));
            Cache(species);
            RefreshModel();
        }

        private void RefreshModel()
        {
            SelectOutfit(_speciesModel.DefaultOutfit);
            SelectColor(_speciesModel.DefaultColor);
        }

        private void SelectOutfit(string outfit)
        {
            ToggleModels(outfit);
            Cache(outfit);
        }

        private void SelectColor(Color color) => _characterModel.GetComponent<CharacterModel>().ColorRenderers(color);

        private void SelectModel(SpeciesModel model)
        {
            Cache(Instantiate(model.Prefab, spawnPoint));
            Cache(model);
        }

        private bool IsShowingModelAlready() => _characterModel != null;

        private SpeciesModel GetModel(Species species) => config.SpeciesModels.First(model => model.Type == species);

        private void ToggleModels(string outfit)
        {
            foreach (OutfitModel model in _characterModel.GetComponentsInChildren<OutfitModel>())
            {
                model.Toggle(outfit);
            }
        }

        private void Cache(GameObject characterModel) => _characterModel = characterModel;

        private void Cache(SpeciesModel speciesModel) => _speciesModel = speciesModel;
        
        private void Cache(Species species) => _species = species;

        private void Cache(string outfit) => _outfit = outfit;

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
            List<Species> values = ((Species[])Enum.GetValues(typeof(Species))).ToList();
            int index = (values.IndexOf(type) + values.Count - 1) % values.Count;
            return values[index];
        }

        private Species SelectNext(Species type)
        {
            List<Species> values = ((Species[])Enum.GetValues(typeof(Species))).ToList();
            int index = (values.IndexOf(type) + values.Count + 1) % values.Count;
            return values[index];
        }
    }
}