using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Saving.Proxies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Saving
{
    public enum SaveType
    {
        Bedroom,
        Shop
    }
    
    public static class SavingController
    {
        private static readonly Dictionary<SaveType, string> FileNameByType = new()
        {
            { SaveType.Bedroom, "teahouse-bedroom-save.json"},
            { SaveType.Shop, "teahouse-shop-save.json"}
        };
        
        public static void Save(SaveType type) => File.WriteAllText(GetSaveFilePath(type), ReadJsonFromScene());

        public static void Load(SaveType type)
        {
            string path = GetSaveFilePath(type);
                
            if (!File.Exists(path))
                return;
            
            ReadJsonToScene(path);
        }

        public static void Clear(SaveType type)
        {
            string path = GetSaveFilePath(type);
                
            if (!File.Exists(path))
                return;
            
            File.Delete(path);
        }

        private static string GetSaveFilePath(SaveType type) => Path.Combine(Application.persistentDataPath, FileNameByType[type]);

        private static string ReadJsonFromScene()
        {
            Dictionary<string, string> dataById = Object.FindObjectsOfType<ASaveProxy>().ToDictionary(proxy => proxy.Id, proxy => proxy.Write());
            return JsonConvert.SerializeObject(dataById);
        }

        private static void ReadJsonToScene(string path)
        {
            Dictionary<string, string> dataById = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

            foreach (ASaveProxy proxy in Object.FindObjectsOfType<ASaveProxy>())
            {
                proxy.Read(dataById.First(data => data.Key == proxy.Id).Value);
            }
        }
    }
}