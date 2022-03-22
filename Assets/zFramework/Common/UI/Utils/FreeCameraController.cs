
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public enum RotationMode
    {
        FREE_RATATION,
        MANUAL_RATATION
    }

    //模型
    public Transform target;
    private Vector2 targetAngle, currentAngle;
    private float targetDistance, currentDistance;
    private Vector3 initAngel;
    private Vector3 initPositon;
    public int minAngel = 0;
    public int maxAngel = 45;
    public float ZoomSensitivity = 10;
    public float RotateSensitivity = 5;
    public float TouchZoomSensitivity = 1;
    public float TouchRotateSensitivity = 0.5f;
    public int minDistance = 1;
    public int maxDistance = 1000;
    public float damper = 5;
    public RotationMode mode = RotationMode.FREE_RATATION;

    void Start()
    {
        
    }

    public void Init()
    {
        //摄像机初始位置和角度
        initAngel = transform.eulerAngles;
        initPositon = transform.position;

        //初始角度
        targetAngle = currentAngle = transform.eulerAngles;

        //初始距离
        targetDistance = currentDistance = Vector3.Distance(transform.position, target.position);
    }

    void LateUpdate()
    {
        if(mode == RotationMode.FREE_RATATION)
        {
            RatateFree();
        }
        else
        {
            RotateByMouseOrTouch();
        }
    }

    private float dx;
    private float dy;
    private Touch beginTouch1;
    private Touch beginTouch2;
    private Touch crtTouch1;
    private Touch crtTouch2;
    private float beginDistance;
    private float crtDistance;

    /// <summary>
    /// 拖动旋转或者放大缩小
    /// </summary>
    private void RotateByMouseOrTouch()
    {
        
        dx = dy = 0;

        //左键移动
        if(Input.GetMouseButton(0))
        {
            dx = Input.GetAxis("Mouse X");
            dy = Input.GetAxis("Mouse Y");
            if(Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {

            }
        }

        //右键旋转
        if (Input.GetMouseButton(1))
        {
            dx = Input.GetAxis("Mouse X");
            dy = Input.GetAxis("Mouse Y");
            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {
                targetAngle.x -= dy * TouchRotateSensitivity;
                targetAngle.y += dx * TouchRotateSensitivity;
                //计算取值范围
                targetAngle.x = Mathf.Clamp(targetAngle.x, minAngel, maxAngel);
            }
        }

        //触摸旋转
        if (Input.multiTouchEnabled && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            dx = Input.GetTouch(0).deltaPosition.x;
            dy = Input.GetTouch(0).deltaPosition.y;
            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {
                targetAngle.x -= dy * TouchRotateSensitivity;
                targetAngle.y += dx * TouchRotateSensitivity;
                //计算取值范围
                targetAngle.x = Mathf.Clamp(targetAngle.x, minAngel, maxAngel);
            }
        }

        

        //滚轮缩放
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            targetDistance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        //触摸缩放
        if (Input.multiTouchEnabled && Input.touchCount >= 2)
        {
            crtTouch1 = Input.GetTouch(0);
            crtTouch2 = Input.GetTouch(1);
            if (crtTouch1.phase == TouchPhase.Began || crtTouch2.phase == TouchPhase.Began)
            {
                beginTouch1 = crtTouch1;
                beginTouch2 = crtTouch2;
            }
            else
            {
                beginDistance = Vector2.Distance(beginTouch1.position, beginTouch2.position);
                crtDistance = Vector2.Distance(crtTouch1.position, crtTouch2.position);

                targetDistance -= (crtDistance - beginDistance) * TouchZoomSensitivity;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
        }

        //计算最终角度和距离
        currentAngle = Vector2.Lerp(currentAngle, targetAngle, damper * Time.deltaTime);
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, damper * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentAngle);
        transform.position = target.position - transform.forward * currentDistance;
    }

    [Header("自由旋转速度")]
    public float speed = 2f;
    [Range(45, 220), Header("摆幅")]
    public int maxAngle = 110;
    private bool isRotating = true;
    private bool reverse = false;
    private float angleX = 0;
    private Vector3 angle;
    /// <summary>
    /// 自由旋转
    /// </summary>
    private void RatateFree()
    {
        if (isRotating)
        {
            angle = transform.rotation.eulerAngles;
            angleX += speed * Time.deltaTime * (reverse ? -1 : 1);
            if(Mathf.Abs(angleX) > maxAngel / 2)
            {
                reverse = !reverse;
                angleX = Mathf.Clamp(angleX, - maxAngel / 2, maxAngel / 2);
            }
            angle.y = initAngel.y + angleX;
            transform.rotation = Quaternion.Euler(angle);
            transform.position = target.position - transform.forward * currentDistance;
        }
    }

    public void Pause()
    {
        isRotating = false;
    }
    public void Restart()
    {
        isRotating = true;
    }

    public void ResumeInitialPosition()
    {
        //初始角度
        targetAngle = currentAngle = initAngel;

        //初始距离
        targetDistance = currentDistance = Vector3.Distance(initPositon, target.position);
        transform.rotation = Quaternion.Euler(currentAngle);
        transform.position = target.position - transform.forward * currentDistance;
    }
}
