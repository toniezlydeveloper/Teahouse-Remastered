using UnityEngine;

namespace Saving.Items
{
    public abstract class ASaveProxy : MonoBehaviour
    {
        [field:SerializeField] public string Id { get; set; }
        
        public abstract void Read(string json);
        
        public abstract string Write();
    }
}