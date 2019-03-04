using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Log;

public class CustomDebuger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.Log("Custom Log");
        this.LogWarning(" Custom LogWarning");
        this.LogError("Custom LogError");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
