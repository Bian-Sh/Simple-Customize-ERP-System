using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

//[InitializeOnLoad]
public class ChangeFontForPrefab: EditorWindow
{

    static ChangeFontForPrefab()
    {
        //toChangeFont = new Font("Arial");
        //toChangeFontStyle = FontStyle.Normal;
    }

    [MenuItem("Tools/更换字体(ForPrefab)")]
    private static void ShowWindow()
    {
        ChangeFontForPrefab cw = EditorWindow.GetWindow<ChangeFontForPrefab>(true, "Window/Change Font");

    }
    Font toFont;
    static Font toChangeFont;
    FontStyle toFontStyle;
    static FontStyle toChangeFontStyle;
    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("目标字体:");
        toFont = (Font)EditorGUILayout.ObjectField(toFont, typeof(Font), true, GUILayout.MinWidth(100f));
        toChangeFont = toFont;
        GUILayout.Space(10);
        GUILayout.Label("类型:");
        toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(100f));
        toChangeFontStyle = toFontStyle;
        if (GUILayout.Button("修改字体！"))
        {
            Change();
        }
    }
    public static void Change()
    {
        //获取所有UILabel组件
        if (Selection.objects == null || Selection.objects.Length == 0) return;
        //如果是UGUI讲UILabel换成Text就可以
        Object[] labels = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
        foreach (Object item in labels)
        {
            //如果是UGUI讲UILabel换成Text就可以
            Text label = (Text)item;
            label.font = toChangeFont;
           // label.fontStyle = toChangeFontStyle;
            //label.font = toChangeFont;（UGUI）
            Debug.Log(item.name + ":" + label.text);
            //
            EditorUtility.SetDirty(item);//重要
        }
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }



    private void Update()
    {
    }

    private void OnDestroy()
    {
    }

}
