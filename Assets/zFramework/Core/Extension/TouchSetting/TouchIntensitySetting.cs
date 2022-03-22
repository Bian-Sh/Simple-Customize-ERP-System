using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TouchIntensitySetting : MonoBehaviour
{
    public GameObject[] set;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject item in set)
            {
                item.SetActive(!item.activeSelf);
            }
        }
    }


   
}
