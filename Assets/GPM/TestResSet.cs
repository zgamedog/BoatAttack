using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        var r = Camera.main.aspect;
        var h = 720;
        var w = (int)(h * r);
        Screen.SetResolution(  w,h,true);

        Application.targetFrameRate = 60;
    }   

    
    public GameObject Toggle1;

    public GameObject Toggle2;

    public GameObject Toggle3;

    public GameObject Toggle4;

    public GameObject Toggle5;

    float h =720;
    public void ResDown()
    {   
        h *= 0.9f;
        var r = Camera.main.aspect;
        var w = (int)(h * r);
        Screen.SetResolution(w, (int)h, true);
    }   

    public void TogGo1() 
    {   
        Toggle1.SetActive(!Toggle1.activeSelf);
    }

    public void TogGo2()
    {
        Toggle2.SetActive(!Toggle2.activeSelf);
    }

    public void TogGo3()
    {   
        Toggle3.SetActive(!Toggle3.activeSelf);
    }

    public void TogGo4()
    {
        Toggle4.SetActive(!Toggle4.activeSelf);
    }

    public void TogGo5()
    {
        Toggle5.SetActive(!Toggle5.activeSelf);
    }
}
