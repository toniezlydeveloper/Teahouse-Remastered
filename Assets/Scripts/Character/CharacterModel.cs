using UnityEngine;

namespace Character
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField] private Renderer[] colorableRenderers;
        [SerializeField] private string[] outfitNames;
        
        public void ToggleOutfits(string outfit)
        {
            foreach (string outfitName in outfitNames)
            {
                GetModel(outfitName).SetActive(string.Equals(outfitName, outfit));
            }
        }

        public void ColorRenderers(Color color)
        {
            foreach (Renderer colorableRenderer in colorableRenderers)
            {
                colorableRenderer.material.color = color;
            }
        }

        private GameObject GetModel(string outfitName)
        {
            Transform model = transform.Find(outfitName);
            return model.gameObject;
        }
    }
}