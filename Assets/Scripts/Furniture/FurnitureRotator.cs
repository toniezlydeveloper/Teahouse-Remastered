using Grids;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class FurnitureRotator
    {
        private InputActionReference _rotateRightInput;
        private InputActionReference _rotateLeftInput;
        private int _orientationIndex;
        
        public GridItemOrientation Orientation { get; private set; }

        public FurnitureRotator(InputActionReference rotateRightInput, InputActionReference rotateLeftInput)
        {
            _rotateRightInput = rotateRightInput;
            _rotateLeftInput = rotateLeftInput;
        }

        public void HandleOrientation(out FurnitureOrientation orientation)
        {
            HandleRotationChange();
            ReadOrientation(out orientation);
            ReadOrientation();
        }

        private void HandleRotationChange()
        {
            if (_rotateLeftInput.action.triggered)
                _orientationIndex--;

            if (_rotateRightInput.action.triggered)
                _orientationIndex++;
            
            _orientationIndex = _orientationIndex.Clamp();
        }

        private void ReadOrientation(out FurnitureOrientation orientation) => orientation = _orientationIndex.ReadFurnitureOrientation();

        private void ReadOrientation() => Orientation = _orientationIndex.ReadGridOrientation();
    }
}