using UnityEngine;

namespace Bedroom
{
    public class DayTimeInitializer : MonoBehaviour
    {
        [SerializeField] private DayTimeProxy proxy;

        private void Start() => proxy.Value = DayTime.Day;
    }
}