using System;
using System.Collections.Generic;
using System.Linq;
using Bedroom;
using Internal.Dependencies.Core;
using Player;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class InteractionProxy : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactDownInput;
        [SerializeField] private InputActionReference interactUpInput;
        [SerializeField] private InputActionReference interactInput;
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private DayTimeProxy dayTime;
        
        private IHintsPanel _hintsPanel = DependencyInjector.Get<IHintsPanel>();

        private Dictionary<PlayerMode, List<AInteractionHandler>> _interactionHandlers = new();
        private List<InteractionElement> _elementsInRange = new();
        private InteractionElement _highlightedElement;

        private void Awake()
        {
            List<PlayerMode> modes = Enum.GetValues(typeof(PlayerMode)).Cast<PlayerMode>().ToList();
            
            foreach (AInteractionHandler interactionHandler in GetComponents<AInteractionHandler>())
            {
                foreach (PlayerMode mode in modes.Where(mode => interactionHandler.HandledModes.HasFlag(mode)))
                {
                    if (_interactionHandlers.ContainsKey(mode))
                        _interactionHandlers[mode].Add(interactionHandler);
                    else
                        _interactionHandlers[mode] = new List<AInteractionHandler> { interactionHandler };
                }
            }
        }

        private void Start()
        {
            AddCallbacks();
            Present();
        }

        private void OnDestroy() => RemoveCallbacks();

        private void OnTriggerEnter(Collider other)
        {
            if (!TryGetHintPreview(other, out InteractionElement hintPreview))
                return;

            Add(hintPreview);
            Present();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!TryGetHintPreview(other, out InteractionElement hintPreview))
                return;

            Remove(hintPreview);
            Present();
        }

        private void Update() => TryProgressing();

        private bool TryGetHintPreview(Collider other, out InteractionElement interactionElement) => other.TryGetComponent(out interactionElement);

        private void Add(InteractionElement interactionElement)
        {
            if (_elementsInRange.Contains(interactionElement))
                return;
            
            _elementsInRange.Add(interactionElement);
        }

        private void Remove(InteractionElement interactionElement) => _elementsInRange.Remove(interactionElement);

        private void Present(PlayerMode _) => Present();

        private void Present(DayTime _) => Present();
        
        private void Present()
        {
            if (_highlightedElement != null)
                _highlightedElement.Highlight(false);

            _highlightedElement = _elementsInRange.Where(hint => hint != null).OrderBy(hint => hint.Order).FirstOrDefault(hint => hint.HandledMode.HasFlag(playerMode.Value) && hint.HandledDayTime.HasFlag(dayTime.Value));
            
            if (_interactionHandlers.ContainsKey(playerMode.Value))
                _interactionHandlers[playerMode.Value].ForEach(handler => handler.Present(_highlightedElement));
            
            _hintsPanel.Present();

            if (_highlightedElement == null)
                return;

            _highlightedElement.Highlight(true);
            _hintsPanel.Present(_highlightedElement);
        }

        private void TryProgressing()
        {
            if (!interactInput.action.IsPressed())
                return;

            if (_highlightedElement == null)
                return;
            
            _interactionHandlers[playerMode.Value].ForEach(handler => handler.HandleProgressInput(_highlightedElement));
        }

        private void InteractDown(InputAction.CallbackContext _) =>
            _interactionHandlers[playerMode.Value].ForEach(handler => handler.HandleInteractionDownInput(_highlightedElement));

        private void InteractUp(InputAction.CallbackContext _) =>
            _interactionHandlers[playerMode.Value].ForEach(handler => handler.HandleInteractionUpInput(_highlightedElement));

        private void Interact(InputAction.CallbackContext _) =>
            _interactionHandlers[playerMode.Value].ForEach(handler => handler.HandleInteractionInput(_highlightedElement));

        private void AddCallbacks()
        {
            interactDownInput.action.performed += InteractDown;
            interactUpInput.action.performed += InteractUp;
            interactInput.action.performed += Interact;
            playerMode.OnChanged += Present;
            dayTime.OnChanged += Present;
        }

        private void RemoveCallbacks()
        {
            interactDownInput.action.performed -= InteractDown;
            interactUpInput.action.performed -= InteractUp;
            interactInput.action.performed -= Interact;
            playerMode.OnChanged -= Present;
            dayTime.OnChanged -= Present;
        }
    }
}