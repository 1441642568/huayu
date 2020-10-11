using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //
    private float invincibleTime = 2f;  //无敌时间2秒

    private float invincibleTimer;  //无敌计时器

    private bool isInvincible;  //是否处于无敌状态

    public float speed;              //角色移动速度
    public GameObject bulletPrefab;  //子弹技能

    private int maxHealth;       //最大生命值

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    private int currentHealth;       //当前生命值

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    private int maxMagic;       //最大魔法值

    public int MaxMagic
    {
        get { return maxMagic; }
        set { maxMagic = value; }
    }
    private int currentMagic;       //当前魔法值

    public int CurrentMagic
    {
        get { return currentMagic; }
        set { currentMagic = value; }
    }

    private Vector2 lookDirection;   //角色朝向
    private Animator anim;     //动画
    private Rigidbody2D rbody;       //刚体组件
    private bool isSkill;            //是否释放技能中


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 10;
        currentHealth = 5;
        maxMagic = 10;
        currentMagic = 5;

        invincibleTimer = 0;  //初始计时器


        lookDirection = new Vector2(1, 0);                //赋值默认人物朝向右方
        anim = transform.GetComponent<Animator>();  //赋值动画组件
        rbody = GetComponent<Rigidbody2D>();              //赋值刚体组件
        //speed = 3f;                                       //赋值初始移动速度
    }

    // Update is called once per frame
    void Update()
    {
        Move();   //移动
        Skill();  //技能
        //Jump();   //跳跃

        Debug.Log("HP:"+currentHealth+"/"+maxHealth+" "+"MP:" + currentMagic + "/" + maxMagic); //输出玩家信息

        //==========无敌计时
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;  //倒计时
            if (invincibleTimer<0)  //倒计时结束以后（2秒）
            {
                isInvincible = false;  //取消无敌状态
            }
        }
    }

    //void Jump()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        //从左边发射一条射线
    //        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position - new Vector3(playerHalfWidth, 0), Vector3.down, 2 * playerHalfHeight + 1, 1 << LayerMask.NameToLayer("plane"));
    //        //检测射线为空就从右边发射一条射线
    //        if (hitInfo.collider == null)
    //        {
    //            hitInfo = Physics2D.Raycast(transform.position + new Vector3(playerHalfWidth, 0), Vector3.down, 2 * playerHalfHeight + 1, 1 << LayerMask.NameToLayer("plane"));
    //        }
    //        //检测射线不为空就从跳跃
    //        if (hitInfo.collider != null)
    //        {
    //            //射线小于角色高度一半
    //            if (hitInfo.distance < playerHalfHeight + 1)
    //            {
    //                rig2d.velocity = new Vector2(rig2d.velocity.x, jumpSpeed);
    //            }
    //        }

    //    }

    //}

    /// <summary>
    /// 释放技能
    /// </summary>
    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.S))  //确认按下技能键
        {
            if (isSkill == false)  //确认技能不在释放中
            {
                StartCoroutine(PlayAndLog());  //开始释放技能
            }
        }
    }

    /// <summary>
    /// 释放技能 协同 等待 帧阻塞 停进程
    /// </summary>
    /// <returns>延迟时间</returns>
    IEnumerator PlayAndLog()
    {
        isSkill = true;  //开始技能释放
        anim.SetBool("isSkill", true);  //切技能动画
        yield return new WaitForSeconds(0.2f);  //延迟0.5秒播放动画
        GameObject bullet = Instantiate(bulletPrefab, rbody.position, Quaternion.identity);  //赋值技能对象
        BulletController bc = bullet.GetComponent<BulletController>();  //获取技能对象
        if (bc != null)  //技能对象不为空
        {
            bc.Move(lookDirection, 150);  //技能移动 (朝向,速度)
        }
        yield return new WaitForSeconds(1f);  //技能释放中 冷却时间1秒
        anim.SetBool("isSkill", false);  //切回动画
        isSkill = false;  //技能释放完
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标

        //角色朝向
        Vector2 moveVector = new Vector2(moveX, moveY);
        if (moveVector.x!=0||moveVector.y!=0)
        {
            lookDirection = moveVector;
        }
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", moveVector.magnitude);

        Vector2 position = rbody.position;  
        position += moveVector * speed * Time.deltaTime;
        rbody.MovePosition(position);

        ////获取按键
        //if (Input.anyKey)
        //{
        //    foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))  //遍历
        //    {
        //        if (Input.GetKey(keyCode))  //确认按下按键
        //        {
        //            switch (keyCode)  //根据按键执行对应操作
        //            {
        //                case KeyCode.LeftArrow:
        //                    {
        //                        myAnimator.SetBool("isRun", true);                            //可以跑
        //                        myAnimator.SetBool("isLeftRun", true);                        //可以左跑
        //                        lookDirection = new Vector2(-1, 0);                           //人物朝向左方
        //                        transform.Translate(Vector2.left * Time.deltaTime * speed);   //执行左跑
        //                        break;
        //                    }
        //                case KeyCode.RightArrow:
        //                    {
        //                        myAnimator.SetBool("isRun", true);                            //可以跑
        //                        myAnimator.SetBool("isLeftRun", false);                       //不能左跑
        //                        lookDirection = new Vector2(1, 0);                            //人物朝向右方
        //                        transform.Translate(Vector2.right * Time.deltaTime * speed);  //执行右跑
        //                        break;
        //                    }
        //                case KeyCode.UpArrow:
        //                    {
        //                        myAnimator.SetBool("isRun", true);
        //                        if (lookDirection.x == 1)//朝向右方
        //                        {
        //                            myAnimator.SetBool("isLeftRun", false);
        //                        }
        //                        else
        //                        {
        //                            myAnimator.SetBool("isLeftRun", true);
        //                        }

        //                        transform.Translate(Vector2.up * Time.deltaTime * speed);
        //                        break;
        //                    }
        //                case KeyCode.DownArrow:
        //                    {
        //                        myAnimator.SetBool("isRun", true);
        //                        if (lookDirection.x == -1)//朝向左方
        //                        {
        //                            myAnimator.SetBool("isLeftRun", true);
        //                        }
        //                        else
        //                        {
        //                            myAnimator.SetBool("isLeftRun", false);
        //                        }
        //                        transform.Translate(Vector2.down * Time.deltaTime * speed);
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    myAnimator.SetBool("isRun", false);  //没有按键的时候停止动画
        //}
    }

    /// <summary>
    /// 改变玩家的生命值
    /// </summary>
    /// <param name="amount">增加的生命值数量</param>
    public void ChangeHealth(int amount)
    {

        if (amount < 0) //如果玩家受到伤害 增加血量值为负数
        {
            if (isInvincible==true)  //如果是无敌状态直接返回不受伤害
            {
                return;
            }
            isInvincible = true;  //无敌
            invincibleTimer = invincibleTime;
        }

        //把玩家的生命值约束在0和最大值之间
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //if (currentHealth < maxHealth && currentHealth >= 0)
        //{
        //    currentHealth += amount;  //当前血量加传过来的值 正数加血 负数扣血
        //}
    }

    /// <summary>
    /// 改变玩家的法力值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMagic(int amount)
    {
        //把玩家的法力值约束在0和最大值之间
        currentMagic = Mathf.Clamp(currentMagic + amount, 0, maxMagic);

        //if (currentMagic < maxMagic && currentMagic >=0)
        //{
        //    currentMagic += amount;  //当前值加传过来的值 正数加 负数扣
        //}
    }















}
