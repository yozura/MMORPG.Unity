using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // 싱글턴 패턴
    static Managers s_instance; // 유일성이 보장된다.
    public static Managers Instance { get { init(); return s_instance; } } // 유일한 매니저를 가져온다.

    void Start()
    {
        init();
    }

    void Update()
    {
        
    }

    static void init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }

    }
}
