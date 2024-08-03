using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Internal.Audio
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private string volumeParameter;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider audioSlider;

        private const float Multiplier = 30f;
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(volumeParameter))
            {
                audioSlider.value = PlayerPrefs.GetFloat(volumeParameter);
            }
            
            audioSlider.onValueChanged.AddListener(RefreshAudio);
            RefreshAudio(audioSlider.value);
        }

        private void RefreshAudio(float normalizedValue)
        {
            audioMixer.SetFloat(volumeParameter, Mathf.Log10(normalizedValue) * Multiplier);
            PlayerPrefs.SetFloat(volumeParameter, normalizedValue);
        }
    }
}