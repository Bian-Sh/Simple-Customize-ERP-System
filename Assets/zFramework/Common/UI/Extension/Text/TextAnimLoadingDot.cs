using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAnimLoadingDot : MonoBehaviour
{
    Tweener tweener;
    public float duration = 1f;
    private Text text;
    public string msg;
    private void Awake()
    {
        text = GetComponent<Text>();
        msg = text.text;
    }
    void OnEnable()
    {
        string[] dots = { "   ", ".  ", ".. ", "...", };
        tweener = DOTween.To(() => 0, v => text.text = $"{msg}{dots[v]}", 3, 1).SetLoops(-1, LoopType.Restart);
    }
    void OnDisable()
    {
        tweener?.Kill();
        tweener = null;
    }
}
