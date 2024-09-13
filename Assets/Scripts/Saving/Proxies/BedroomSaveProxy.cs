using System.Collections.Generic;
using System.Linq;
using Internal.Dependencies.Core;
using Internal.Pooling;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Proxies
{
    public class BedroomSaveData
    {
        [JsonProperty("pJBI")] public Dictionary<string, string> PieceJsonsById { get; set; }
    }

    public class BedroomSaveProxy : ASaveProxy
    {
        public override void Read(string json)
        {
            BedroomSaveData data = JsonConvert.DeserializeObject<BedroomSaveData>(json);
            
            foreach ((string pieceId, string pieceJson) in data.PieceJsonsById)
            {
                ReadPiece(pieceId, pieceJson);
            }
        }

        public override string Write()
        {
            Dictionary<string, string> piecesSaveData = FindObjectsOfType<FurniturePieceSaveProxy>().ToDictionary(proxy => proxy.Id, proxy => proxy.Write());

            return JsonConvert.SerializeObject(new BedroomSaveData
            {
                PieceJsonsById = piecesSaveData
            });
        }

        private void ReadPiece(string id, string json)
        {
            FurniturePieceSaveData pieceData = JsonConvert.DeserializeObject<FurniturePieceSaveData>(json);
            Vector3 piecePosition = new Vector3(pieceData.PositionX, pieceData.PositionY, pieceData.PositionZ);
            Quaternion pieceRotation = Quaternion.Euler(0f, pieceData.RotationY, 0f);
            ASaveProxy proxy = DependencyInjector.Get<IPoolsProxy>().GetTyped<ASaveProxy>(pieceData.PrefabName, piecePosition, pieceRotation);
            proxy.Id = id;
        }
    }
}