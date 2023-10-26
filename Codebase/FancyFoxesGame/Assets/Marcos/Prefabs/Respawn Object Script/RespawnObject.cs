using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public GameObject obj;

    private int waitTime = 1000;
    private int timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (!this.obj.activeSelf)
        {
            if(timer == waitTime)
            {
                timer = 0;
            }
            if (timer < waitTime)
            {
                timer += 1;
            }
            if(timer == waitTime)
            {
                this.obj.SetActive(true);
            }
        }
    }
}
