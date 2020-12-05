using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }
    //=====玩家属性信息
    private int _HpMax;       //最大生命值
    private int _HpCurrent;   //当前生命值
    private int _MpMax;       //最大法力值
    private int _MpCurrent;   //当前法力值
    private int _AttackPoint;//攻击力
    private int _DefensePoint;//防御力
    private int _BeHitPoint;//被击值
    private int _HurtPoint;//受伤值
    private int _MoveSpeed;     //移动速度
    public int HpMax { get => _HpMax; set => _HpMax = value; }
    public int HpCurrent { get => _HpCurrent; set => _HpCurrent = value; }
    public int MpMax { get => _MpMax; set => _MpMax = value; }
    public int MpCurrent { get => _MpCurrent; set => _MpCurrent = value; }
    public int AttackPoint { get => _AttackPoint; set => _AttackPoint = value; }
    public int DefensePoint { get => _DefensePoint; set => _DefensePoint = value; }
    public int BeHitPoint { get => _BeHitPoint; set => _BeHitPoint = value; }
    public int HurtPoint { get => _HurtPoint; set => _HurtPoint = value; }
    public int MoveSpeed { get => _MoveSpeed; set => _MoveSpeed = value; }



    //=====无敌控制
    private bool isInvincible;  //是否处于无敌状态
    private float invincibleTime = 2f;  //受伤间隔无敌时间2秒
    private float invincibleTimer;  //受伤间隔无敌计时器
    //=====动画控制
    private Animator anim;     //动画对象
    
    //=====方向属性
    enum FaceDirection { left, right }; //脸的朝向
    FaceDirection faceDirectionf;//脸的朝向
    private Vector2 moveDirection;//移动方向
    //=====玩家的音效
    [Header("普通攻击打击音效")]
    public AudioClip _NormalAttackHitClip;
    [Header("普通攻击角色语音")]
    public AudioClip[] _NormalAttackPlayerClip;
    [Header("邪光斩音效")]
    public AudioClip _LaunchClip;
    [Header("受伤音效")]
    public AudioClip _HitClip;
    [Header("移动音效")]
    public AudioClip _RunClip;

    private bool isClipPlayingSkill;          //技能音效是否播放中
    private bool isClipPlayingRun;            //移动音效是否播放中
    private bool isClipPlayingNormalAttack;   //普通攻击音效是否播放中
    private bool isClipPlayingNormalAttackPlayer;   //普通攻击玩家语言是否播放中

    //=====近景图
    public GameObject[] nearLandscapePrefab;   //近景图预制体
    private GameObject nearLandscape;        //近景图对象
    private Rigidbody2D nearLandscapeRbody;  //近景图刚体组件 
    //=====远景图
    public GameObject[] farLandscapePrefab;   //远景图预制体
    private GameObject farLandscape;        //远景图对象
    private Rigidbody2D farLandscapeRbody;  //远景图刚体组件

    public float playerPositionX;//玩家位置X
    public float playerPositionY;//玩家位置Y
    float nearScenerPositionY;//近景位置Y
    float farScenerPositionY;//远景位置Y
    int SceneryIndex;//风景数组下标

    //=====游戏对象
    public GameObject bulletPrefab;  //子弹技能

    private Rigidbody2D playerRbody;       //角色刚体组件
 
    public Text gameMessage;//游戏报文
    public int testValue1;
    public int testValue2;
    string test3;
    private int i;//通用数组下标变量

    void Awake()
    {
        playerRbody = GetComponent<Rigidbody2D>();


        
        switch (Application.loadedLevelName)
        {
            case "SeriaRoom"://赛丽亚房房间
                {
                    playerPositionX = 0.21f;
                    playerPositionY = -2.87f;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "Elvenguard"://艾尔文防线
                {
                    nearScenerPositionY = 0.04f;
                    farScenerPositionY = 0.1f;
                    SceneryIndex = 1;
                    switch (UITextManager.instance.lastScene)
                    {
                        case "SeriaRoom":
                            playerRbody.position = new Vector2(-1.54f, -0.44f);
                            break;
                        case "Elvenguard-01":
                            playerRbody.position = new Vector2(9.5f, -1.79f);
                            break;
                        case "HendonMyre-07":
                            playerRbody.position = new Vector2(-9.12f, -1.79f);
                            break;
                    }
                    break;
                }
            case "Elvenguard-01"://艾尔文防线进入洛兰入口
                {
                    nearScenerPositionY = 0.04f;
                    farScenerPositionY = 0.1f;
                    SceneryIndex = 1;
                    switch (UITextManager.instance.lastScene)
                    {
                        case "Elvenguard":
                            playerRbody.position = new Vector2(-5.97f, -1.85f);
                            break;
                        case "LorienForest":
                            playerRbody.position = new Vector2(6.07f, -1.85f);
                            break;
                    }
                    break;
                }
            case "Elvenguard-02"://南部溪谷桥
                {
                    nearScenerPositionY = 0.04f;
                    farScenerPositionY = 0.1f;
                    SceneryIndex = 1;
                    switch (UITextManager.instance.lastScene)
                    {
                        case "Elvenguard":
                            playerRbody.position = new Vector2(-3.21f, -0.67f);
                            break;
                        case "Alfhlyra":
                            playerRbody.position = new Vector2(6.03f, -1.51f);
                            break;
                    }
                    break;
                }
            case "Alfhlyra"://赫顿马尔过道
                {
                    playerPositionX = 13.51f;
                    playerPositionY = 1.46f;
                    nearScenerPositionY = 3.98f;
                    farScenerPositionY = 5.28f;
                    SceneryIndex = 1;
                    break;
                }
            case "Alfhlyra-01"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "Alfhlyra-02"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "Alfhlyra-03"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-01"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-02"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-03"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-04"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-05"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-06"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "HendonMyre-07"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast-01"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast-02"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast-03"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast-04"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "WestCoast-05"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
            case "MoonlightTavern"://
                {
                    playerPositionX = 0;
                    playerPositionY = 0;
                    nearScenerPositionY = 0;
                    farScenerPositionY = 0;
                    SceneryIndex = 0;
                    break;
                }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        HpMax = 99;//最大生命值
        HpCurrent = 50;//当前生命值
        MpMax = 99;//最大法力值
        MpCurrent = 50;//当前法力值
        AttackPoint = 10;//攻击力
        DefensePoint = 10;//防御力
        BeHitPoint = 0;//被击值
        HurtPoint = 0;//受伤害
        MoveSpeed = 3;//移动速度

        i = 0;//通用数组下标变量
        invincibleTimer = 0;  //初始计时器    
        //lookDirection = new Vector2(1, 0);                //赋值默认人物朝向右方
        anim = transform.GetComponentInChildren<Animator>();  //赋值动画组件
                                                              //赋值角色刚体组件
        

        //设置角色初始位置


        //if (UITextManager.instance.a == "SeriaRoom" && Application.loadedLevelName == "Elvenguard")
        //{
        //    playerRbody.position = new Vector2(5, 0);
        //}
        //if (UITextManager.instance.a == "SeriaRoom" && Application.loadedLevelName == "Elvenguard")
        //{
        //    playerRbody.position = new Vector2(5, 0);
        //}
        //if (UITextManager.instance.a == "SeriaRoom" && Application.loadedLevelName == "Elvenguard")
        //{
        //    playerRbody.position = new Vector2(5, 0);
        //}


        //实例化近景图对象
        nearLandscape = Instantiate(nearLandscapePrefab[SceneryIndex], playerRbody.position, Quaternion.identity);
        //获取近景图对象刚体
        nearLandscapeRbody = nearLandscape.GetComponent<Rigidbody2D>();
        //设置近景图初始位置
        nearLandscapeRbody.position = new Vector2(playerRbody.position.x, nearScenerPositionY);
        //实例化远景图对象
        farLandscape = Instantiate(farLandscapePrefab[SceneryIndex], playerRbody.position, Quaternion.identity);
        //获取远景图对象刚体
        farLandscapeRbody = farLandscape.GetComponent<Rigidbody2D>();
        //设置远景图初始位置
        farLandscapeRbody.position = new Vector2(playerRbody.position.x, farScenerPositionY);





        moveDirection = Vector2.right;//默认朝右
        faceDirectionf = FaceDirection.right;//默认朝右
        UImanager.instance.UpdateHealthBar(HpCurrent, HpMax);//更新血条
        UImanager.instance.UpdateHealthPoint(HpCurrent, HpMax);//更新生命值显示
        UImanager.instance.UpdateMagicBar(MpCurrent, MpMax);//更新蓝条
        UImanager.instance.UpdateMagicPoint(MpCurrent, MpMax);//更新法力值显示

        //DontDestroyOnLoad(this.gameObject);//加载新场景的时候使目标物体不被自动销毁。


        //UITextManager.instance.SetText("111");

        //UITextManager.instance.x.text = "112";//当前场景名存储
        // UITextManager.instance.y.text = UITextManager.instance.a.text;


        
    }

    //public void playerPosition(float x, float y)
    //{
    //    playerRbody.position = new Vector2(x, y);
    //}
    // Update is called once per frame
    void Update()
    {
        //float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        //float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标
        //Vector2 moveVector = new Vector2(moveX, moveY);//移动向量
        

        MoveSpeed = testValue2;
      //  gameMessage.text = Time.deltaTime+"\n"+AttackPoint + "\n"+playerRbody.position.ToString() + "\n" + moveDirection.ToString() + "\n" + playerRbody.position.magnitude+"\n"+ i.ToString()+"\n"+ test3.ToString();
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
        //=====邪光斩
        if (Input.GetKeyDown(KeyCode.S)&&MpCurrent>0)  //确认按下技能键
        {
            ChangeMagic(-1);//每次攻击消耗一点法力值
            if (isClipPlayingSkill == false)
            {
                GameObject bullet = Instantiate(bulletPrefab, playerRbody.position, Quaternion.identity);  //赋值技能对象
                BulletController bc = bullet.GetComponent<BulletController>();  //获取技能对象
                if (bc != null)  //技能对象不为空
                {
                    bc.Move(moveDirection, testValue1);  //技能移动 (朝向,速度)
                }
                StartCoroutine(PlaySkillClip());  //开始释放技能
            }      
        }
        //=====普通攻击
        if (Input.GetKey(KeyCode.X))  //确认按下X键开始普通攻击
        {
            BlendTreeSetFloat();//重置混合树
            anim.SetBool("isNormalAttack", true);//切换角色动作动画
            StartCoroutine(PlayNormalAttackPlayerClip());//普通攻击角色语音
            switch (faceDirectionf)//判断脸的朝向
            {
                case FaceDirection.right:  //脸朝向右
                    {
                        anim.SetFloat("RightNormalAttack", 1);
                        faceDirectionf = FaceDirection.right;//脸朝向右
                        break;
                    }
                case FaceDirection.left://脸朝向左
                    {
                        anim.SetFloat("LeftNormalAttack", 1);
                        faceDirectionf = FaceDirection.left;//脸朝向右
                        break;
                    }
            }
           // StartCoroutine(PlayNormalAttackClip());//播放普通攻击音效
        }
        else
        {
            anim.SetBool("isNormalAttack", false);  //松开X按键后停止普通攻击
        }
        //====按下Enter回车键 进行NPC交互
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit2D hit = Physics2D.Raycast(playerRbody.position, moveDirection, 2f, LayerMask.GetMask("NPC"));
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
    /// 播放普通攻击音效
    /// </summary>
    /// <returns>音效播放时间长度</returns>
    IEnumerator PlayNormalAttackClip()
    {
        //音效未处于播放中，才允许播放
        if (isClipPlayingNormalAttack == false)
        {
            AudioManager.instance.AudioPlay(_NormalAttackHitClip);//移动音效
            isClipPlayingNormalAttack = true; //音效播放中
            yield return new WaitForSeconds(_NormalAttackHitClip.length);  //播放完
            isClipPlayingNormalAttack = false;//音效处于未播放状态
        }
    }
    /// <summary>
    /// 播放普通攻击玩家语音
    /// </summary>
    /// <returns>音效播放时间长度</returns>
    IEnumerator PlayNormalAttackPlayerClip()
    {
        //音效未处于播放中，才允许播放
        if (isClipPlayingNormalAttackPlayer == false)
        {
            AudioManager.instance.AudioPlay(_NormalAttackPlayerClip[i]);//播放音效
            isClipPlayingNormalAttackPlayer = true; //音效播放中
            yield return new WaitForSeconds(_NormalAttackPlayerClip[i].length);  //音效播放完
            isClipPlayingNormalAttackPlayer = false;//音效处于未播放状态
            //普通攻击音效数组轮流播放
            if (i >= _NormalAttackPlayerClip.Length-1)
            {
                i = 0;
            }
            else
            {
                i++;
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
        if (isClipPlayingRun == false)
        {
            AudioManager.instance.AudioPlay(_RunClip);//播放移动音效
            isClipPlayingRun = true; //移动音效播放中
            yield return new WaitForSeconds(_RunClip.length);  //播放完
            isClipPlayingRun = false;//移动音效处于未播放状态
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
        if (isClipPlayingSkill == false)
        {
            BlendTreeSetFloat();//重置混合树
            anim.SetTrigger("Launch");//切换人物技能动作动画
            AudioManager.instance.AudioPlay(_LaunchClip);//播放技能音效
            isClipPlayingSkill = true; //技能音效播放中
            yield return new WaitForSeconds(1);  //技能冷却时间
            isClipPlayingSkill = false;//技能处于未释放状态
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    void Move()
    {
        Scenery();//风景跟随玩家移动

        float moveX = Input.GetAxisRaw("Horizontal");  //水平坐标
        float moveY = Input.GetAxisRaw("Vertical");  //垂直坐标
        

        Vector2 moveVector = new Vector2(moveX, moveY);//移动向量
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            moveDirection = moveVector;//赋值移动方向

            StartCoroutine(PlayRunClip());//播放移动音效
            
        }
        anim.SetFloat("Speed", moveVector.magnitude);//传递移动速度
        //角色位置移动
        Vector2 position = playerRbody.position;                //获取角色位置
        position += moveVector * MoveSpeed * Time.deltaTime;  //更新角色位置
        playerRbody.MovePosition(position);                     //移动刚体位置
        




        //动画控制
        if (moveVector.x * MoveSpeed * Time.deltaTime > 0)
        {
            //向右走
            BlendTreeSetFloat();//重置混合树
            anim.SetFloat("RightRun", 1);//播放向右跑动画
            faceDirectionf = FaceDirection.right;//脸朝向右
            
        }
        else if (moveVector.x * MoveSpeed * Time.deltaTime < 0)//向左走
        {
            BlendTreeSetFloat();
            anim.SetFloat("LeftRun", 1);
            faceDirectionf = FaceDirection.left;
        }
        else if (moveVector.y * MoveSpeed * Time.deltaTime > 0) //向上走
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
        else if (moveVector.y * MoveSpeed * Time.deltaTime < 0)
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



        //==============
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
            AudioManager.instance.AudioPlay(_HitClip);//播放受伤音效
            invincibleTimer = invincibleTime;
        }

        HpCurrent = Mathf.Clamp(HpCurrent + amount, 0, HpMax);//把玩家的生命值约束在0和最大值之间
        UImanager.instance.UpdateHealthBar(HpCurrent, HpMax);//更新血条
        UImanager.instance.UpdateHealthPoint(HpCurrent, HpMax);//更新生命值显示
    }

    /// <summary>
    /// 改变玩家的法力值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMagic(int amount)
    {
        MpCurrent = Mathf.Clamp(MpCurrent + amount, 0, MpMax);//把玩家的法力值约束在0和最大值之间
        UImanager.instance.UpdateMagicBar(MpCurrent, MpMax);//更新蓝条
        UImanager.instance.UpdateMagicPoint(MpCurrent, MpMax);//更新法力值显示
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
        anim.SetFloat("RightNormalAttack", 0);
        anim.SetFloat("LeftNormalAttack", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController enemy = collision.gameObject.GetComponent<MonsterController>();
        if (enemy != null)
        {
            AudioManager.instance.AudioPlay(_NormalAttackHitClip);//普通攻击打击音效
            enemy.Fixed(AttackPoint);
        }
        test3 = collision.gameObject.name;

        //if (collision.gameObject.name== "Alfhlyra-01")
        //{
        //    if (Application.loadedLevelName == "Alfhlyra")//关于这个下面有详细介绍  
        //    {
        //        Application.LoadLevel(collision.gameObject.name);
        //    }
        //    else
        //    {
        //        Application.LoadLevel("Alfhlyra");
        //    }
        //}
        //if (collision.gameObject.name == "SeriaRoom")
        //{
        //    if (Application.loadedLevelName == "SeriaRoom")//关于这个下面有详细介绍  
        //    {
        //        Application.LoadLevel(collision.gameObject.name);
        //    }
        //    else
        //    {
        //        Application.LoadLevel("SeriaRoom");
        //    }
        //}
    }

    /// <summary>
    /// 背景图跟随角色移动
    /// </summary>
    /// <param name="moveVector">移动向量</param>
    public void Scenery()
    {
        //近景图跟随玩家移动
        Vector2 nearLandscapePosition = nearLandscapeRbody.position;
        //近景图的X轴位置等于玩家X轴的1/2
        nearLandscapePosition.x = this.playerRbody.position.x / 10;
        //更新近景图位置
        nearLandscapeRbody.MovePosition(nearLandscapePosition);

        //远景图跟随玩家移动
        Vector2 farLandscapePosition = farLandscapeRbody.position;
        //远景图的X轴位置等于玩家X轴的1/3
        farLandscapePosition.x = this.playerRbody.position.x / 3;
        //更新远景图位置
        farLandscapeRbody.MovePosition(farLandscapePosition);
    }











}
