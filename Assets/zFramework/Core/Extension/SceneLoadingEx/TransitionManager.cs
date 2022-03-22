/*
 *  这个是一个场景过渡控制器，实现屏幕渐隐渐显 +  高斯模糊的逐步模糊效果。
 *  需要正确配置：
 *  1. MianCamera 需要配置  SuperBlur 脚本
 *  2. SuperBlur RenderMode 选 UI
 *  3. SuperBlur 下的Material 请分别指定对应的材质球（已经在脚本中指定好了）
 *  4. 本脚本依赖 Image 且 Image 请指定对应的材质球 （SuperBlurUI）
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class TransitionManager : MonoBehaviour
{
    private float duration; //Fade时长
    public UnityEvent OnTransitionStart = new UnityEvent();
    public UnityEvent OnTransitionMiddle = new UnityEvent();
    public UnityEvent OnTransitionFinish = new UnityEvent();
    public TransitionProgressEvent OnTransitionUpdate = new TransitionProgressEvent();
    [System.Serializable] public class TransitionProgressEvent : UnityEvent<float> { }
    private CustomYieldInstruction yieldInstruction; //中断指示
    private Coroutine coroutine;
    [Header("测试用户中断与Transition 的交互！")]
    public bool go;

    private bool ignoreFadeIn = false;
    private Image image;
    public float FadeValue
    {
        set
        {
            image.enabled = value != 0;
            //float v = 1 - value;
            Color color = image.color;
            color[3] = value;
            image.color = color;
        }
    }
    private void Awake()
    {
        image = GetComponent<Image>();


        image.enabled = false;
        yieldInstruction = new WaitUntil(() => go);  //This Is for a  test .放在 Awake 里好啊，作为默认值的设定，一旦有其他中断这个就能被及时顶掉。
    }


    private IEnumerator DoFade()
    {
        OnTransitionStart.Invoke();
        float progress = ignoreFadeIn ? 1 : 0;
        bool addition = true;
        bool finish = false;
        float totalduration = Time.realtimeSinceStartup;
        while (!finish)
        {
            progress += (addition ? 1 : -1) * Time.deltaTime * (2 / duration);
            progress = Mathf.Clamp01(progress);
            FadeValue = progress;
            if (progress == 1) Debug.Log("Middle Duration : " + (Time.realtimeSinceStartup - totalduration));
            OnTransitionUpdate.Invoke(progress);
            //嵌套三目运算做状态反转
            addition = progress == 1 ? false : progress == 0 ? true : addition;
            if (progress == 1)
            {
                Debug.Log("Transition Reached The Middle Progress !!");
                yield return yieldInstruction;
                Debug.Log("Custom Yield Instruction Finished  !!");
                OnTransitionMiddle.Invoke();
            }


            finish = progress == 0;
            yield return null;
        }
        Debug.Log("Total Duration : " + (Time.realtimeSinceStartup - totalduration));

        OnTransitionFinish.Invoke();
        coroutine = null;
    }


    public TransitionManager SetYieldInstruction(CustomYieldInstruction yieldInstruction)
    {
        this.yieldInstruction = yieldInstruction;
        return this;
    }

    /// <summary>
    /// 当屏幕达到最黑且 用户阻塞取消时执行的事件 (OnTransitionMiddle )
    /// </summary>
    /// <returns></returns>
    public TransitionManager AddStartEventListener(UnityAction action)
    {
        this.OnTransitionStart.AddListener(action);
        return this;
    }
    /// <summary>
    /// 当屏幕达到最黑且 用户阻塞取消时执行的事件 (OnTransitionMiddle )
    /// </summary>
    /// <returns></returns>
    public TransitionManager AddMiddleEventListener(UnityAction action)
    {
        this.OnTransitionMiddle.AddListener(action);
        return this;
    }
    /// <summary>
    /// 当屏幕达到最黑且 用户阻塞取消时执行的事件 (OnTransitionMiddle )
    /// </summary>
    /// <returns></returns>
    public TransitionManager AddFinishEventListener(UnityAction action)
    {
        this.OnTransitionFinish.AddListener(action);
        return this;
    }
    /// <summary>
    /// 设置是否跳过渐隐的过程
    /// </summary>
    /// <returns></returns>
    public TransitionManager IgnoreFadeIn(bool value = false)
    {
        this.ignoreFadeIn = value;
        return this;
    }

    public void Play(float duration)
    {
        this.duration = duration;
        if (null == coroutine)
        {
            coroutine = StartCoroutine(DoFade());
        }
    }

    [EditorButton]
    private void TestEmitEvent()
    {
        OnTransitionFinish.Invoke();
    }

    public void Init()
    {

    }

}
