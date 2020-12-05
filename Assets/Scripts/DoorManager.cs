using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager instance { get; private set; }
    public string sceneName;//传送的场景的名字
    public int pathPoint;//表示和场景中的哪个点相对应，所对应的点在目标场景的SceneControl中
    public float transTime;//传送等待的时间，比如在此时间内让场景屏幕变黑，有过渡感
    //public float playerPositionX;//玩家位置X
    //public float playerPositionY;//玩家位置Y

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 碰撞检测相关
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)  //玩家控制类对象不为空
        {
           // SceneryManager.instance.lastScene = Application.loadedLevelName;//当前场景名存储
            UITextManager.instance.lastScene = Application.loadedLevelName;//当前场景名存储


            Application.LoadLevel(sceneName);



           // pc.playerPosition(playerPositionX, playerPositionY);//把玩家位置传参过去
                                                                // pc.playerRbody = GetComponent<Rigidbody2D>();  //赋值角色刚体组件
                                                                //pc.position = new Vector2(playerRbody.position.x, 3.83f);//设置近景图初始位置
        }
    }

    //public void playerPosition(float x, float y)
    //{
    //    x = playerPositionX;
    //    y = playerPositionY;
    //}


}




