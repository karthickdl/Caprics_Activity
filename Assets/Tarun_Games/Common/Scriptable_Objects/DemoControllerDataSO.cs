using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DemoControllerDataSO", menuName = "ScriptableObjects/DemoControllerDataSO", order = 0)]
public class DemoControllerDataSO : ScriptableObject
{
    public DemoController_N demoController_N;
    public Sprite bgSprite;
    public DemoControllInfo[] demoControllInfos;
    public AnimationClip clip;
}
[Serializable]
public struct DemoControllInfo
{
    public AudioClip clip;
    public string text;
}