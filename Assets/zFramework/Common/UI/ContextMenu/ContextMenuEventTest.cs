using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.Events;
using zFrame.UI;

public class ContextMenuEventTest : MonoBehaviour {

 //   public GameObject prefab;
    //public Transform canvas;
	void Start () {
        EventManager.AddListener(ContextMenuEvent.Click, OnContextMenuItemClicked);
        EventManager.AddListener(ContextMenuEvent.PointEnter, OnContextMenuItemEnter);
        EventManager.AddListener(ContextMenuEvent.PointExit, OnContextMenuItemExit);

	}

    private void OnContextMenuItemEnter(BaseEventArgs obj)
    {
        ContextMenuEventArgs args = obj as ContextMenuEventArgs;
        Debug.Log("OnContextMenuItemEnter: " + args.command);
    }

    private void OnContextMenuItemExit(BaseEventArgs obj)
    {
        ContextMenuEventArgs args = obj as ContextMenuEventArgs;

        Debug.Log("OnContextMenuItemExit: "+args.command);
    }

    private void OnContextMenuItemClicked(BaseEventArgs obj)
    {
        ContextMenuEventArgs args = obj as ContextMenuEventArgs;

        Debug.Log("OnContextMenuItemClicked: " + args.command);
     //   GameObject goo = GameObject.Instantiate(prefab);
        //goo.transform.SetParent(canvas,false);

    }

    void Update () {
		
	}

    private void OnDestroy()
    {
        EventManager.DelListener(ContextMenuEvent.Click, OnContextMenuItemClicked);
        EventManager.DelListener(ContextMenuEvent.PointEnter, OnContextMenuItemEnter);
        EventManager.DelListener(ContextMenuEvent.PointExit, OnContextMenuItemExit);

    }
}
