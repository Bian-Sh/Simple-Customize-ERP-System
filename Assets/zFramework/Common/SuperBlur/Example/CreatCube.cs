using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreatCube : MonoBehaviour
{
    Slider slider;
    Button button;
    public GameObject instanceOne;
    private Coroutine coroutine;
    void Start()
    {
        slider = FindObjectOfType<Slider>();
        button = FindObjectOfType<Button>();
      
        button.onClick.AddListener(() =>
        {
            if (null == coroutine)
            {
                coroutine = StartCoroutine(DoFade());
            }
        });
    }

    public float factor = 1; //时间缩放因子，决定了 PingPong的周期
    private IEnumerator DoFade()
    {
        float progress = 0;
        bool addition = true;
        bool finish = false;
        float duration = Time.time;
        while (!finish)
        {
            progress += (addition ? 1 : -1) * Time.deltaTime *(2/factor) ;
            progress = Mathf.Clamp01(progress);
            //嵌套三目运算做状态反转
            addition = progress == 1 ? false : progress == 0 ? true : addition;
            //驱动订阅者
            slider.value = progress;
            finish = progress == 0;
            yield return null;
        }
        Debug.Log(Time.time - duration);
        coroutine = null;
    }

}
