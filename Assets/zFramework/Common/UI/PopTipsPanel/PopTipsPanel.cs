using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopTipsPanel : MonoBehaviour
{
    public string Message { set => Show(value); }
    CanvasGroup canvasGroup;
    Text text;
    Sequence sequence;
    [SerializeField] float min = 0;
    [SerializeField] float max = 1;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if (canvasGroup && text) return;
        text = GetComponentInChildren<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    void Show(string msg)
    {
        if (string.IsNullOrEmpty(msg)) return;
        Init();
        text.text = msg;
        if (null != sequence && sequence.IsPlaying())
        {
            sequence.Kill();
        }
        canvasGroup.alpha = 0;
        var s = DOTween.Sequence();
        s.Append(canvasGroup.DOFade(max, 0.6f));
        s.Append(canvasGroup.DOFade(min, 0.6f).SetDelay(3));
    }
}
