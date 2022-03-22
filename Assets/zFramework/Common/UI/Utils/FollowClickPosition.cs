using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityFramework.Utils
{
    public class FollowClickPosition : MonoBehaviour
    {
        private void OnEnable()
        {
            rootRect = this.transform as RectTransform;
            rootRect.pivot = new Vector2(0, 1);
            canvas = GameObject.Find("CanvasUI").transform;
            CalculateContainerPositon();
        }

        public float offsetX = 30f;
        private RectTransform rootRect;
        private  Transform canvas;
        private void CalculateContainerPositon()
        {
            var width = rootRect.rect.width * canvas.localScale.x;
            var height = rootRect.rect.height * canvas.localScale.y;
            var newY = Input.mousePosition.y < height ? Input.mousePosition.y + height : Input.mousePosition.y; //编辑器下有边界换算错误，打包后无异常。
            var newX = Input.mousePosition.x < Screen.width - width ? Input.mousePosition.x : Input.mousePosition.x - width;
            rootRect.position = new Vector2(newX + offsetX, newY);
        }
    }
}
