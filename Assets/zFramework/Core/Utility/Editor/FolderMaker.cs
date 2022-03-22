using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class FolderMaker
{
    /// <summary>
    /// 编辑器下使用，给定一个类型，在这个类的周边目录下创建文件夹并返回绝对路径
    /// </summary>
    /// <param name="script">MonoBehaviour 对象</param>
    /// <param name="subPath">指定要创建的文件夹的名称</param>
    /// <param name="local">指定获取相对路径还是绝对路径</param>
    /// <returns>文件夹的相对路径，相对于Assets文件夹</returns>
    public static string AllocatePath(MonoBehaviour script, string subPath, bool local = false)
    {
        string path = string.Empty;
#if UNITY_EDITOR
        MonoScript m_Script = MonoScript.FromMonoBehaviour(script); //更新 使用UnityEditor API 
        path = AssetDatabase.GetAssetPath(m_Script);
        path = Path.GetDirectoryName(path);  //去除文件名
        path = Path.GetFullPath(path + "/" + subPath); //整合到完整路径，使用"/../" 回退到上一目录
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (local) path = path.Substring(path.IndexOf("Assets"));
#endif
        return path;
    }
    /// <summary>
    /// 编辑器下使用，给定一个类型，在这个类的周边目录下创建文件夹并返回绝对路径
    /// </summary>
    /// <param name="script">ScriptableObject 对象</param>
    /// <param name="subPath">指定要创建的文件夹的名称</param>
    /// <param name="local">指定获取相对路径还是绝对路径</param>
    /// <returns>文件夹的相对路径，相对于Assets文件夹</returns>
    public static string AllocatePath(ScriptableObject script, string subPath, bool local = false)
    {
        string path = string.Empty;
#if UNITY_EDITOR
        MonoScript m_Script = MonoScript.FromScriptableObject(script); //更新 使用UnityEditor API 
        path = AssetDatabase.GetAssetPath(m_Script);
        path = Path.GetDirectoryName(path);  //去除文件名
        path = Path.GetFullPath(path + "/" + subPath); //整合到完整路径，使用"/../" 回退到上一目录
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (local) path = path.Substring(path.IndexOf("Assets"));
#endif
        return path;
    }

    private static Dictionary<Type, MonoScript> MonoScriptCache = new Dictionary<Type, MonoScript>();

    public static MonoScript FindScriptFromType(Type _type)
    {
        if (!MonoScriptCache.TryGetValue(_type, out MonoScript monoScript))
        {
            var scriptGUIDs = AssetDatabase.FindAssets($"t:script {_type.Name}");
            foreach (var scriptGUID in scriptGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                if (script && string.Equals(_type.Name, Path.GetFileNameWithoutExtension(assetPath), StringComparison.OrdinalIgnoreCase) && script.GetClass() == _type)
                {
                    MonoScriptCache[_type] = monoScript = script;
                }
            }
        }
        return monoScript;
    }
}
