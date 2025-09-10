using UnityEngine;

[CreateAssetMenu(fileName = "PopUpsListSO", menuName = "ScriptableObjects/PopUpsListSO", order = 1)]
public class PopUpsListSO : ScriptableObject
{
    public PopUpsBase[] popUps;
    public ToastPopUp[] flyPopUps;
}