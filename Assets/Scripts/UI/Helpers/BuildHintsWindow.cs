using Internal.Dependencies.Core;
using States;
using UnityEngine;

namespace UI.Helpers
{
    public class BuildHintsWindow : MonoBehaviour, IFurnishingListener
    {
        [SerializeField] private GameObject[] buildHints;

        private void Start() => DependencyInjector.AddRecipeElement(GetComponent<IFurnishingListener>());

        private void OnDestroy() => DependencyInjector.RemoveRecipeElement(GetComponent<IFurnishingListener>());
        
        public void Toggle(bool state)
        {
            foreach (GameObject hint in buildHints)
            {
                hint.SetActive(state);
            }
        }
    }
}