using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Saving.Proxies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Saving
{
    public enum FileSaveType
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
        public static void Save(PersistenceType persistenceType, FileSaveType saveType) => File.WriteAllText(GetSaveFilePath(persistenceType, saveType), ReadJsonFromScene(saveType));

        public static void Load(PersistenceType persistenceType, FileSaveType saveType)
        {
            string filePath = GetSaveFilePath(persistenceType, saveType);

            if (!File.Exists(filePath))
            {
                return;
            }
            
            ReadJsonToScene(saveType, filePath);
        }

        public static void ClearVolatile()
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                Clear(PersistenceType.Volatile, saveType);
            }
        }

        public static void Clear(PersistenceType persistenceType, FileSaveType saveType)
        {
            string filePath = GetSaveFilePath(persistenceType, saveType);

            if (!File.Exists(filePath))
            {
                return;
            }
            
            File.Delete(filePath);
        }

        public static void OverrideVolatileWithPersistent()
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                OverrideVolatileWithPersistent(saveType);
            }
        }

        private static void OverrideVolatileWithPersistent(FileSaveType saveType)
        {
            if (TryGetSaveExistingFilePath(PersistenceType.Persistent, saveType, out string persistentFilePath))
            {
                File.WriteAllText(GetSaveFilePath(PersistenceType.Volatile, saveType), File.ReadAllText(persistentFilePath));
            }
            else
            {
                Clear(PersistenceType.Volatile, FileSaveType.Shop);
            }
        }

        private static string GetSaveFilePath(PersistenceType persistenceType, FileSaveType saveType) => Path.Combine(Application.persistentDataPath, $"Teahouse-{persistenceType}-{saveType}.json");
        
        private static bool TryGetSaveExistingFilePath(PersistenceType persistenceType, FileSaveType saveType, out string filePath)
        {
            filePath = Path.Combine(Application.persistentDataPath, $"Teahouse-{persistenceType}-{saveType}.json");
            return File.Exists(filePath);
        }

        private static string ReadJsonFromScene(FileSaveType type)
        {
            Dictionary<string, string> dataById = Object.FindObjectsOfType<ASaveProxy>().Where(proxy => proxy.Type == type).ToDictionary(proxy => proxy.Id, proxy => proxy.Write());
            return JsonConvert.SerializeObject(dataById);
        }

        private static void ReadJsonToScene(FileSaveType type, string path)
        {
            Dictionary<string, string> dataById = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

            foreach (ASaveProxy proxy in Object.FindObjectsOfType<ASaveProxy>().Where(proxy => proxy.Type == type))
            {
                proxy.Read(dataById.First(data => data.Key == proxy.Id).Value);
            }
        }
    }
}