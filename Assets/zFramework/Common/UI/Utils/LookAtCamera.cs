using UnityEngine;
public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        Vector3 v = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, v.y + 180, v.z);
    }
}
