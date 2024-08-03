using Internal.Audio;
using UnityEngine;

namespace Internal.Animations
{
    public class AnimatorSfxTrigger : MonoBehaviour
    {
        [SerializeField] private AudioTypeChannel audioChannel;

        public void TriggerSfx(AudioSoundType audioType)
        {
            audioChannel.Value = audioType;
        }
    }
}