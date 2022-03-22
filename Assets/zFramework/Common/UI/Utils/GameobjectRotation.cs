
using DG.Tweening;
using UnityEngine;

namespace Assets.UnityFramework.Utils
{
    public class GameobjectRotation : MonoSingleton<GameobjectRotation>
    {
        public float speed = 1f;
        public bool isRotate = true;
        [Range(45, 220), Header("摆幅")]
        public int maxAngle = 110;
        private bool reverse = false;

        public override void OnInit()
        {
            

        }

        void Update()
        {
            if (isRotate)
            {
                transform.Rotate(Vector3.up, speed * Time.deltaTime * (reverse ? -1 : 1), Space.Self);
                float y_axis = transform.eulerAngles.y;
                if (y_axis >= 180)
                {
                    y_axis -= 360;
                }
                if (Mathf.Abs(y_axis) > maxAngle / 2)
                {
                    reverse = !reverse;
                    if (y_axis > 0)
                    {
                        transform.eulerAngles = new Vector3(0, maxAngle / 2, 0);
                    }
                    else if (y_axis < 0)
                    {
                        transform.eulerAngles = new Vector3(0, maxAngle / 2 * -1, 0);
                    }
                }
            }
        }

        public void Pause()
        {
            isRotate = false;
        }
        public void Restart()
        {
            isRotate = true;
        }
    }
}
