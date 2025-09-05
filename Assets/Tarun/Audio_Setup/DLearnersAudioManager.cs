using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace DLearners
{
    public class DLearnersAudioManager : Singleton<DLearnersAudioManager>
    {
        public AudioMixer inGameMixer;
        public AudioMixer bgMixer;

        [System.Serializable]
        public class Sound
        {
            public string name;

            [HideInInspector]
            public AudioSource audioSource;
            public void Play()
            {
                audioSource.Play();
            }

            public void Stop()
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            public void StopImmidiate()
            {
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }

        public Sound[] fxSounds;

        private SoundDataStruct[] fxSounds2 => TarunTesting.Instance.gameAudioDataSO.fxSounds;

        #region Unity Calls
        public void Initialize()
        {
           /* bgMixer.SetFloat("Val", SaveDataHandler.Instance.BgSoundValue);
            inGameMixer.SetFloat("Val", SaveDataHandler.Instance.InGameSoundFXValue);*/
        }
        protected override void Awake()
        {
            base.Awake();
#if VAULT_Plugin_Manager
            return;
#endif
            Initialize();
        }
        #endregion

        public void PlaySound(string name)
        {
            Sound sound = Array.Find(fxSounds, s => s.name == name);
            if (sound != null)
            {
                if (sound.audioSource == null)
                {
                    Transform _tempTransform = transform.Find(name);
                    if (_tempTransform)
                    {
                        sound.audioSource = _tempTransform.GetComponent<AudioSource>();
                    }
                }

                sound.Play();
            }
            else
            {
                Debug.LogWarning("AudioManager -- Sound not found:" + name);
            }
        }

        public void PlaySound2(string name, float delay = 0)
        {
            GameObject audioPlayer = new GameObject("AP");
            SoundDataStruct soundDataStruct = Array.Find(fxSounds2, s => s.name == name);
            AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

            AudioMixerGroup[] group;
            switch (soundDataStruct.mixerType)
            {
                case AudioMixerType.None:
                    break;
                case AudioMixerType.BG:
                    group = bgMixer.FindMatchingGroups("Master");
                    audioSource.outputAudioMixerGroup = group[0];
                    break;
                case AudioMixerType.InGame:
                    group = inGameMixer.FindMatchingGroups("Master");
                    audioSource.outputAudioMixerGroup = group[0];
                    break;
            }
            audioSource.volume = soundDataStruct.volumeLevel;
            audioSource.clip = soundDataStruct.audioClip;
            audioSource.PlayDelayed(delay);            
            Destroy(audioPlayer, audioSource.clip.length);
        }

        public AudioSource PlaySoundCashed(string name, float delay = 0)
        {
            GameObject audioPlayer = new GameObject("AP");
            SoundDataStruct soundDataStruct = Array.Find(fxSounds2, s => s.name == name);
            AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

            AudioMixerGroup[] group;
            switch (soundDataStruct.mixerType)
            {
                case AudioMixerType.None:
                    break;
                case AudioMixerType.BG:
                    group = bgMixer.FindMatchingGroups("Master");
                    audioSource.outputAudioMixerGroup = group[0];
                    break;
                case AudioMixerType.InGame:
                    group = inGameMixer.FindMatchingGroups("Master");
                    audioSource.outputAudioMixerGroup = group[0];
                    break;
            }
            audioSource.volume = soundDataStruct.volumeLevel;
            audioSource.clip = soundDataStruct.audioClip;
            return audioSource;
        }


        public void StopSound(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Sound sound = Array.Find(fxSounds, s => s.name == name);
            if (sound != null)
            {
                if (sound.audioSource == null)
                {
                    Transform _tempTransform = transform.Find(name);
                    if (_tempTransform)
                    {
                        sound.audioSource = _tempTransform.GetComponent<AudioSource>();
                    }
                }

                //sound.Stop();
                sound.StopImmidiate();
            }
            else
            {
                Debug.LogWarning("AudioManager -- Sound not found:" + name);
            }
        }

        public void StopSound2(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            GameObject audioPlayer = new GameObject("AP");
            SoundDataStruct soundDataStruct = Array.Find(fxSounds2, s => s.name == name);
            AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

            audioSource.Stop();
            Destroy(audioPlayer);
        }

        public void StopAllSounds()
        {
            foreach (var item in fxSounds)
            {
                item.audioSource.Stop();
            }
        }

        public void PauseSound(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Sound sound = Array.Find(fxSounds, s => s.name == name);
            if (sound != null)
            {
                if (sound.audioSource == null)
                {
                    Transform _tempTransform = transform.Find(name);
                    if (_tempTransform)
                    {
                        sound.audioSource = _tempTransform.GetComponent<AudioSource>();
                    }
                }

                //sound.Stop();

                sound.audioSource.Pause();

            }
            else
            {
                Debug.LogWarning("AudioManager -- Sound not found:" + name);
            }
        }
        public void UnPauseSound(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Sound sound = Array.Find(fxSounds, s => s.name == name);
            if (sound != null)
            {
                if (sound.audioSource == null)
                {
                    Transform _tempTransform = transform.Find(name);
                    if (_tempTransform)
                    {
                        sound.audioSource = _tempTransform.GetComponent<AudioSource>();
                    }
                }

                //sound.Stop();

                sound.audioSource.UnPause();
            }
            else
            {
                Debug.LogWarning("AudioManager -- Sound not found:" + name);
            }
        }
    }    
}