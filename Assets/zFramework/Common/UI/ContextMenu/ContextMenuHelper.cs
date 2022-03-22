/*
 *  功能： 检测右键事件并根据光标下游戏对象弹出指定的右键菜单
 *  实现：利用物理碰撞器怼场景中的3d 游戏对象，当右键按下时根据怼到的东西唤起不同的
 *  右键菜单。
 *  备注： 怼到的游戏对象，必须要挂载 collider 和协议声明租价
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace zFrame.UI
{
    public class ContextMenuHelper : PhysicsRaycaster
    {
        private ContextMenu menu;
        public GameObject target;
        protected override void Start()
        {
            menu = ContextMenu.Instance;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            try
            {
                base.Raycast(eventData, resultAppendList);
            }
            catch (System.Exception)
            {
                //throw;
            }
            if (resultAppendList.Count > 0)
            {
                GameObject go = resultAppendList[0].gameObject;
                if (go.GetComponent<Collider>())
                {
                    target = go;
                }
            }
            else
            {
                target = null;
            }
        }


        //        void Update()
        //        {
        //            if (null != EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        //            {
        ////             Debug.Log("Cursor is hover some UI element ! ");
        //                return;
        //            }
        //            RaycastHit hit;
        //            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,maxDistance,layerMask))
        //            {
        //                GameObject cached = hit.collider.gameObject;
        //                if (null == selected)//从无到有
        //                {
        //                    selected = cached;
        //                    EventManager.Allocate<StylusEventArgs>()
        //                        .Config(StylusEvent.Enter, gameObject, selected)
        //                        .Invoke();
        //                }
        //                else  //从有到有
        //                {
        //                    if (selected != cached)
        //                    {
        //                        EventManager.Allocate<StylusEventArgs>()
        //                             .Config(StylusEvent.Exit, gameObject, selected)
        //                             .Invoke();
        //                        selected = cached;
        //                        EventManager.Allocate<StylusEventArgs>()
        //                            .Config(StylusEvent.Enter, gameObject, selected)
        //                            .Invoke();
        //                    }
        //                }

        //                if (Input.GetMouseButtonDown(0))
        //                {
        //                    EventManager.Allocate<StylusEventArgs>()
        //                        .Config(StylusEvent.Press, gameObject, selected, 0)
        //                        .Invoke();
        //                }
        //                if (Input.GetMouseButtonUp(0))
        //                {
        //                    EventManager.Allocate<StylusEventArgs>()
        //                        .Config(StylusEvent.Release, gameObject, selected, 0)
        //                        .Invoke();
        //                }

        //            }
        //            else
        //            {
        //                if (null != selected)
        //                {
        //                    EventManager.Allocate<StylusEventArgs>()
        //                        .Config(StylusEvent.Exit, gameObject, selected)
        //                        .Invoke();
        //                    selected = null;
        //                }
        //            }
        //        }

    }
}
