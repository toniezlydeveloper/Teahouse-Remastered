using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using UnityEngine;

namespace UI.Core
{
    public interface ISelectableFurniturePanel : IDependency
    {
        void Present(SelectableFurniturePieceData[] piecesData);
    }

    public class SelectableFurniturePieceData
    {
        public Action SelectionCallback { get; set; }
        public Sprite Icon { get; set; }
        public string Text { get; set; }
        public int Cost { get; set; }
    }
    
    public class BedroomPanel : AUIPanel, ISelectableFurniturePanel
    {
        public void Present(SelectableFurniturePieceData[] piecesData)
        {
            foreach (SelectableFurniturePieceData pieceData in piecesData)
            {
                pieceData.SelectionCallback.Invoke();
            }
        }
    }
}