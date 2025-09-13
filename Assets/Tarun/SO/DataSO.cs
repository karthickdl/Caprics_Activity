using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLearners
{
    [CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/DataSO", order = 0)]
    public class DataSO : ScriptableObject
    {
        public bool isProduction;
        [Header("URL")]
        public string getValueURLPRO;
        public string sndValueURLPRO;
        public string getValueURLTesting;
        public string sndValueURLTesting;
        public URLData uRLData;


        public DifficultyLevelType difficultyLevelType;
        public int correctAnswerPoint;
        public int wrongAnswerPoint;

        public Sprite coverPageSprit;

        public InstructionData instructionData;

        public List<Data> datas = new List<Data>();
        public UserData userData;

        public InstructionData GetInstructionData()
        {
            return instructionData;
        }
        public Data GetData(int questionID)
        {
            return datas[questionID];
        }

        public int GetCorrectAnswerPoint()
        {
            return correctAnswerPoint;
        }
        public int GetWrongAnswerPoint()
        {
            return wrongAnswerPoint;
        }
        public Sprite GetCoverPageSprit()
        {
            return coverPageSprit;
        }

        public DifficultyLevelType GetDifficultyLevelType()
        {
            return difficultyLevelType;
        }

        public URLData GetURLData()
        {
            URLData tempURLData  = new URLData();
            if (isProduction)
            {
                tempURLData.sendValueURL = sndValueURLPRO;
                tempURLData.getValueURL = getValueURLPRO;
            }
            else
            {
                tempURLData.sendValueURL = sndValueURLTesting;
                tempURLData.getValueURL = getValueURLTesting;
            }
            return tempURLData;
        }
    }
    [Serializable]
    public struct Data
    {
        public TemplateType answerType;
        public TemplateType questionType;

        public QuestionData questionData;
        public List<OptionData> options;
        public string correctOptions;
    }
    [Serializable]
    public struct OptionData
    {
        public string optionID;
        public string option;
        public string optionAudioURL;
        public AudioClip optionAudioClip;
        public Sprite optionSprit;
    }
    [Serializable]
    public struct QuestionData
    {        
        public string questionID;
        public string question;
        public string questionAudioURL;
        public AudioClip questionAudioClip;
        public Sprite questionSprit;
    }
    [Serializable]
    public struct UserData
    {
        public string userName;
        public string userStandard;
        public string lessonName;
    }
    [Serializable]
    public struct InstructionData
    {
        public List <string> instruction;
        public List <string> instructionAudioURL;
        public List <AudioClip> instructionAudioClip;
    }

    [Serializable]
    public struct URLData
    {        
        public string getValueURL;
        public string sendValueURL;
    }
    public enum TemplateType
    {
        Text,
        IMG,
        Audio,
        Video
    }

    public enum DifficultyLevelType
    {
        Easy,
        Medium,
        Hard 
    }
}