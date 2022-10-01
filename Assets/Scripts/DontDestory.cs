using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : Singleton<DontDestory>
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }

}
