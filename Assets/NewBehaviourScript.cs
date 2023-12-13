using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        var w = Camera.main.aspect * 720f;
        
        Screen.SetResolution((int)w, 720,true,60);
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
