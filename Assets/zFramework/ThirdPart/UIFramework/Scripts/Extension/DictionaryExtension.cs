using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对字典的扩展方法
/// </summary>
public static class DictionaryExtension
{
    /// <summary>
    /// 根据key 找到Valus
    /// </summary>
    public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;

    }

}
