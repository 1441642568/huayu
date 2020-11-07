using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人控制相关
/// </summary>
public class EnemyController : MonoBehaviour
{
    public float speed = 3;//移动速度

    public float changeDirectionTime = 2f;//改变方向的时间

    private float changeTimer;//改变方向的计时器

    public bool isVertical;//是否垂直方向移动

    private Vector2 moveDirection;//移动方向

    private Rigidbody2D rbody;//刚体组件

    private Animator anim;//动画组件

    private bool isFixed;//是否被修复

    public ParticleSystem brokenEffect;//损坏特效
    
    enum FaceDirection { left, right }; //脸的朝向

    FaceDirection faceDirectionf;//脸的朝向


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveDirection = isVertical ? Vector2.up : Vector2.right;//如果是垂直移动赋初值朝上，否则朝右
        faceDirectionf = FaceDirection.right;//默认朝右
        changeTimer = changeDirectionTime;
        isFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFixed) return;//如果被修复了，就不执行下面代码
        changeTimer -= Time.deltaTime;
        if (changeTimer<0)
        {
            moveDirection *= -1;
            changeTimer = changeDirectionTime;
        }
        Vector2 position = rbody.position;
        position.x += moveDirection.x * speed * Time.deltaTime;
        position.y += moveDirection.y * speed * Time.deltaTime;
        rbody.MovePosition(position);

        //动画控制
        if (moveDirection.x * speed * Time.deltaTime > 0)
        {
            //向右走
            anim.SetFloat("walk_right", 1);
            anim.SetFloat("walk_left", 0);
            anim.SetFloat("idle", 0);
            faceDirectionf = FaceDirection.right;//脸朝向右
        }
        else if (moveDirection.x * speed * Time.deltaTime < 0)
        {
            //向左走
            anim.SetFloat("walk_left", 1);
            anim.SetFloat("walk_right", 0);
            anim.SetFloat("idle", 0);
            faceDirectionf = FaceDirection.left;
        }
        else if (moveDirection.y * speed * Time.deltaTime > 0) //向上走
        {
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        anim.SetFloat("walk_right", 1);
                        anim.SetFloat("walk_left", 0);
                        anim.SetFloat("idle", 0);
                        faceDirectionf = FaceDirection.right;
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        anim.SetFloat("walk_left", 1);
                        anim.SetFloat("walk_right", 0);
                        anim.SetFloat("idle", 0);
                        faceDirectionf = FaceDirection.left;
                        break;
                    }
            }
        }
        else if (moveDirection.y * speed * Time.deltaTime < 0)
        {
            //向下走
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        anim.SetFloat("walk_right", 1);
                        anim.SetFloat("walk_left", 0);
                        anim.SetFloat("idle", 0);
                        faceDirectionf = FaceDirection.right;
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        anim.SetFloat("walk_left", 1);
                        anim.SetFloat("walk_right", 0);
                        anim.SetFloat("idle", 0);
                        faceDirectionf = FaceDirection.left;
                        break;
                    }
            }
        }
        else
        {
            //站立不动
            anim.SetFloat("idle", 1);
        }
        
        
        
    }





    /// <summary>
    /// 与玩家的碰撞检测
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc!=null)
        {
            pc.ChangeHealth(-1);
        }
    }

    /// <summary>
    /// 敌人被击
    /// </summary>
    public void Fixed()
    {
        isFixed = true;//被修复
        if (brokenEffect.isPlaying == true)
        {
            brokenEffect.Stop();
        }
        rbody.simulated = false;//禁用物理
        anim.SetFloat("walk_left", 0);
        anim.SetFloat("walk_right", 0);
        anim.SetFloat("idle", 0);
        anim.SetTrigger("fall");//播放被修复动画

        
    }

}
