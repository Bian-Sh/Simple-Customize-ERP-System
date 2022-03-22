using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Fade : MonoBehaviour {
    Image image;
  
    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }	
}
