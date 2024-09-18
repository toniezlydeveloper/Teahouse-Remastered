using UnityEngine;

namespace Character
{
    public class OutfitModel : MonoBehaviour
    {
        [SerializeField] private Outfit presentedOutfit;
        [SerializeField] private string outfitName;
        
        public void Toggle(Outfit outfit) => GetModel().SetActive(ShouldEnable(outfit));

        private GameObject GetModel()
        {
            Transform model = transform.Find(outfitName);
            return model.gameObject;
        }

        private bool ShouldEnable(Outfit outfit) => outfit == presentedOutfit;
    }
}