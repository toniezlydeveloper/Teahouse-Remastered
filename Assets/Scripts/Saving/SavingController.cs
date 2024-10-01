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
        Bedroom = 0,
        Shop = 1,
        Inventory = 2,
        Character = 3
    }

    public enum PersistenceType
    {
        Initial,
        Persistent,
        Volatile
    }
    
    public static class SavingController
    {
        public static void Save(PersistenceType persistenceType, FileSaveType saveType) => File.WriteAllText(GetSaveFilePath(persistenceType, saveType), ReadJsonFromScene(saveType));

        public static void Save(PersistenceType persistenceType, FileSaveType saveType, string fileContent) => File.WriteAllText(GetSaveFilePath(persistenceType, saveType), fileContent);

        public static void Load(PersistenceType persistenceType, FileSaveType saveType)
        {
            string filePath = GetSaveFilePath(persistenceType, saveType);

            if (!File.Exists(filePath))
            {
                return;
            }
            
            ReadJsonToScene(saveType, filePath);
        }

        public static bool HasFile(PersistenceType persistenceType, FileSaveType saveType) => File.Exists(GetSaveFilePath(persistenceType, saveType));

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

        public static void Override(PersistenceType originalType, PersistenceType overrideType)
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                TryOverride(saveType, originalType, overrideType);
            }
        }

        public static void OverrideSafe(PersistenceType originalType, PersistenceType overrideType)
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                if (TryOverride(saveType, originalType, overrideType))
                {
                    continue;
                }
                
                Clear(originalType, saveType);
            }
        }

        private static bool TryOverride(FileSaveType saveType, PersistenceType originalType, PersistenceType overrideType)
        {
            if (!TryGetSaveExistingFilePath(overrideType, saveType, out string overrideFilePath))
            {
                return false;
            }
            
            File.WriteAllText(GetSaveFilePath(originalType, saveType), File.ReadAllText(overrideFilePath));
            return true;
        }
        
        private static bool TryGetSaveExistingFilePath(PersistenceType persistenceType, FileSaveType saveType, out string filePath)
        {
            filePath = GetSaveFilePath(persistenceType, saveType);
            return File.Exists(filePath);
        }
        
        private static string GetSaveFilePath(PersistenceType persistenceType, FileSaveType saveType) => Path.Combine(Application.persistentDataPath, $"Teahouse-{persistenceType}-{saveType}.json");

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
                try
                {
                    proxy.Read(dataById.First(data => data.Key == proxy.Id).Value);
                }
                catch
                {
                    Debug.LogError($"Failed to read: {proxy.GetType()}");
                }
            }
        }
    }
}