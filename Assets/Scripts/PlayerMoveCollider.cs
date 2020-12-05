using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCollider : MonoBehaviour
{
    private Rigidbody2D rbody;       //刚体组件
    private int _MoveSpeed;     //移动速度

    public int MoveSpeed { get => _MoveSpeed; set => _MoveSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 3;//移动速度
        rbody = GetComponent<Rigidbody2D>();  //赋值角色刚体组件
    }

    // Update is called once per frame
    void Update()
    {
        Move();   //移动
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标
        Vector2 moveVector = new Vector2(moveX, moveY);//移动向量
        //角色位置移动
        Vector2 position = rbody.position;                //获取角色位置
        position += moveVector * MoveSpeed * Time.deltaTime;  //更新角色位置
        rbody.MovePosition(position);                     //移动刚体位置
    }
}
