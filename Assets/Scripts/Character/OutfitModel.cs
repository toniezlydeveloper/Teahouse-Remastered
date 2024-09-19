using UnityEngine;

namespace Character
{
    public class OutfitModel : MonoBehaviour
    {
        [SerializeField] private string outfitName;
        
        public void Toggle(string outfit) => GetModel().SetActive(ShouldEnable(outfit));

        private GameObject GetModel()
        {
            Transform model = transform.Find(outfitName);
            return model.gameObject;
        }

        private bool ShouldEnable(string outfit) => outfit == outfitName;
    }
}