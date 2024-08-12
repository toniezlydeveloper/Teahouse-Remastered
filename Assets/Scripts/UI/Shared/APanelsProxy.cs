using System.Linq;
using Internal.Dependencies.Core;
using UnityEngine;

namespace UI.Shared
{
    public abstract class APanelsProxy<TPanel> : MonoBehaviour where TPanel : IDependency
    {
        [SerializeField] private GameObject[] panels;

        protected TPanel[] typedPanels;

        private void Awake() => typedPanels = panels.Select(panel => panel.GetComponent<TPanel>()).ToArray();

        private void OnValidate()
        {
            if (panels.Length > 0)
                return;

            panels = GetComponentsInChildren<TPanel>().OfType<MonoBehaviour>().Select(panel => panel.gameObject).ToArray();
        }
    }
}