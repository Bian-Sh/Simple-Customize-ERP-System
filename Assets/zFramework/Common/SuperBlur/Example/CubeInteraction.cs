using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zFrame.Example
{
    /// <summary>
    ///   Cube 在demo 中的交互 
    /// </summary>
    public class CubeInteraction : MonoBehaviour
    {

        void Update()
        {
            transform.Rotate(Vector3.up, 1, Space.Self);
        }

        public void OnFadeFinished()
        {
            transform.position = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
        }
    }
}
