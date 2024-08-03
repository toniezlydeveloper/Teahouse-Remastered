using UnityEngine;

namespace Bedroom
{
    [RequireComponent(typeof(Light))]
    public class DayTimeLightPresenter : MonoBehaviour
    {
        [SerializeField] private DayTimeProxy timeProxy;
        [SerializeField] private Color nightColor;
        [SerializeField] private Color dayColor;

        private Light _light;

        private void Awake() => _light = GetComponent<Light>();

        private void Start() => Setup();

        private void OnEnable() => timeProxy.OnChanged += Setup;
        
        private void OnDisable() => timeProxy.OnChanged -= Setup;

        private void Setup() => Setup(timeProxy.Value);

        private void Setup(DayTime dayTime) => _light.color = dayTime == DayTime.Day ? dayColor : nightColor;
    }
}