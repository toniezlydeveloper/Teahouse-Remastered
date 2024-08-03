using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Internal.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private float audioPlayInterval;
        [SerializeField] private AudioTypeChannel audioChannel;
        [SerializeField] private AudioSourcePackage[] sources;
        [SerializeField] private AudioPackage[] packages;
        
        private Dictionary<AudioSoundType, AudioSource> _sourcesByAudioType = new Dictionary<AudioSoundType, AudioSource>();
        private Dictionary<AudioSoundType, AudioClip> _clipsByAudioType = new Dictionary<AudioSoundType, AudioClip>();
        private Dictionary<AudioSoundType, float> _requestTimesByAudioType = new Dictionary<AudioSoundType, float>();

        private void Awake()
        {
            audioChannel.OnChanged += PlayAudio;

            _clipsByAudioType = packages.ToDictionary(package => package.Type, package => package.Clip);
            
            foreach (AudioSourcePackage sourcePackage in sources)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = sourcePackage.Group;
                
                foreach (AudioSoundType type in sourcePackage.Types)
                {
                    _sourcesByAudioType.Add(type, source);
                }
            }
            
            foreach (AudioSoundType audioType in Enum.GetValues(typeof(AudioSoundType)))
            {
                _requestTimesByAudioType.Add(audioType, 0f);
            }
        }

        private void OnDestroy()
        {
            audioChannel.OnChanged -= PlayAudio;
        }

        private void PlayAudio(AudioSoundType type)
        {
            if (Time.time < _requestTimesByAudioType[type] + audioPlayInterval)
            {
                return;
            }

            if (!_clipsByAudioType.TryGetValue(type, out AudioClip clip))
            {
                return;
            }
            
            if (!_sourcesByAudioType.TryGetValue(type, out AudioSource source))
            {
                return;
            }
            
            _requestTimesByAudioType[type] = Time.time;
            source.PlayOneShot(clip);
        }
    }
}