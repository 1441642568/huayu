using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    //单例模式
    public static UImanager instance { get; private set; }

     void Awake()
    {
        instance = this;
    }

    public Image healthBar;//角色的血条
    public Image magicBar;//角色的蓝条
    public Text HealthPointText;//生命值显示
    public Text MagicPointText;//法力值显示
    /// <summary>
    /// 更新血条
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateHealthBar(int curAmount, int maxAmount)
    {
        healthBar.fillAmount = (float)curAmount / (float)maxAmount;
    }
    /// <summary>
    /// 更新蓝条
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateMagicBar(int curAmount, int maxAmount)
    {
        magicBar.fillAmount = (float)curAmount / (float)maxAmount;
    }
    /// <summary>
    /// 更新生命值显示
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateHealthPoint(int curAmount, int maxAmount)
    {
        HealthPointText.text = curAmount.ToString() + " / " + maxAmount.ToString();
    }
    /// <summary>
    /// 更新法力值显示
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateMagicPoint(int curAmount,int maxAmount)
    {
        MagicPointText.text = curAmount.ToString() + " / " + maxAmount.ToString();
    }
}
