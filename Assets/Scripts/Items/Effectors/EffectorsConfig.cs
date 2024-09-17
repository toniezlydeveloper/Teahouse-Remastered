using UnityEngine;

namespace Items.Effectors
{
    [CreateAssetMenu(menuName = "Config/Effectors")]
    public class EffectorsConfig : ScriptableObject
    {
        [field:SerializeField] public Color ProcessingColor { get; set; }
        [field:SerializeField] public Color ProcessedColor { get; set; }
    }
}