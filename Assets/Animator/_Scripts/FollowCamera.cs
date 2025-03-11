using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{   
    public GameObject player;//获得Player位置组件
    public float speed = 2.0f; //摄像机移动速度
    public Vector3 offset = new Vector3(0, 5, -10); //摄像机位移修正变量
    
    void Update()
    {
        transform.position =
            Vector3.Lerp(transform.position, player.transform.position + offset, speed * Time.deltaTime);
    }
}
