using System.Linq;
using UnityEngine;

namespace Character
{
    public class OutfitsDestroyer : MonoBehaviour
    {
        [SerializeField] private string[] outfitNames;

        private void Start()
        {
            foreach (string outfitName in outfitNames)
            {
                Destroy(GetModel(outfitName));
            }
        }

        private GameObject GetModel(string outfitName) => GetComponentsInChildren<Transform>().Select(child => child.gameObject).FirstOrDefault(child => child.name == outfitName);
    }
}