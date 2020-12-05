using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    //单例模式
    public static MonsterUI instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    [Header("血条图片数组")]
    public Sprite[] _HpBarSprite;
    [Header("当前层血条")]
    public Image _CurretHpBar;
    [Header("下一层血条")]
    public Image _NextHpBar;
    [Header("血条白底")]
    public Image _HpBarBottom;

    [Header("剩余血条数")]
    public int _SurplusHpBar;
    [Header("总计血条数")]
    public int _CountHpBar;

    [Header("剩余血条数")]
    public Text _HpBarCountText;
    [Header("血量文字显示")]
    public Text _HealthPointText;
    
    /// <summary>
    /// 更新血条
    /// </summary>
    /// <param name="curAmount">HP当前值</param>
    /// <param name="maxAmount">HP最大值</param>
    public void UpdateHealthBar(int curAmount, int maxAmount)
    {
        _CurretHpBar.fillAmount = (float)curAmount % 100 / 100;//当前层血条的值
        _SurplusHpBar = curAmount / 100;//剩余血条数
        _CountHpBar = maxAmount / 100;//总计血条数

        //_CurretHpBar.sprite = _HpBarSprite[_SurplusHpBar % 4 + 1];//当前层血条
        //_NextHpBar.sprite = _HpBarSprite[_SurplusHpBar % 4];//下一层血条

        //当前血条颜色
        if (_SurplusHpBar == 0 || _SurplusHpBar + 1 == _CountHpBar)
        {
            _CurretHpBar.sprite = _HpBarSprite[4]; //第一条和最后一条血为红色
        }
        else
        {
            _CurretHpBar.sprite = _HpBarSprite[_SurplusHpBar % 4];//当前层血条
        }
        //下一层血条颜色
        if (_SurplusHpBar == 0)
        {
            _NextHpBar.sprite = _HpBarSprite[5];//最后一条血透明
        }
        else
        {
            if (_SurplusHpBar % 4 - 1 == -1)
            {
                _NextHpBar.sprite = _HpBarSprite[3];//下一层血条
            }
            else
            {
                _NextHpBar.sprite = _HpBarSprite[_SurplusHpBar % 4 - 1];//下一层血条
            }
            
        }

    }
    /// <summary>
    /// 更新血条白底
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateHealthBottom(int curAmount, int maxAmount)
    {
      //  _HealthBottom.fillAmount = (float)curAmount / (float)maxAmount;
        _HpBarBottom.fillAmount = (float)curAmount % 100 / 100 ;
    }
    /// <summary>
    /// 更新生命值显示
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateHealthPoint(int curAmount, int maxAmount)
    {
        _HealthPointText.text = curAmount.ToString() + " / " + maxAmount.ToString()+"   剩余血条："+ _SurplusHpBar.ToString()+" /总计血条:"+ _CountHpBar.ToString();
    }
}
