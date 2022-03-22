    using UnityEngine;
using System.Collections;
using UnityEditor;
public class LoadTagAndLayer : AssetPostprocessor
{
    private static string[] Tags = { "GrabMyParent", "ImParent" };//所有需要添加的tag值  
    private static string[] Layers = { "BreadboardComponents", "Breadboard" };//所有需要添加的layer值  
    /// <summary>  
    /// 当所有资源加载完毕后执行  
    /// </summary>  
    /// <param name="importedAssets"></param>  
    /// <param name="deletedAssets"></param>  
    /// <param name="movedAssets"></param>  
    /// <param name="movedFromAssetPaths"></param>  
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string s in importedAssets)
        {
            string[] ss = s.Split('/');
            if (ss[ss.Length - 1].Equals("LoadTagAndLayer.cs"))
            {
                Debug.Log(ss[ss.Length - 1]+":自动更新Layer&Tag");
                foreach (string item in Tags)
                {
                    AddTag(item);//循环加载tag  
                }
                foreach (string item in Layers)
                {
                    AddLayer(item);//循环加载layer  
                }
                return;
            }
        }
    }
    /// <summary>  
    /// 加载层  
    /// </summary>  
    /// <param name="layer"></param>  
    private static void AddLayer(string layer)
    {


        if (!IsHasLayer(layer))
        {
            //加载项目设置层以及tag值管理 资源  
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();//获取层或tag值所有列表信息  
            while (it.NextVisible(true))//判断向后是否还有信息，如果没有则返回false  
            {
                if (it.name == "layers")
                {
                    //层默认是32个，只能从第8个开始写入自己的层  
                    for (int i = 8; i < it.arraySize; i++)
                    {
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);//获取层信息  
                        if (string.IsNullOrEmpty(dataPoint.stringValue))//如果制定层内为空，则可以填写自己的层名称  
                        {
                            dataPoint.stringValue = layer;//设置名字  
                            tagManager.ApplyModifiedProperties();//保存修改的属性  
                            return;
                        }
                    }
                }
            }
        }
    }

    /// <summary>  
    /// 判断是否已经有层  
    /// </summary>  
    /// <param name="layer"></param>  
    /// <returns></returns>  
    private static bool IsHasLayer(string layer)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Equals(layer))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>  
    /// 添加Tag值  
    /// </summary>  
    /// <param name="tag"></param>  
    private static void AddTag(string tag)
    {
        if (!IsHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "tags")
                {
                    it.arraySize++;
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                    dataPoint.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                    return;
                }
            }
        }
    }

    /// <summary>  
    /// 判断是否有tag值  
    /// </summary>  
    /// <param name="tag"></param>  
    /// <returns></returns>  
    private static bool IsHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Equals(tag))
            {
                return true;
            }
        }
        return false;
    }
}