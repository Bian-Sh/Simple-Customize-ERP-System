using BenYuan.UI.Tips;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 场景中停靠类的加载器
/// 作用：
/// 生成，放置，显示指定类型的tips
/// </summary>
public class StickedTipsLoader : MonoSingleton<StickedTipsLoader>
{
    public Transform tipsContainer;
    private string tipsContainerPath = @"CanvasUI/TipsContainer";

    public override void OnInit()
    {
        if (!tipsContainer)
        {
            tipsContainer = GameObject.Find(tipsContainerPath)?.transform;
        }
        if (!tipsContainer)
        {
            tipsContainer = transform;
        }
    }
    public static T AllocateTips<T>() where T : BaseTips
    {
        BaseTips.PathAttribute attr = typeof(T).GetCustomAttribute<BaseTips.PathAttribute>();
        var arr = attr.prefab.Split(new string[2] { "/Resources/", ".prefab" }, StringSplitOptions.RemoveEmptyEntries);
        if (arr.Length==2)
        {
            T t = Resources.Load<T>(arr[1]);
            if (t)
            {
                T tips = Instantiate(t);
                tips.transform.SetParent(Instance.tipsContainer, false);
                return tips;
            }
            else
            {
                Debug.LogError($"给定的预制体路径找不到指定文件：{attr.prefab}");
            }
        }
        else
        {
            Debug.LogError($"给定的预制体路径格式错误，可能不是预制体或者不在Resources 文件夹下：{attr.prefab}");
        }
        return null;
    }
}
