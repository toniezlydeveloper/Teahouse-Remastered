using System.Collections;
using Character;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace Saving.Proxies
{
    public class CharacterSaveData
    {
        [JsonProperty("sI")] public int SpeciesIndex { get; set; }
        [JsonProperty("oI")] public int OutfitIndex { get; set; }
        [JsonProperty("cI")] public int ColorIndex { get; set; }
    }
    
    public class CharacterSaveProxy : ASaveProxy
    {
        [SerializeField] private CharactersConfig config;

        private const string SpeciesModelVariableName = "_speciesModel";
        private const string OutfitVariableName = "_outfit";
        private const string ColorVariableName = "_color";
        private const string PlayerObjectName = "P_Player";

        public override void Read(string json) => StartCoroutine(ReadEnumerator(json));

        public override string Write()
        {
            SpeciesModel speciesModel = SpeciesModelVariableName.GetComponent<CustomizationPlatform, SpeciesModel>(FindObjectOfType<CustomizationPlatform>());
            CharacterSaveData data = new CharacterSaveData
            {
                OutfitIndex = speciesModel.Outfits.IndexOf(OutfitVariableName.GetComponent<CustomizationPlatform, string>(FindObjectOfType<CustomizationPlatform>())),
                ColorIndex = speciesModel.Colors.IndexOf(ColorVariableName.GetComponent<CustomizationPlatform, Color>(FindObjectOfType<CustomizationPlatform>())),
                SpeciesIndex = config.SpeciesModels.IndexOf(speciesModel)
            };

            return JsonConvert.SerializeObject(data);
        }

        private IEnumerator ReadEnumerator(string json)
        {
            CharacterSaveData data = JsonConvert.DeserializeObject<CharacterSaveData>(json);
            OverrideModel(data);
            
            yield return null;
            
            SetUpModel(data);
        }

        private void OverrideModel(CharacterSaveData data)
        {
            GameObject player = GameObject.Find(PlayerObjectName);
            
            if (HasModelAlready(player))
            {
                return;
            }
            
            AssignParent(player, Instantiate(GetModel(data)));
        }

        private void SetUpModel(CharacterSaveData data)
        {
            CharacterModel model = GameObject.Find(PlayerObjectName).GetComponentInChildren<CharacterModel>();
            model.ToggleOutfits(GetOutfit(data));
            model.ColorRenderers(GetColor(data));
        }

        private bool HasModelAlready(GameObject player)
        {
            CharacterModel model = player.GetComponentInChildren<CharacterModel>();
            return model != null;
        }

        private void AssignParent(GameObject player, GameObject model)
        {
            model.transform.parent = player.transform;
            model.transform.localPosition = Vector3.zero;
        }

        private GameObject GetModel(CharacterSaveData data)
        {
            CharacterModel model = config.SpeciesModels[data.SpeciesIndex].Prefab;
            return model.gameObject;
        }

        private string GetOutfit(CharacterSaveData data) => config.SpeciesModels[data.SpeciesIndex].Outfits[data.OutfitIndex];

        private Color GetColor(CharacterSaveData data) => config.SpeciesModels[data.SpeciesIndex].Colors[data.ColorIndex];
    }
}