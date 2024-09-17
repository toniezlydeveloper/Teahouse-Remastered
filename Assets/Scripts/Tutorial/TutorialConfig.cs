using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Config/Tutorial")]
    public class TutorialConfig : ScriptableObject
    {
        [field:SerializeField] public TutorialStep[] Steps { get; set; }
        [field:SerializeField] public bool ShouldSkipSteps { get; set; }
    }
}