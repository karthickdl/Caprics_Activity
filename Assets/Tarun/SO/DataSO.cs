using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLearners
{
    [CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/DataSO", order = 0)]
    public class DataSO : ScriptableObject
    {

        public DifficultyLevelType difficultyLevelType;
        public int correctAnswerPoint;
        public int wrongAnswerPoint;

        public string instruction;
        public string instructionAudioURL;
        public AudioClip instructionAudioClip;

        public List<Data> datas = new List<Data>();
        public UserData userData;

      /*  public void SetData(Texture2D tt)
        {
            questionte = tt;
        }

        public void SetDatasp(Sprite tt)
        {
            questiontequestionSP = tt;
        }*/

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