using System.Collections.Generic;
using System.Linq;
using Internal.Flow.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Utilities
{
    public static class UIHelpers
    {
        public static bool IsPointerOverUI()
        {
            PointerEventData data = new PointerEventData(EventSystem.current)
            {
                position = Pointer.current.position.ReadValue()
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem system = EventSystem.current;

            if (system == null)
            {
                return false;
            }
            
            system.RaycastAll(data, results);
            return results.Any(result => result.gameObject.GetComponent<AUIPanel>() == null);
        }
    }
}