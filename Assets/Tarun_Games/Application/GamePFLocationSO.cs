using System;
using UnityEngine;

namespace DLearnersApplication
{
    /// <summary>
    /// Container to Game PF locations.
    /// </summary>
    [CreateAssetMenu(fileName = "GamePFLocationSO", menuName = "ScriptableObjects/GamePFLocationSO", order = 0)]
    public class GamePFLocationSO : ScriptableObject
    {
        /// <summary>
        /// Level locations.
        /// </summary>
        public string fileLocation;

        public GamePFLocationData[] gamePFLocationDatas;
    }
    [Serializable]
    public struct GamePFLocationData
    {
        public string gameID;
        public string gameName;
        public Sprite gameIcon;
        public string pFLocation;
        public GameType gameType;
    }
    public enum GameType
    {
        LWS,
        Game
    }
}