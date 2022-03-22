using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace UnityFramework.Utils
{
    [RequireComponent(typeof(PhysicsRaycaster))]
    public class CheckAllClick : MonoSingleton<CheckAllClick>
    {
        private PhysicsRaycaster raycaster;
        public EventSystem eventSystem;
        // Use this for initialization
        void Start()
        {
            raycaster = this.GetComponent<PhysicsRaycaster>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool CheckIsHitTag(params string[] tags)
        {
            if (null != raycaster)
            {
                PointerEventData data = new PointerEventData(eventSystem);
                data.pressPosition = Input.mousePosition;
                data.position = Input.mousePosition;
                List<RaycastResult> list = new List<RaycastResult>();
                raycaster.Raycast(data, list);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach(string tag in tags)
                        {
                            if (list[i].gameObject.CompareTag(tag))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }

}