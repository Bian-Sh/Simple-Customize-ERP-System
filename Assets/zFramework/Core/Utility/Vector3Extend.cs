using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace zFrame
{
    public static class Vector3Extend
    {
        public static Vector3 Project(this Transform trans, Transform target, Vector3 axis, bool drawGizmos = true)
        {
            Vector3 projectVector = Vector3.Project(trans.position - target.position, axis);
            if (drawGizmos)
            {
#if UNITY_EDITOR
                Debug.DrawRay(target.position, Vector3.right * 100, Color.red); //世界坐标 X-axis 延迟线
                Debug.DrawRay(target.position, Vector3.up * 100, Color.red); //世界坐标 Y-axis 延迟线
                Debug.DrawLine(target.position, trans.position, Color.blue); //绘制对象连线
                Debug.DrawLine(target.position, target.position + projectVector, Color.black); //绘制投影向量
                Debug.DrawLine(trans.position, target.position + projectVector, Color.green); //绘制垂线
#endif
            }
            return projectVector;
        }
    }
}