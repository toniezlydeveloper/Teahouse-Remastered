using System;
using System.Linq;
using Internal.Dependencies.Core;
using UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items.Selection
{
    public interface IItemSelector : IDependency
    {
        void Init(AddInSelectionData data);
        void Deinit();
    }
    
    public class ItemSelector : MonoBehaviour, IItemSelector
    {
        [SerializeField] private InputActionReference[] playerInputs;
        [SerializeField] private InputActionReference pointer;
        [SerializeField] private InputActionReference select;

        private IItemSelectionPanel _itemSelectionPanel;
        private Action<Enum> _selectionCallback;
        private int _latelySelectedValueIndex;
        private int _selectedValueIndex;
        private float _valueOffset;
        private float _valueRange;
        private bool _isEnabled;
        private Enum[] _values;
        private float _angle;

        private const float FullAngle = 360f;
        private const float HalfAngle = 180f;

        private void Update()
        {
            HandleSelection();
            HandleSelectionPresentation();
        }

        public void Init(AddInSelectionData data)
        {
            ToggleInput(false);
            EnableSelection(data);
            EnablePresentation();
        }

        public void Deinit()
        {
            ToggleInput(true);
            DisableSelection();
            DisablePresentation();
        }

        private void ToggleInput(bool state)
        {
            foreach (InputActionReference input in playerInputs)
            {
                if (state)
                {
                    input.action.Enable();
                }
                else
                {
                    input.action.Disable();
                }
            }
        }

        private void EnableSelection(AddInSelectionData data)
        {
            _itemSelectionPanel = DependencyInjector.Get<IItemSelectionPanel>();
            _values = Enum.GetValues(data.AddInType).OfType<Enum>().ToArray();
            _valueRange = FullAngle / _values.Length;
            _selectionCallback = data.SelectionCallback;
            _valueOffset = _valueRange * 0.5f;
            _latelySelectedValueIndex = -1;
            _isEnabled = true;
        }

        private void DisableSelection()
        {
            _latelySelectedValueIndex = -1;
            _selectedValueIndex = -1;
            _isEnabled = false;
        }

        private void HandleSelection()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            Vector2 direction = pointer.action.ReadValue<Vector2>() - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            float angle = (_valueOffset + Vector2.SignedAngle(Vector2.up, direction) + HalfAngle) % FullAngle;
            _selectedValueIndex = (int)(angle / _valueRange);

            if (!select.action.triggered)
            {
                return;
            }
            
            _selectionCallback.Invoke(_values[_selectedValueIndex]);
        }
        
        private void HandleSelectionPresentation()
        {
            if (_latelySelectedValueIndex == _selectedValueIndex)
            {
                return;
            }
            
            _itemSelectionPanel.Present(_selectedValueIndex);
            _latelySelectedValueIndex = _selectedValueIndex;
        }

        private void EnablePresentation()
        {
            _itemSelectionPanel.Init(_values);
            _itemSelectionPanel.Present(true);
        }

        private void DisablePresentation()
        {
            _itemSelectionPanel.Present(false);
        }
    }
}