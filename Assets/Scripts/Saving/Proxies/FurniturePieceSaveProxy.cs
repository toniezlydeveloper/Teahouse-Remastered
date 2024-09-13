using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Proxies
{
    public class FurniturePieceSaveData
    {
        [JsonProperty("pN")] public string PrefabName { get; set; }
        [JsonProperty("pX")] public float PositionX { get; set; }
        [JsonProperty("pY")] public float PositionY { get; set; }
        [JsonProperty("pZ")] public float PositionZ { get; set; }
        [JsonProperty("rY")] public float RotationY { get; set; }
    }
    
    public class FurniturePieceSaveProxy : ASaveProxy
    {
        public GameObject GameObject => gameObject;
        
        public override void Read(string json)
        {
        }

        public override string Write()
        {
            FurniturePieceSaveData data = new FurniturePieceSaveData
            {
                PrefabName = transform.name.Replace("(Clone)", ""),
                RotationY = transform.transform.rotation.eulerAngles.y,
                PositionX = transform.transform.position.x,
                PositionY = transform.transform.position.y,
                PositionZ = transform.transform.position.z
            };

            return JsonConvert.SerializeObject(data);
        }
    }
}