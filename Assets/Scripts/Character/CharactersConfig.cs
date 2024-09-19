using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "Config/Characters")]
    public class CharactersConfig : ScriptableObject
    {
        [field: SerializeField] public SpeciesModel[] SpeciesModels { get; set; }
        [field: SerializeField] public Species DefaultSpecies { get; set; }
    }
}