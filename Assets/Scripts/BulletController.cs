using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制飞行物体的移动和碰撞
/// </summary>
public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;
    public AudioClip hitClip;//命中音效
    public int AttackPoint;//技能的攻击力
    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        //两秒后消失 相当于300距离
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
        Debug.Log("碰到敌人" );
        MonsterController ec = other.gameObject.GetComponent<MonsterController>();
        if (ec!=null)
        {
            //碰到敌人
            
            ec.Fixed(AttackPoint);//修复敌人
        }
        AudioManager.instance.AudioPlay(hitClip);//播放命中音效
        Destroy(this.gameObject);//碰到物体消失
    }
}
