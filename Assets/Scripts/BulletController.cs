using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制飞行物体的移动和碰撞
/// </summary>
public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;
    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject,2f);
    }

    /// <summary>
    /// 物体移动
    /// </summary>
    public void Move(Vector2 moveDirection,float moveForce)
    {
        rbody.AddForce(moveDirection * moveForce);
    }

    //碰撞检测
    void OnCollisionEnter2D(Collision2D other)
    {
        //EnemyController ec
    }
}
