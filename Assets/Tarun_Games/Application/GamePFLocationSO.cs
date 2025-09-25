using DLearners;
using System;
using Unity.VisualScripting;
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

        public GameHandlerBase GetGamePF(int locationID)
        {
            GameObject currentLevelGObj = Resources.Load<GameHandlerBase>(fileLocation + gamePFLocationDatas[locationID].pFLocation).gameObject;
            return currentLevelGObj.GetComponent<GameHandlerBase>();
        }

        public DataSO GetDataSO(int gameID,int locationID)
        {
            DataSO currentLevelGObj = Resources.Load<DataSO>(fileLocation + gamePFLocationDatas[gameID].dataSOLocations[locationID]);
            return currentLevelGObj;
        }

        public GamePFLocationData[] gamePFLocationDatas;
    }
    [Serializable]
    public struct GamePFLocationData
    {
        public int gameID;
        public string gameName;
        public string pFLocation;
        public GameType gameType;
        public string[] dataSOLocations;
    }
    public enum GameType
    {
        LWS,
        Game
    }
}