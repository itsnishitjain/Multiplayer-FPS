using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    private float slowMo = 0.1f;
    private float normTime = 1f;
   
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.T))
        {
            Time.timeScale = normTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = slowMo;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}
