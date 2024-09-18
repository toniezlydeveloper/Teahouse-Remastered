using UnityEngine;

namespace Character
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField] private Renderer[] colorableRenderers;

        public void ColorRenderers(Color color)
        {
            foreach (Renderer colorableRenderer in colorableRenderers)
            {
                colorableRenderer.material.color = color;
            }
        }
    }
}