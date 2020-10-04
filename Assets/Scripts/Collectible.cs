using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 精灵被玩家碰撞时检测的相关类
/// </summary>
public class Collectible : MonoBehaviour
{
    public string FairyName;  //精灵名称
    private Animator myAnimator;     //动画组件

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = transform.GetComponent<Animator>();  //赋值动画组件
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
            switch (FairyName)  //根据相应道具 获得对应效果
            {
                case "RedFairy":  //红精灵
                    {
                        if (pc.CurrentHealth < pc.MaxHealth)  //玩家血量不可以超过最大值
                        {
                            pc.ChangeHealth(1);  //血量+1
                            StartCoroutine(PlayAndLog());  //吃精灵
                        }
                        break;
                    }
                case "BlueFairy":  //蓝精灵
                    {
                        if (pc.CurrentMagic < pc.MaxMagic)  //玩家蓝量不可以超过最大值
                        {
                            pc.ChangeMagic(1);  //蓝量+1
                            StartCoroutine(PlayAndLog());  //吃精灵
                        }
                        break;
                    }
                case "GreenFairy":  //绿精灵
                    {
                        if (pc.CurrentHealth < pc.MaxHealth)  //玩家血量不可以超过最大值
                        {
                            pc.ChangeHealth(1);  //血和蓝都+1
                            pc.ChangeMagic(1);
                            StartCoroutine(PlayAndLog());  //吃精灵
                        }
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// 播放完动画再销毁对象
    /// </summary>
    /// <returns>延迟时间</returns>
    IEnumerator PlayAndLog()
    {
        switch (FairyName)  //根据相应道具 获得对应效果
        {
            case "RedFairy":  //红精灵
                {
                    myAnimator.SetBool("isRedFairyVanishing", true);  //切消失精灵动画
                    break;
                }
            case "BlueFairy":  //蓝精灵
                {
                    myAnimator.SetBool("isBlueFairyVanishing", true);  //切消失精灵动画
                    break;
                }
            case "GreenFairy":  //绿精灵
                {
                    myAnimator.SetBool("isGreenFairyVanishing", true);  //切消失精灵动画
                    break;
                }
        }
        yield return new WaitForSeconds(1f);  //播放1秒
        Destroy(this.gameObject);  //销毁对象
    }

}
