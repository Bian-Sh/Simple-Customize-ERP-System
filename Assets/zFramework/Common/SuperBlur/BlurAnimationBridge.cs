using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperBlurring;
/// <summary>
/// 基于松耦合理念下的模糊组件动画与Image联动桥接器
/// </summary>
public class BlurAnimationBridge : MonoBehaviour
{

    [SerializeField]
    private SuperBlur superBlur;
    private Image blurFacede;
    public TransitionManager transitionManager;
    private void Awake()
    {
        blurFacede = GetComponent<Image>();
        blurFacede.enabled = true;
    }

    void Start()
    {
       transitionManager.OnTransitionUpdate.AddListener(v =>
        {
            superBlur.Interaction = v;
            if (v == 0)
            {
                blurFacede.enabled = false;
            }
        });
    }
}
