using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人控制相关
/// </summary>
public class MonsterController : MonoBehaviour
{
    [Header("最大生命值")]
    public int _HpMax;       //最大生命值
    [Header("当前生命值")]
    public int _HpCurrent;   //当前生命值
    [Header("攻击力")]
    public int _AttackPoint;//攻击力
    [Header("防御力")]
    public int _DefensePoint;//防御力
    [Header("移动速度")]
    public int _MoveSpeed;     //移动速度
    [Header("是否垂直方向移动")]
    public bool isVertical;
    [Header("被击音效")]
    public AudioClip _BeHitClip;
    [Header("死亡音效")]
    public AudioClip _DieClip;
    [Header("血条UI")]
    public GameObject _HpBarUI;

    private int _BeHitPoint;//被击者的攻击力
    private int _HurtPoint;//受到的伤害
    public int BeHitPoint { get => _BeHitPoint; set => _BeHitPoint = value; }
    public int HurtPoint { get => _HurtPoint; set => _HurtPoint = value; }

    public float changeDirectionTime = 2f;//改变方向的时间

    private float changeTimer;//改变方向的计时器



    private Vector2 moveDirection;//移动方向

    private Rigidbody2D rbody;//刚体组件

    private Animator anim;//动画组件

    private bool isFixed;//是否被修复

    public ParticleSystem brokenEffect;//损坏特效
    
    enum FaceDirection { left, right }; //脸的朝向

    FaceDirection faceDirectionf;//脸的朝向

    public float force = 30;

    // Start is called before the first frame update
    void Start()
    {
        //_HpMax = 99;//最大生命值
        //_HpCurrent = 50;//当前生命值
        //_AttackPoint = 10;//攻击力
        //_DefensePoint = 0;//防御力
        //_MoveSpeed = 1;//移动速度
        BeHitPoint = 0;//被击值
        HurtPoint = 0;//受伤害
        

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
        //anim.SetTrigger("isByAttack");
        //  anim.SetFloat("byAttack", 0);
        if (isFixed) return;//如果被修复了，就不执行下面代码
        changeTimer -= Time.deltaTime;
        if (changeTimer<0)
        {
            moveDirection *= -1;
            changeTimer = changeDirectionTime;
        }
        Vector2 position = rbody.position;
        position.x += moveDirection.x * _MoveSpeed * Time.deltaTime;
        position.y += moveDirection.y * _MoveSpeed * Time.deltaTime;
        rbody.MovePosition(position);

        //动画控制
        if (moveDirection.x * _MoveSpeed * Time.deltaTime > 0)
        {
            //向右走
            anim.SetFloat("walk_right", 1);
            anim.SetFloat("walk_left", 0);
            anim.SetFloat("idle", 0);
            faceDirectionf = FaceDirection.right;//脸朝向右
        }
        else if (moveDirection.x * _MoveSpeed * Time.deltaTime < 0)
        {
            //向左走
            anim.SetFloat("walk_left", 1);
            anim.SetFloat("walk_right", 0);
            anim.SetFloat("idle", 0);
            faceDirectionf = FaceDirection.left;
        }
        else if (moveDirection.y * _MoveSpeed * Time.deltaTime > 0) //向上走
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
        else if (moveDirection.y * _MoveSpeed * Time.deltaTime < 0)
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
            pc.ChangeHealth(-1);//受到的伤害
        }
    }

    /// <summary>
    /// 敌人被击
    /// </summary>
    public void Fixed(int AttackPoint)
    {
        _HpBarUI.SetActive(true);//显示血条UI
        isFixed = true;//被击
        //if (brokenEffect.isPlaying == true)
        //{
        //    brokenEffect.Stop();//关闭特效
        //}
       // rbody.simulated = false;//禁用物理
        //重置动画混合树
        anim.SetFloat("walk_left", 0);
        anim.SetFloat("walk_right", 0);
        anim.SetFloat("idle", 0);



        anim.SetTrigger("isByAttack");//播放被击动画
        AudioManager.instance.AudioPlay(_BeHitClip);//普通攻击打击音效
        ChangeHealth(AttackPoint);//受到攻击 参数为攻击者的攻击力
        if (_HpCurrent<=0)
        {
            rbody.simulated = false;//禁用物理
            _HpBarUI.SetActive(false);//隐藏血条UI
            StartCoroutine(PlayDieAnimation());//播放怪物死亡动画
        }

        //HPUIManager.instance.OnClickHurtHP();//更新血条
    }
    /// <summary>
    /// 改变怪物的生命值
    /// </summary>
    /// <param name="AttackPoint">玩家攻击力</param>
    public void ChangeHealth(int AttackPoint)
    {
        HurtPoint = AttackPoint - _DefensePoint;//受到的攻击力 减去 怪物防御力 等于受到的伤害
        _HpCurrent = Mathf.Clamp(_HpCurrent - HurtPoint, 0, _HpMax);//把怪物的生命值约束在0和最大值之间
        MonsterUI.instance.UpdateHealthBar(_HpCurrent, _HpMax);//更新血条
        MonsterUI.instance.UpdateHealthPoint(_HpCurrent, _HpMax);//更新生命值显示
        //MonsterUI.instance.UpdateHealthBottom(_HpCurrent, _HpMax);//更新血条白底显示
        StartCoroutine(UpdateHealthBottom());//更新血条白底显示
    }
    /// <summary>
    /// 播放技能音效
    /// 释放技能 协同 等待 帧阻塞 停进程
    /// </summary>
    /// <returns>延迟时间</returns>
    IEnumerator UpdateHealthBottom()
    {
        yield return new WaitForSeconds(0.5f);  //白底更新间隔时间
        MonsterUI.instance.UpdateHealthBottom(_HpCurrent, _HpMax);//更新血条白底显示
    }
    /// <summary>
    /// 播放怪物死亡动画
    /// 释放技能 协同 等待 帧阻塞 停进程
    /// </summary>
    /// <returns>延迟时间</returns>
    IEnumerator PlayDieAnimation()
    {
        anim.SetTrigger("isDie");//播放怪物死亡动画
        yield return new WaitForSeconds(0.5f);  //等待时间
        AudioManager.instance.AudioPlay(_DieClip);//怪物死亡音效
        Destroy(this.gameObject);//怪物死亡销毁对象
    }
}
