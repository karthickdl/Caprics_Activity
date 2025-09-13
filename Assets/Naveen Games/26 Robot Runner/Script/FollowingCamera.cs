using DLearners;
using UnityEngine;

public class FollowingCamera : Singleton<FollowingCamera>
{
    public bool canfollow;

    Transform T_TargetPlayer;
   
    public float X_Offset, Y_Offset;

    public void Init(Transform transform)
    {
        T_TargetPlayer = transform;
    }

    private void LateUpdate()   //player movement in fixed update for smoothness
    {
        if (canfollow)
        {
            Vector3 xtemp = transform.position;
            xtemp.x = T_TargetPlayer.position.x;
            xtemp.x += X_Offset;
            transform.position = xtemp;

            Vector3 ytemp = transform.position;
            ytemp.y = T_TargetPlayer.position.y;
            ytemp.y += Y_Offset;
            transform.position = ytemp;
        }
    }
}
