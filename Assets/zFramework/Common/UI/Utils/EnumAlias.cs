using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using System.Text.RegularExpressions;
#endif

/// <summary>
/// 设置枚举别名，用于 Inspector 展示
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumAlias : PropertyAttribute
{
    /// <summary>
    /// 枚举名称
    /// </summary>
    public string name;
    public EnumAlias(string name)
    {
        this.name = name;
    }
    /// <summary>
    /// 获取枚举项的别名,只建议在编辑器下使用
    /// </summary>
    /// <param name="type">枚举的类型</param>
    /// <param name="enumvalue">指定的枚举项</param>
    /// <returns>枚举项别名</returns>
    public static string GetEnumItemAlias(Type type, Enum enumvalue)
    {
        if (!type.IsEnum) return string.Empty;
        string enumname = enumvalue.ToString();
        FieldInfo info = type.GetField(enumname);
        EnumAlias[] enumAttributes = (EnumAlias[])info.GetCustomAttributes(typeof(EnumAlias), false);
        return enumAttributes.Length == 0 ? enumname : enumAttributes[0].name;
    }
    public static string GetEnumFieldAlias(Type type, string enumfieldname)
    {
        if (!type.IsClass) return string.Empty;
        FieldInfo info = type.GetField(enumfieldname);
        EnumAlias[] enumAttributes = (EnumAlias[])info.GetCustomAttributes(typeof(EnumAlias), false);
        return enumAttributes.Length == 0 ? enumfieldname : enumAttributes[0].name;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumAlias))]
public class EnumNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 替换属性名称
        EnumAlias enumAttirbute = (EnumAlias)attribute;
        label.text = enumAttirbute.name;

        bool isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement)
        {
            label.text = property.displayName;
        }

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            DrawEnum(position, property, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    /// <summary>
    /// 重新绘制枚举类型属性
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    private void DrawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        Type type = fieldInfo.FieldType;

        string[] names = property.enumNames;
        string[] values = new string[names.Length];
        while (type.IsArray)
        {
            type = type.GetElementType();
        }

        for (int i = 0; i < names.Length; ++i)
        {
            FieldInfo info = type.GetField(names[i]);
            EnumAlias[] enumAttributes = (EnumAlias[])info.GetCustomAttributes(typeof(EnumAlias), false);
            values[i] = enumAttributes.Length == 0 ? names[i] : enumAttributes[0].name;
        }

        int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1)
        {
            property.enumValueIndex = index;
        }
    }
}
#endif
