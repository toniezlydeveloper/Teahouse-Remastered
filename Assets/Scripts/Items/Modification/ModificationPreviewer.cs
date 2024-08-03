using UnityEngine;

namespace Items.Modification
{
    public class ModificationPreviewer : MonoBehaviour
    {
        [SerializeField] private GameObject uiParent;

        public void Toggle(bool state) => uiParent.SetActive(state);
    }
}