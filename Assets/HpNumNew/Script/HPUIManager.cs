using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPUIManager : MonoBehaviour {
    //单例模式
    public static HPUIManager instance { get; private set; }
    [Header("当前血量显示名称")]
    public Text _CurretHpNumName;
    [Header("当前血量层名称")]
    public Text _CurretHpLayerName;
    [Header("输入当前血量显示名称")]
    public string _CurretHpNumNameString;
    [Header("输入当前血量层名称")]
    public string _CurretHpLayerNameString;
    [Header("当前血量显示值")]
    public Text _CurretHpNum;
    [Header("当前血量层值")]
    public Text _CurretHpLayer;
    [Header("当前血量图片")]
    public Image _CurretHpImage;
    [Header("空血底图")]
    public Image _LateHpImage;
    [Header("最大血量值")]
    public float _ResultHpNum;
    [Header("伤害值")]
    public float _HurtNum;
    [Header("失血速度")][Range(0,1)]
    public float _LoseHpSpeed = 0.1f;
    [Header("空血底色颜色")]
    public Color _NullHpColor;
    [Header("血条数量颜色")]
    public List<Color> _UIHpList;
    //剩余血量数
    private float _CurretHpNumString;
    //剩余血量条
    private float _CurretHpLayerString;
    //血量条数
    private int _UIHpListCount;
    //伤害血条比例
    private float _HurtHpNum;
    //当前受到伤害后血条比例
    private float _CurretHpHurt = 1;
    //临时存储最大生命值
    float _TempResultHpNum;
    //临时存储最大生命值
    float _ResultHpNumSum;
    //临时存储当前长度(第一行血)
    int _CurrteCount;
    //临时存储BG长度(第二行血)
    int _LateCount;
    public float CurretHpNumString
    {
        get{return _CurretHpNumString;}
        set{_CurretHpNumString = value;}
    }
    public float CurretHpLayerString
    {
        get{return _CurretHpLayerString;}
        set{_CurretHpLayerString = value;}
    }
    public int UIHpListCount
    {
        get{ return _UIHpListCount;}
        set{_UIHpListCount = value;}
    }
    public float HurtHpNum
    {
        get{return _HurtHpNum;}
        set{_HurtHpNum = value;}
    }
    public float CurretHpHurt
    {
        get{return _CurretHpHurt;}
        set{_CurretHpHurt = value;}
    }
    // Use this for initialization
    void Start ()
    {
        //显示文本文框的字(显示名称)
        _CurretHpNumName.text = _CurretHpNumNameString;
        //显示文本文框的字(显示名称)
        _CurretHpLayerName.text = _CurretHpLayerNameString;
        //记录血条数
        UIHpListCount = _UIHpList.Count;
        //开始存储最大值
        _TempResultHpNum = _ResultHpNum;
        //开始存储最大值
        _ResultHpNumSum = _ResultHpNum;
    }
	// Update is called once per frame
	void Update ()
    {
        //当前血量层数显示值
        _CurretHpLayer.text = _UIHpList.Count.ToString();
        //当前血量显示值
        _CurretHpNum.text = (_ResultHpNum = _TempResultHpNum).ToString();
        //计算出每条血量扣血比例 = 伤害值/(总血量/记录的血条数量);
        HurtHpNum = _HurtNum /(_ResultHpNumSum / UIHpListCount);
        //如果满足第一行血条比例长度大于受到伤害后血条比例 && 第一个子物体的名字不是当前血条名字(第一行血) &&当前颜色不是空血颜色 进行血条减少动画。
        if (_CurretHpImage.fillAmount > CurretHpHurt && _CurretHpImage.transform.parent.GetChild(0).name != _CurretHpImage.name && _CurretHpImage.color != _NullHpColor)
        {
            _CurretHpImage.fillAmount -= _LoseHpSpeed * Time.deltaTime;
        }
        //如果满足第二行血条比例长度大于受到伤害后血条比例 && 第一个子物体的名字不是当前血条名字(第二行血) &&当前颜色不是空血颜色 进行血条减少动画。
        else if (_LateHpImage.fillAmount > CurretHpHurt && _LateHpImage.transform.parent.GetChild(0).name != _LateHpImage.name && _LateHpImage.color != _NullHpColor)
        {
            _LateHpImage.fillAmount -= _LoseHpSpeed * Time.deltaTime;
        }
        //（记录最大血条层数 - 当前血条层数）的余数为0（此数为偶数）
        if ((UIHpListCount - _UIHpList.Count)%2 == 0 )
        {
            //当前血条层数不为0，修改二行血条的先后赋值集合的颜色索引
            if (_UIHpList.Count != 0)
            {
                _CurrteCount = 1;
                _LateCount = 2;
            }
        }
        //（此数为奇数）
        else
        {
            //当前血条层数不为0，修改二行血条的先后赋值集合的颜色索引
            if (_UIHpList.Count !=0)
            {
                _CurrteCount = 2;
                _LateCount = 1;
            }
        }
        //长度为0（结束血条的意思）
        if (_UIHpList.Count == 0)
        {
            //底色血条为空血条色（第二行）
            _LateHpImage.color = _NullHpColor;
            //隐藏（第一行）
            _CurretHpImage.gameObject.SetActive(false);
        }
        //判断（记录最大血条层数-当前血条层数）是否小于最大减1（防止索引溢出）
        if (UIHpListCount - _UIHpList.Count < UIHpListCount - 1)
        {
            //修改二行血条的颜色
            _CurretHpImage.color = _UIHpList[_UIHpList.Count - _CurrteCount];
            _LateHpImage.color = _UIHpList[_UIHpList.Count - _LateCount];
        }
        else
        {
            //防止索引溢出
            if (_UIHpList.Count !=0)
            {
                //判断（当前）颜色是否为0的索引颜色（第一行）
                if (_CurretHpImage.color == _UIHpList[0])
                {
                    //第二行为空血色
                    _LateHpImage.color = _NullHpColor;
                }
                //判断（稍后）颜色是否为0的索引颜色（第二行）
                else if (_LateHpImage.color == _UIHpList[0])
                {
                    //第一行为空血色
                    _CurretHpImage.color = _NullHpColor;
                }
            }
        }
        //满足第一行血条比例长度<=0 && 第一行颜色不是空血色
        if (_CurretHpImage.fillAmount <= 0 && _CurretHpImage.color != _NullHpColor)
        {
            //交换渲染位置
            _CurretHpImage.transform.SetSiblingIndex(0);
            //减少血条数索引
            _UIHpList.RemoveAt(_UIHpList.Count - 1);
            //重置血条比例长度
            _CurretHpImage.fillAmount = 1;
            //重置受伤血条比例长度
            CurretHpHurt = 1;
        }
        //满足第二行血条比例长度<=0 && 第二行颜色不是空血色
        else if (_LateHpImage.fillAmount <= 0 && _LateHpImage.color != _NullHpColor)
        {
            //交换渲染位置
            _LateHpImage.transform.SetSiblingIndex(0);
            //减少血条数索引
            _UIHpList.RemoveAt(_UIHpList.Count - 1);
            //重置血条比例长度
            _LateHpImage.fillAmount = 1;
            //重置受伤血条比例长度
            CurretHpHurt = 1;
        }


    }
    /// <summary>
    /// 点击失血判断事件
    /// </summary>
    public void OnClickHurtHP()
    {
        //限制点击判断防止血条减少BUG， 如果满足血条长度小于并且等于受到伤害后血条比例 && 防止索引溢出
        if (_CurretHpImage.fillAmount <= CurretHpHurt && _UIHpList.Count != 0)
        {
            //计算当前受到伤害后血条比例。
            CurretHpHurt = _CurretHpImage.fillAmount - HurtHpNum;
            //计算显示的血量
            _TempResultHpNum -= _HurtNum;
        }
        //限制点击判断防止血条减少BUG， 如果满足血条长度小于并且等于受到伤害后血条比例 && 防止索引溢出
        else if (_LateHpImage.fillAmount <= CurretHpHurt && _UIHpList.Count != 0)
        {
            //计算当前受到伤害后血条比例。
            CurretHpHurt = _LateHpImage.fillAmount - HurtHpNum;
            //计算显示的血量
            _TempResultHpNum -= _HurtNum;
        }
    }
    /// <summary>
    /// 四舍五入
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    int RoundingNum(float Num)
    {
        if (Num % 1 >= 0.5f)
        {
            Num += 1;
        }
        return (int)Num;
    }
}
