using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);//加载新场景的时候使目标物体不被自动销毁。
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
