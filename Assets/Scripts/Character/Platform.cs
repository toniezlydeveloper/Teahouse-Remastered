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

        private void SelectSpecies(Species species)
        {
            if (IsShowingModelAlready())
            {
                Destroy(_characterModel);
            }

            Cache(Instantiate(GetModel(species).Prefab, spawnPoint));
            Cache(species);
        }

        private void SelectOutfit(Outfit outfit)
        {
            ToggleModels(outfit);
            Cache(outfit);
        }

        private void SelectColor(Color color) => _characterModel.GetComponent<CharacterModel>().ColorRenderers(color);

        private bool IsShowingModelAlready() => _characterModel != null;

        private SpeciesModel GetModel(Species species) => config.SpeciesModels.First(model => model.Type == species);

        private void ToggleModels(Outfit outfit)
        {
            foreach (OutfitModel model in _characterModel.GetComponentsInChildren<OutfitModel>())
            {
                model.Toggle(outfit);
            }
        }

        private void Cache(GameObject characterModel) => _characterModel = characterModel;

        private void Cache(Species species) => _species = species;

        private void Cache(Outfit outfit) => _outfit = outfit;

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