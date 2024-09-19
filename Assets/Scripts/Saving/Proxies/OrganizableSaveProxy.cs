using Internal.Dependencies.Core;
using Internal.Pooling;
using Newtonsoft.Json;
using Organization;
using UnityEngine;

namespace Saving.Proxies
{
    public class OrganizableSaveData
    {
        [JsonProperty("pN")] public string PrefabName { get; set; }
        [JsonProperty("yR")] public float YRotation { get; set; }
        [JsonProperty("xP")] public float XPosition { get; set; }
        [JsonProperty("yP")] public float YPosition { get; set; }
        [JsonProperty("zP")] public float ZPosition { get; set; }
    }
    
    public class OrganizableSaveProxy : ASaveProxy
    {
        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();
        
        public override void Read(string json)
        {
            if (TryGetOrganizable(out Organizable organizable))
            {
                Destroy(organizable.gameObject);
            }

            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            
            ReadData(json);
        }

        public override string Write()
        {
            if (!TryGetOrganizable(out Organizable organizable))
            {
                return string.Empty;
            }

            return WriteData(organizable);
        }

        private bool TryGetOrganizable(out Organizable organizable)
        {
            organizable = GetComponentInChildren<Organizable>();
            return organizable != null;
        }

        private void ReadData(string json)
        {
            OrganizableSaveData data = JsonConvert.DeserializeObject<OrganizableSaveData>(json);
            Vector3 position = new Vector3(data.XPosition, data.YPosition, data.ZPosition);
            Quaternion rotation = Quaternion.Euler(0f, data.YRotation, 0f);
            _poolsProxy.Get(data.PrefabName, position, rotation, transform);
        }

        private static string WriteData(Organizable organizable)
        {
            OrganizableSaveData data = new OrganizableSaveData
            {
                PrefabName = organizable.name.Replace("(Clone)", ""),
                YRotation = organizable.transform.rotation.eulerAngles.y,
                XPosition = organizable.transform.position.x,
                YPosition = organizable.transform.position.y,
                ZPosition = organizable.transform.position.z
            };
            
            return JsonConvert.SerializeObject(data);
        }
    }
}