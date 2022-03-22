using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.Events;
using zFrame.Time;

public class SmoothDampLable : MonoBehaviour
{
    [Header("Label跟随目标点")]
    public Transform target;        //The player
    [Header("Label跟随点切换阈值")]
    public float swicthThreshold = 0.3f;
    [Header("缓动灵敏度")]
    public float smoothTime = 0.1f;  //Smooth Time
    [Header("当前跟随的位置")]
    [Range(0, 2)]
    public int index = 0; //Label的Pin点
    private Transform targetParent;
    private Transform lineStartPos;
    private Transform anchorRoot;

    private Vector3 velocity;       //Velocity
    List<Transform> anchors;
    private Transform interactiveAnchor;
    private Dictionary<Transform, Vector3> relativePos;
    //private bool doMove = false;
    private LineRenderer lineRenderer;
    private void Start()
    {
        targetParent = target.parent;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineStartPos = targetParent.Find("StartPoint");

        anchorRoot = transform.Find("Anchors");

        relativePos = new Dictionary<Transform, Vector3>();
        anchors = new List<Transform>();

        foreach (Transform item in anchorRoot)
        {
            anchors.Add(item);
            relativePos.Add(item, transform.position - item.position);
        }
        SetPinPoint(index);
    }

    void Update()
    {
        if (null != target && Vector3.Distance(interactiveAnchor.position, target.position) > 0.001f)
        {
            transform.position = relativePos[interactiveAnchor] + new Vector3(Mathf.SmoothDamp(interactiveAnchor.position.x, target.position.x, ref velocity.x, smoothTime), Mathf.SmoothDamp(interactiveAnchor.position.y, target.position.y, ref velocity.y, smoothTime), Mathf.SmoothDamp(interactiveAnchor.position.z, target.position.z, ref velocity.z, smoothTime));
            lineRenderer.SetPositions(new Vector3[2] { interactiveAnchor.position, lineStartPos.position });

            if (!Timer.Exist("ForThreshold"))
            {
                Vector3 cachedDrection = target.position - interactiveAnchor.position;
                Timer.AddTimer(0.02f, "ForThreshold").OnCompleted(() =>
                 {
                     Vector3 direction = target.position - interactiveAnchor.position;
                     if (Mathf.Abs(cachedDrection.x - direction.x) > swicthThreshold)
                     {
                         if (direction.x < 0 && index != 0)
                         {
                             SetPinPoint(--index);
                         }
                         else if (direction.x > 0 && index != anchors.Count - 1)
                         {
                             SetPinPoint(++index);
                         }
                     }
                 });

            }
        }
    }


    /// <summary>
    /// 设置Label的那个点跟随被label的物体
    /// </summary>
    /// <param name="name"></param>
    public void SetPinPoint(string name)
    {
        int _index = anchors.FindIndex(v => v.name == name);
        if (_index != -1)
        {
            index = _index;
            interactiveAnchor = anchors[index];
        }
    }

    /// <summary>
    /// 设置Label的那个点跟随被label的物体
    /// </summary>
    /// <param name="_index"></param>
    //[EditorButton]
    public void SetPinPoint(int _index)
    {
        if (_index >= 0 && _index < anchors.Count)
        {
            index = _index;
            interactiveAnchor = anchors[index];
        }
    }
}
