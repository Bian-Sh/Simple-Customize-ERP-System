using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBlocker : MonoBehaviour //target 默认的必须先是隐藏的
{
    public int blockerOrder = 0;
    [SerializeField]
    private Canvas rootCanvas; //必须指定
    private Canvas innercanvas;
    private GraphicRaycaster raycaster;
    private GameObject blocker;
    [SerializeField] private bool blockOnEnable = true; //适用于 通过SetActive()逻辑显示隐藏UI的时候
    public UnityEvent OnDisBlocked = new UnityEvent();
    private void Awake()
    {
        List<Canvas> list = new List<Canvas>();
        gameObject.GetComponentsInParent<Canvas>(false, list);
        if (list.Count!=0)
        {
            rootCanvas = list[list.Count - 1];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].isRootCanvas)
                {
                    rootCanvas = list[i];
                    break;
                }
            }
        }
    }
    private void OnEnable()
    {
        if (blockOnEnable)
        {
            Block();
        }
    }
    public void Block()
    {
        if (!innercanvas && rootCanvas) //如果没有Block过且父级画布存在
        {
            blocker = new GameObject("Blocker");
            RectTransform rectTransform = blocker.AddComponent<RectTransform>();
            rectTransform.SetParent(rootCanvas.transform, false);
            rectTransform.anchorMin = Vector3.zero;
            rectTransform.anchorMax = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            Canvas canvas = blocker.AddComponent<Canvas>();
            blocker.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;

            innercanvas = gameObject.AddComponent<Canvas>();
            innercanvas.overrideSorting = true;
            innercanvas.sortingOrder = 25000 + blockerOrder; //blockerOrder 用于存在多个blocker时微调他们的层级
            raycaster = gameObject.AddComponent<GraphicRaycaster>();

            canvas.sortingLayerID = innercanvas.sortingLayerID;
            canvas.sortingOrder = innercanvas.sortingOrder - 1;
            Image image = blocker.AddComponent<Image>();
            image.color = Color.clear;
            Button button = blocker.AddComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
            blocker.hideFlags = HideFlags.HideInHierarchy;
        }
        else
        {
            Debug.Log($"rootCanvas=null  {null == rootCanvas} innercanvas = null {null == innercanvas}");
        }
    }

    private void OnButtonClicked()
    {
        //Debug.Log("UIBlocker Clicked");
        DestoryBlocker();
        OnDisBlocked.Invoke();
    }
    private void OnDisable()
    {
        if (raycaster) Destroy(raycaster); //销毁的顺序必须的要对，否则：Can't remove Canvas because GraphicRaycaster (Script) depends on it
        if (innercanvas) Destroy(innercanvas);
        if (blocker) Destroy(blocker);
    }
    public void DestoryBlocker()
    {
        DestroyImmediate(raycaster); //销毁的顺序必须的要对，否则：Can't remove Canvas because GraphicRaycaster (Script) depends on it
        DestroyImmediate(innercanvas);
        DestroyImmediate(blocker);
    }
}
