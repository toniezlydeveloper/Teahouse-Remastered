using System.Collections.Generic;
using Bedroom;
using Grids;
using Interaction;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class PlacedFurniture
    {
        public IFurniturePiece Piece { get; set; }
        public List<GridCell> Cells { get; set; }
        public GameObject Model { get; set; }
    }
    
    public class FurnitureHandler : AInteractionHandler, IGridItemHolder
    {
        [SerializeField] private List<InputActionReference> selectionsInput;
        [SerializeField] private InputActionReference rotateRightInput;
        [SerializeField] private InputActionReference rotateLeftInput;
        [SerializeField] private InputActionReference placeInput;
        [SerializeField] private Material pickPreviewMaterial;
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Color invalidSpotColor;
        [SerializeField] private Color validSpotColor;

        private List<PlacedFurniture> _placedFurniture;
        private IFurniturePiece _selectedPiece;
        private FurnitureSelector _selector;
        private FurnitureRotator _rotator;
        private FurniturePlacer _placer;
        private FurniturePicker _picker;

        public override PlayerMode HandledModes => PlayerMode.Organization;
        public override DayTime HandledDayTime => DayTime.Night;

        public GridItemOrientation Orientation => _rotator.Orientation;
        public GridDimensions Dimensions => _selector.Dimensions;

        private void Awake()
        {
            _placedFurniture = new List<PlacedFurniture>();
            _selector = new FurnitureSelector(selectionsInput, playerMode);
            _rotator = new FurnitureRotator(rotateRightInput, rotateLeftInput);
            _placer = new FurniturePlacer(_placedFurniture, placeInput, previewMaterial, invalidSpotColor, validSpotColor);
            _picker = new FurniturePicker(_placedFurniture, placeInput, pickPreviewMaterial);
        }

        private void OnEnable()
        {
            playerMode.OnChanged += _placer.TryTogglingPreview;
            playerMode.OnChanged += _picker.TryTogglingPreview;
        }

        private void OnDisable()
        {
            playerMode.OnChanged -= _placer.TryTogglingPreview;
            playerMode.OnChanged -= _picker.TryTogglingPreview;
        }

        private void Update()
        {
            if (!ShouldHandleInput())
                return;
            
            HandleVisuals();

            if (TryHandlingPicking())
                return;
            
            HandlePlacement();
        }

        private bool ShouldHandleInput() => playerMode.Value == PlayerMode.Organization;

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
            
            _rotator.HandleOrientation(out FurnitureOrientation orientation);
            _placer.HandlePreview(orientation);

            if (!_placer.TryPlacing(_selectedPiece, orientation))
                return;

            if (_selectedPiece.Count > 0)
                return;
            
            _selector.RemoveSelection();
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
    }
}