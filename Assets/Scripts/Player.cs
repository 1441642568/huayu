using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRbody;       //角色刚体组件
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerRbody = GetComponent<Rigidbody2D>();  //赋值角色刚体组件
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标
        Vector2 moveVector = new Vector2(moveX, moveY);//移动向量
        //角色位置移动
        Vector2 position = playerRbody.position;                //获取角色位置
        position += moveVector * speed * Time.deltaTime;  //更新角色位置
        playerRbody.MovePosition(position);                     //移动刚体位置
    }

    public void ChangeSpeed(float speedNew)
    {
        this.speed = speedNew;
    }
}
