using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(menuName = "Config/Development")]
    public class DevelopmentConfig : ScriptableObject
    {
        [field: SerializeField] public bool ShouldSkipCharacterCustomization { get; set; }
        [field: SerializeField] public bool ShouldStartInBuildMode { get; set; }
        [field: SerializeField] public bool ShouldSkipCustomers { get; set; }
        [field: SerializeField] public bool ShouldSkipMainMenu { get; set; }
        [field: SerializeField] public bool ShouldSkipTutorial { get; set; }
        
        public static DevelopmentConfig Instance => Resources.Load<DevelopmentConfig>(InstancePath);

#if UNITY_EDITOR
        private const string InstancePath = "EditorDevelopmentConfig";
#else 
        private const string InstancePath = "BuildDevelopmentConfig";
#endif
    }
}