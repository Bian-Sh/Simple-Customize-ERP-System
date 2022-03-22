﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Utilities/HUDFPS")]
public class FPSCounter : MonoBehaviour
{
    //fps 显示的初始位置和大小  
    public Rect startRect = new Rect(512, 10f, 75f, 50f);
    //fps 过低时是否改变UI颜色  
    public bool updateColor = true;
    //fps UI 是否允许拖动   
    public bool allowDrag = true;
    //fps 更新的频率  
    public float frequency = 0.5F;
    //fps 显示的精度  
    public int nbDecimal = 1;
    //一定时间内的fps数量  
    private float accum = 0f;
    //fps计算的时间  
    private int frames = 0;
    //GUI 依赖fps的颜色 fps<10 红色  fps<30 黄色 fps>=30 绿色  
    private Color color = Color.white;
    //fps   
    private string sFPS = "";
    //GUI 的样式  
    private GUIStyle style;

    void Start()
    {
        StartCoroutine(FPS());
    }

    void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }

    IEnumerator FPS()
    {
        while (true)
        {
            //更新fps  
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));

            //更新颜色  
            color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.yellow : Color.red);

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSeconds(frequency);
        }
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.fontSize=24;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = updateColor ? color : Color.white;
        startRect = GUI.Window(0, startRect, DoMyWindow, "");
    }

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(0, 0, startRect.width, startRect.height), sFPS + " FPS", style);
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}