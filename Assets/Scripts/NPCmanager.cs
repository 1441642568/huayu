using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// NPC交互相关
/// </summary>
public class NPCmanager : MonoBehaviour
{
    public GameObject tipImage;//按键提示

    public GameObject dialogImage;//对话

    public float showTime = 4;//对话框显示时间

    public float showTimer;//对话框显示计时器

    //=====NPC的台词音效
    public AudioClip npcClip;//NPC台词音效

    private bool isNPCClipPlaying;            //NPC音效是否播放中

    // Start is called before the first frame update
    void Start()
    {
        tipImage.SetActive(true);//初始默认显示提示键
        dialogImage.SetActive(false);//初始默认隐藏对话框
        showTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        showTimer -= Time.deltaTime;
        if (showTimer<0)
        {
            tipImage.SetActive(true);
            dialogImage.SetActive(false);
        }
    }
    /// <summary>
    /// 显示对话框
    /// </summary>
    public void ShowDialog()
    {
        showTimer = showTime;
        tipImage.SetActive(false);//关闭提示框
        dialogImage.SetActive(true);//打开对话框
        StartCoroutine(PlayNPCClip());  //开始NPC交互
    }
    /// <summary>
    /// 播放NPC音效
    /// </summary>
    /// <returns>音效播放时间长度</returns>
    IEnumerator PlayNPCClip()
    {
        //NPC音效未处于播放中，才允许播放
        if (isNPCClipPlaying == false)
        {

            AudioManager.instance.AudioPlay(npcClip);//播放NPC音效
            isNPCClipPlaying = true; //NPC音效播放中
            yield return new WaitForSeconds(npcClip.length);  //播放完
            isNPCClipPlaying = false;//NPC音效处于未播放状态
        }
    }
}
