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
        Shop,
        Inventory
    }

    public enum PersistenceType
    {
        Persistent,
        Volatile
    }
    
    public static class SavingController
    {
        public static void Save(PersistenceType persistenceType, SaveType saveType) => File.WriteAllText(GetSaveFilePath(persistenceType, saveType), ReadJsonFromScene(saveType));

        public static void Load(PersistenceType persistenceType, SaveType saveType)
        {
            string filePath = GetSaveFilePath(persistenceType, saveType);
                
            if (!File.Exists(filePath))
                return;
            
            ReadJsonToScene(saveType, filePath);
        }

        public static void Clear(PersistenceType persistenceType, SaveType saveType)
        {
            string filePath = GetSaveFilePath(persistenceType, saveType);
                
            if (!File.Exists(filePath))
                return;
            
            File.Delete(filePath);
        }

        private static string GetSaveFilePath(PersistenceType persistenceType, SaveType saveType) => Path.Combine(Application.persistentDataPath, $"Teahouse-{persistenceType}-{saveType}.json");

        private static string ReadJsonFromScene(SaveType type)
        {
            Dictionary<string, string> dataById = Object.FindObjectsOfType<ASaveProxy>().Where(proxy => proxy.Type == type).ToDictionary(proxy => proxy.Id, proxy => proxy.Write());
            return JsonConvert.SerializeObject(dataById);
        }

        private static void ReadJsonToScene(SaveType type, string path)
        {
            Dictionary<string, string> dataById = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

            foreach (ASaveProxy proxy in Object.FindObjectsOfType<ASaveProxy>().Where(proxy => proxy.Type == type))
            {
                proxy.Read(dataById.First(data => data.Key == proxy.Id).Value);
            }
        }
    }
}