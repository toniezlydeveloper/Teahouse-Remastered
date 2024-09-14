using System;
using UnityEngine;

namespace Saving.Proxies
{
    public abstract class ASaveProxy : MonoBehaviour
    {
        [field:SerializeField] public FileSaveType Type { get; set; }
        [field:SerializeField] public string Id { get; set; }

        private void Awake() => Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;

        public abstract void Read(string json);
        
        public abstract string Write();
    }
}