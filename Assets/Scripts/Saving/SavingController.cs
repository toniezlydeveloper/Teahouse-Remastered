using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Items
{
    public static class SavingController
    {
        public static void Save()
        {
            Dictionary<string, string> dataById = Object.FindObjectsOfType<ASaveProxy>().ToDictionary(proxy => proxy.Id, proxy => proxy.Write());
            string saveData  = JsonConvert.SerializeObject(dataById);

            string path = Path.Combine(Application.persistentDataPath, "teahouse-save.json");
            
            File.WriteAllText(path, saveData);
        }

        public static void Load()
        {
            string path = Path.Combine(Application.persistentDataPath, "teahouse-save.json");
            
            if (!File.Exists(path))
                return;
            
            Dictionary<string, string> dataById = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
            
            foreach (ASaveProxy proxy in Object.FindObjectsOfType<ASaveProxy>())
            {
                proxy.Read(dataById.First(data => data.Key == proxy.Id).Value);
            }
        }
    }
}