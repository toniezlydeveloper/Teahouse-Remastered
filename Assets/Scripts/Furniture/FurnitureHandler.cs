using System.Collections.Generic;
using Bedroom;
using Grids;
using Interaction;
using Internal.Dependencies.Core;
using Player;
using States;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Furniture
{
    public class PlacedFurniture
    {
        public IFurniturePiece Piece { get; set; }
        public List<GridCell> Cells { get; set; }
        public GameObject Model { get; set; }
    }
    
    public class FurnitureHandler : MonoBehaviour, IGridItemHolder
    {
        [SerializeField] private InputActionReference rotateRightInput;
        [SerializeField] private InputActionReference rotateLeftInput;
        [SerializeField] private InputActionReference placeInput;
        [SerializeField] private Material pickPreviewMaterial;
        [SerializeField] private PurchasableItemsConfig itemsConfig;
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Color invalidSpotColor;
        [SerializeField] private Color validSpotColor;

        private IFurnishingCore _furnishingCore = DependencyInjector.Get<IFurnishingCore>();
        private List<PlacedFurniture> _placedFurniture;
        private IFurniturePiece _selectedPiece;
        private FurnitureSelector _selector;
        private FurnitureRotator _rotator;
        private FurniturePlacer _placer;
        private FurniturePicker _picker;

        public GridItemOrientation Orientation => _rotator.Orientation;
        public GridDimensions Dimensions => _selector.Dimensions;

        private void Start()
        {
            GetReferences();
            AddCallbacks();
        }

        private void OnDestroy() => RemoveCallbacks();

        private void Update()
        {
            if (UIHelpers.IsPointerOverUI())
                return;
            
            if (!ShouldHandleInput())
                return;
            
            HandleVisuals();

            if (TryHandlingPicking())
                return;
            
            HandlePlacement();
        }

        private bool ShouldHandleInput() => _furnishingCore.IsEnabled && playerMode.Value == PlayerMode.Organization;

        private void HandleVisuals()
        {
            _selector.HandleSelection(out _selectedPiece);
            _placer.TryPrepareNewPreview(_selectedPiece);
        }

        private void HandlePlacement()
        {
            _picker.Restore();
            
            if (_selectedPiece == null)
                return;
            
            _rotator.HandleOrientation(out Orientation orientation);
            _placer.HandlePreview(orientation);

            if (!_placer.TryPlacing(_selectedPiece, orientation))
                return;
            
            _placer.Destroy();
        }

        private bool TryHandlingPicking()
        {
            if (!_picker.HasPickUp())
                return false;
            
            _picker.HandlePickUp();
            _placer.Destroy();
            return true;
        }

        private void GetReferences()
        {
            _placedFurniture = new List<PlacedFurniture>();
            _selector = new FurnitureSelector(itemsConfig, playerMode);
            _rotator = new FurnitureRotator(rotateRightInput, rotateLeftInput);
            _placer = new FurniturePlacer(_placedFurniture, placeInput, previewMaterial, invalidSpotColor, validSpotColor);
            _picker = new FurniturePicker(_placedFurniture, placeInput, pickPreviewMaterial);
        }

        private void AddCallbacks()
        {
            playerMode.OnChanged += _placer.TryTogglingPreview;
            playerMode.OnChanged += _picker.TryTogglingPreview;
        }

        private void RemoveCallbacks()
        {
            playerMode.OnChanged -= _placer.TryTogglingPreview;
            playerMode.OnChanged -= _picker.TryTogglingPreview;
        }
    }
}