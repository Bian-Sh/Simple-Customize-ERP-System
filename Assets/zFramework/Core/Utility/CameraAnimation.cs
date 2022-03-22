using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour {

    public float maxAngle=90;
    public float speed=1;
    private bool reverse = false;
    private float previewValue;
    private float startAX;
    private float startAY;
    private float startAZ;
    private Vector3 startA;
	void Start () {
        //previewValue = maxAngle;
        startA = transform.localEulerAngles;
        startAX = startA.x;
        startAY = startA.y;
        startAZ = startA.z;
	}

	void Update () {
        //if (previewValue!=maxAngle)
        //{
        //    previewValue = maxAngle;
        //    transform.localRotation = Quaternion.identity;
        //}
        transform.Rotate(Vector3.up, speed*Time.deltaTime*(reverse ? -1 : 1));
        float y_axis = transform.localEulerAngles.y - startAY;
        
        if (y_axis>=180)
        {
            y_axis -= 360;
        }

        if(y_axis <= -180)
        {
            y_axis += 360;
        }

        if (Mathf.Abs(y_axis)>maxAngle/2)
        {
            reverse = !reverse;
            //if (y_axis > maxAngle / 2)
            //{
                
            //    transform.localEulerAngles = new Vector3(startAX, maxAngle / 2 + startAY, startAZ);
                
            //}
            //else if (y_axis < -maxAngle / 2)
            //{
            //    transform.localEulerAngles = new Vector3(startAX, maxAngle / 2 * -1 + startAY, startAZ);
            //}
        }
	}

    private void OnDisable()
    {
        //transform.localRotation = Quaternion.identity;
    }
}
