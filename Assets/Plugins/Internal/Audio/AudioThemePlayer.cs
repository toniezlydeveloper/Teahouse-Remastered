using DG.Tweening;
using UnityEngine;

namespace Internal.Audio
{
    public class AudioThemePlayer : MonoBehaviour
    {
        [SerializeField] private float targetVolume;
        [SerializeField] private float volumeRaiseDuration;
        [SerializeField] private AudioSource themeSource;

        private void Start()
        {
            themeSource.volume = 0f;
            DOTween.To(() => themeSource.volume, value => themeSource.volume = value, targetVolume, volumeRaiseDuration);
        }
    }
}