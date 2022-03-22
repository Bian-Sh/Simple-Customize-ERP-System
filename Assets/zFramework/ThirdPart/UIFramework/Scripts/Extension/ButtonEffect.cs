using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField]float normalSize= 1;//按键正常大小
    [SerializeField]float distSize= 1.2f;//按键选中大小
    [SerializeField]float duration= 0.8f;//按键选中大小
    [SerializeField] GameObject titleImageObj;//标题图片的显示
    Vector2 orignrectsize;
    Tweener tweener;
    // Use this for initialization
    void Start()
    {
        rect =transform as RectTransform;
        orignrectsize = rect.sizeDelta;

    }
    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (titleImageObj != null)        
            titleImageObj.SetActive(true); 
        if (null != tweener)        
            tweener.Kill();        
        tweener= rect.DOSizeDelta(orignrectsize*distSize, duration);
    }
    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (titleImageObj != null)        
            titleImageObj.SetActive(false);   
        if (null != tweener)        
            tweener.Kill();        
        tweener = rect.DOSizeDelta(orignrectsize*normalSize, duration);
    }

  
	
}
