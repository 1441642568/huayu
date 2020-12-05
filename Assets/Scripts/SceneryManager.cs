using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryManager : MonoBehaviour
{
    public static SceneryManager instance { get; private set; }
    public string lastScene;//上一个场景名


    private Rigidbody2D playerRbody;       //角色刚体组件

    // Start is called before the first frame update
    void Start()
    {
        instance = this;//单例模式赋值
    }


}
