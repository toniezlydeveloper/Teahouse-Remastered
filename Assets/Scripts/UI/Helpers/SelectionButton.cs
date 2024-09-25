using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private Image iconContainer;
        [SerializeField] private Button button;

        private Action _selectionCallback;

        private void Start() => button.onClick.AddListener(() => _selectionCallback?.Invoke());

        public void Init(SelectableFurniturePieceData pieceData)
        {
            _selectionCallback = pieceData.SelectionCallback;
            iconContainer.sprite = pieceData.Icon;
        }
    }
}