using System;
using UnityEngine;

namespace DLearners
{
    [CreateAssetMenu(fileName = "GameAudioDataSO", menuName = "ScriptableObjects/GameAudioDataSO", order = 0)]
    public class GameAudioDataSO : ScriptableObject
    {
        public SoundDataStruct[] fxSounds;
    }

    [Serializable]
    public struct SoundDataStruct
    {
        public string name;
        public AudioClip audioClip;
        public AudioMixerType mixerType;
        [Range (0,1)]
        public float volumeLevel;
    }
    public enum AudioMixerType
    {
        None,
        BG,
        InGame
    }
}