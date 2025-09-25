using System.Collections.Generic;
using UnityEngine;

namespace DLearners
{
    public class DataSOBase : ScriptableObject
    {
        public Sprite coverPageSprit;

        public InstructionData instructionData;

        public List<Data> datas = new List<Data>();

        public UserData userData;
    }
}