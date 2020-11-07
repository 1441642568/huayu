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

    //private Vector2 lookDirection;   //角色朝向
    private Animator anim;     //动画
    private Rigidbody2D rbody;       //刚体组件
    private bool isSkillClipPlaying;            //技能音效是否播放中
    private bool isRunClipPlaying;            //移动音效是否播放中


    

    enum FaceDirection { left, right }; //脸的朝向

    FaceDirection faceDirectionf;//脸的朝向

    private Vector2 moveDirection;//移动方向

    //=====玩家的音效
    public AudioClip hitClip;//受伤音效
    public AudioClip launchClip;//邪光斩音效
    public AudioClip runClip;//移动音效
    

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 99;
        currentHealth = 99;
        maxMagic = 99;
        currentMagic = 99;

        invincibleTimer = 0;  //初始计时器


        //lookDirection = new Vector2(1, 0);                //赋值默认人物朝向右方
        anim = transform.GetComponent<Animator>();  //赋值动画组件
        rbody = GetComponent<Rigidbody2D>();              //赋值刚体组件
                                                          //speed = 3f;                                       //赋值初始移动速度

        moveDirection = Vector2.right;//默认朝右
        faceDirectionf = FaceDirection.right;//默认朝右

        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//更新血条
        UImanager.instance.UpdateHealthPoint(currentHealth, maxHealth);//更新生命值显示

        UImanager.instance.UpdateMagicBar(currentMagic, maxMagic);//更新蓝条
        UImanager.instance.UpdateMagicPoint(currentMagic, maxMagic);//更新法力值显示
    }

    // Update is called once per frame
    void Update()
    {
        
        
        Move();   //移动
        Skill();  //技能
        //Jump();   //跳跃

      //  Debug.Log("HP:"+currentHealth+"/"+maxHealth+" "+"MP:" + currentMagic + "/" + maxMagic); //输出玩家信息

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
        if (Input.GetKeyDown(KeyCode.S)&&currentMagic>0)  //确认按下技能键
        {
            ChangeMagic(-1);//每次攻击消耗一点法力值
            if (isSkillClipPlaying == false)
            {
                GameObject bullet = Instantiate(bulletPrefab, rbody.position, Quaternion.identity);  //赋值技能对象
                BulletController bc = bullet.GetComponent<BulletController>();  //获取技能对象
                if (bc != null)  //技能对象不为空
                {
                    bc.Move(moveDirection, 150);  //技能移动 (朝向,速度)
                }
                StartCoroutine(PlaySkillClip());  //开始释放技能
            }      
        }
        //====按下Enter回车键 进行NPC交互
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit2D hit = Physics2D.Raycast(rbody.position, moveDirection, 2f, LayerMask.GetMask("NPC"));
            if (hit.collider!=null)
            {
                NPCmanager npc = hit.collider.GetComponent<NPCmanager>();
                if (npc!=null)
                {
                    npc.ShowDialog();//显示对话框
                }
            }
        }
    }

    /// <summary>
    /// 播放移动音效
    /// </summary>
    /// <returns>音效播放时间长度</returns>
    IEnumerator PlayRunClip()
    {
        //移动音效未处于播放中，才允许播放
        if (isRunClipPlaying == false)
        {
            AudioManager.instance.AudioPlay(runClip);//播放移动音效
            isRunClipPlaying = true; //移动音效播放中
            yield return new WaitForSeconds(runClip.length);  //播放完
            isRunClipPlaying = false;//移动音效处于未播放状态
        }   
    }
    /// <summary>
    /// 播放技能音效
    /// 释放技能 协同 等待 帧阻塞 停进程
    /// </summary>
    /// <returns>延迟时间</returns>
    IEnumerator PlaySkillClip()
    {
        //技能音效未处于播放中，才允许播放
        if (isSkillClipPlaying == false)
        {
            BlendTreeSetFloat();//重置混合树
            anim.SetTrigger("Launch");//切换人物技能动作动画
            AudioManager.instance.AudioPlay(launchClip);//播放技能音效
            isSkillClipPlaying = true; //技能音效播放中
            yield return new WaitForSeconds(1);  //技能冷却时间
            isSkillClipPlaying = false;//技能处于未释放状态
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标

        Vector2 moveVector = new Vector2(moveX, moveY);//移动向量
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            moveDirection = moveVector;//赋值移动方向

            StartCoroutine(PlayRunClip());//播放移动音效
            
        }
        anim.SetFloat("Speed", moveVector.magnitude);//传递移动速度
        
        //if (moveVector.x == 0 || moveVector.y == 0)
        //{
        //    //站立不动
        //    switch (faceDirectionf)//判断脸的朝向
        //    {
        //        case FaceDirection.right:  //脸朝向右
        //            {
        //                BlendTreeSetFloat();
        //                anim.SetFloat("RightStand", 1);
        //                faceDirectionf = FaceDirection.right;//脸朝向右
        //                break;
        //            }
        //        case FaceDirection.left://脸朝向左
        //            {
        //                BlendTreeSetFloat();
        //                anim.SetFloat("LeftStand", 1);
        //                faceDirectionf = FaceDirection.left;
        //                break;
        //            }
        //    }
        //}

        //anim.SetFloat("Look X", moveDirection.x);
        //anim.SetFloat("Look Y", moveDirection.y);
        //anim.SetFloat("Speed", moveVector.magnitude);

        Vector2 position = rbody.position;  
        position += moveVector * speed * Time.deltaTime;
        rbody.MovePosition(position);

        //changeTimer -= Time.deltaTime;
        //if (changeTimer < 0)
        //{
        //    moveDirection *= -1;
        //    changeTimer = changeDirectionTime;
        //}
        //Vector2 position = rbody.position;
        //position.x += moveDirection.x * speed * Time.deltaTime;
        //position.y += moveDirection.y * speed * Time.deltaTime;
        //rbody.MovePosition(position);

        //动画控制
        if (moveVector.x * speed * Time.deltaTime > 0)
        {
            //向右走
            BlendTreeSetFloat();
            anim.SetFloat("RightRun", 1);
            faceDirectionf = FaceDirection.right;//脸朝向右
        }
        else if (moveVector.x * speed * Time.deltaTime < 0)
        {
            //向左走
            BlendTreeSetFloat();
            anim.SetFloat("LeftRun", 1);
            faceDirectionf = FaceDirection.left;
        }
        else if (moveVector.y * speed * Time.deltaTime > 0) //向上走
        {
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("RightRun", 1);
                        faceDirectionf = FaceDirection.right;//脸朝向右
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("LeftRun", 1);
                        faceDirectionf = FaceDirection.left;
                        break;
                    }
            }
        }
        else if (moveVector.y * speed * Time.deltaTime < 0)
        {
            //向下走
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("RightRun", 1);
                        faceDirectionf = FaceDirection.right;//脸朝向右
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("LeftRun", 1);
                        faceDirectionf = FaceDirection.left;
                        break;
                    }
            }
        }
        else
        {
            //站立不动
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("RightStand", 1);
                        faceDirectionf = FaceDirection.right;//脸朝向右
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        BlendTreeSetFloat();
                        anim.SetFloat("LeftStand", 1);
                        faceDirectionf = FaceDirection.left;
                        break;
                    }
            }
        }

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
            anim.SetTrigger("Hit");//播放受伤动画
            AudioManager.instance.AudioPlay(hitClip);//播放受伤音效
            invincibleTimer = invincibleTime;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);//把玩家的生命值约束在0和最大值之间
        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//更新血条
        UImanager.instance.UpdateHealthPoint(currentHealth, maxHealth);//更新生命值显示
    }

    /// <summary>
    /// 改变玩家的法力值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMagic(int amount)
    {
        currentMagic = Mathf.Clamp(currentMagic + amount, 0, maxMagic);//把玩家的法力值约束在0和最大值之间
        UImanager.instance.UpdateMagicBar(currentMagic, maxMagic);//更新蓝条
        UImanager.instance.UpdateMagicPoint(currentMagic, maxMagic);//更新法力值显示
    }

    /// <summary>
    /// 重置动画机混合树参数Parameter
    /// </summary>
    public void BlendTreeSetFloat()
    {
        anim.SetFloat("RightStand", 0);
        anim.SetFloat("LeftStand", 0);
        anim.SetFloat("RightRun", 0);
        anim.SetFloat("LeftRun", 0);
    }













}
