using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_Follow : MonoBehaviour
{
    public static Frog_Follow OBJ_followingCamera;
    public bool B_canfollow = false;

    Transform T_TargetPlayer;

    public float X_Offset, Y_Offset;
    public bool B_Follow_X, B_Follow_Y;


    public void Awake()
    {
        OBJ_followingCamera = this;

    }
    private void LateUpdate()   //player movement in fixed update for smoothness
    {
        if (B_canfollow)
        {
            if(B_Follow_X)
            {
                T_TargetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 xtemp = transform.position;
                xtemp.x = T_TargetPlayer.position.x;
                xtemp.x += X_Offset;
                transform.position = xtemp;
            }
           
            if(B_Follow_Y)
            {
                T_TargetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 ytemp = transform.position;
                ytemp.y = T_TargetPlayer.position.y;
                ytemp.y += Y_Offset;
                transform.position = ytemp;
            }
             
        }

       
    }
}
