using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrler;
using UnityEngine.UI;

public class GforcePicoGameMain : MonoBehaviour
{
    public GameMain gameMain;
    [Header("演示用")]
    [SerializeField] Transform ShotPosition;
    //[SerializeField] GameObject ShotObj;
    //[SerializeField] List<Material> materialList;
    //[SerializeField] Text GrabGameObjText;
    //GameObject GrabGameObj;
    Vector3 distance = Vector3.zero;
    public Text RayTargetValue;
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform leftPoint;
    Ray rightRay;
    Ray lefitRay;
    RaycastHit rightHit;
    RaycastHit leftHit;
    [SerializeField] Transform LeftAndTop;
    [SerializeField] Transform RightAndBottm;
    [SerializeField] GameObject[] Props;
    [SerializeField] int CreateInterval;
    [SerializeField] Transform propAll;
    float mytime = 0;
    [SerializeField] Transform leftGrabParent;
    [SerializeField] Transform rightGrabParent;
    GameObject leftProp;
    GameObject rightProp;
    float maxDistance = 5.0f;

    private void Start()
    {
        //游戏回调
        //提出来?
        gameMain.DeviceOneTickBackUp += DeviceOneTick;
        gameMain.DeviceTwoTickBackUp += DeviceTwoTick;
    }

    public void Update()
    {
        gameMain.Update();
        rightRay = new Ray(rightPoint.position, -rightGrabParent.forward * 10);
        if (Physics.Raycast(rightRay, out rightHit))
        {
        }
        Debug.DrawRay(rightPoint.position, -rightGrabParent.forward * 10, Color.red);

        lefitRay = new Ray(leftPoint.position, -leftGrabParent.forward * 10);
        if (Physics.Raycast(lefitRay, out leftHit))
        {
        }
        Debug.DrawRay(leftPoint.position, -leftGrabParent.forward * 10,Color.red);

        mytime += Time.deltaTime;
        if (mytime >= CreateInterval)
        {
            mytime = 0;
            CreateProp();
        }
    }

    public  void DeviceOneTick(GForceGameController _ctrler)
    {
        if (_ctrler ==null)
        {
            return;
        }
        if (_ctrler.GetGesture() == 1 && rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Prop")
            {
                Grab(rightGrabParent, rightHit.transform);
                rightProp = rightHit.collider.gameObject;
            }
            Debug.Log("Device1   " + 1 +_ctrler 
                + "    rightHit : " + rightHit.collider.name);
        }

        if (rightProp!=null)
        {
            if (_ctrler.GetGesture() == 2)
            {
                PutDown(rightProp.transform);
                rightProp = null;
                Debug.Log("Device1   " + 2);
            }
            if (_ctrler.GetGesture() == 3)
            {
                Move(rightProp.transform, 0.1f);
                Debug.Log("Device1   " + 3);
            }
            if (_ctrler.GetGesture() == 4)
            {
                Move(rightProp.transform, -0.1f);
                Debug.Log("Device1   " + 4);
            }
        }
    }

    

    public void DeviceTwoTick(GForceGameController _ctrler)
    {
        if (_ctrler == null)
        {
            return;
        }

        if (_ctrler.GetGesture() == 1 && leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Prop")
            {
                Grab(leftGrabParent, leftHit.transform);
                leftProp = leftHit.collider.gameObject;
                Debug.Log("Device2   " + 1 + _ctrler
                    + "   leftHit : " + leftHit.collider.name);
            }
        }
        if (leftProp!=null)
        {
            if (_ctrler.GetGesture() == 2)
            {
                PutDown(leftProp.transform);
                leftProp = null;
                Debug.Log("Device2   " + 2);
            }

            if (_ctrler.GetGesture() == 3)
            {
                Move(leftProp.transform, 0.1f);
                Debug.Log("Device2   " + 3);
            }

            if (_ctrler.GetGesture() == 4)
            {
                Move(leftProp.transform, -0.1f);
                Debug.Log("Device2   " + 4);
            }
        }

    }

    public void CreateProp()
    {
        if (propAll.childCount >= 10)
        {
            return;
        }
        var prop = Instantiate(Props[UnityEngine.Random.Range(0, Props.Length - 1)]);
        prop.transform.position = new Vector3(UnityEngine.Random.Range(LeftAndTop.position.x, RightAndBottm.position.x),
            LeftAndTop.position.y,
            UnityEngine.Random.Range(RightAndBottm.position.z, LeftAndTop.position.z));
        prop.transform.SetParent(propAll);
    }

    /// <summary>
    /// 放下手中的物体
    /// </summary>
    /// <param name="prop"></param>
    public void PutDown(Transform prop)
    {
        if (prop == null)
        {
            return;
        }
        prop.GetComponent<Rigidbody>().useGravity = true;
        prop.SetParent(propAll);
    }

    /// <summary>
    /// 移动手中的物体
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="_value"></param>
    public void Move(Transform transform, float _value)
    {
        // - 负数是推走
        // + 正数是拉近
        if (transform == null)
        {
            return;
        }
        Debug.Log("111   " + transform.gameObject);
        if (transform.localPosition.z + _value <= 0
            && transform.localPosition.z + _value >= -maxDistance)
        {
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z + _value);
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, Mathf.Clamp(transform.localPosition.z, -maxDistance, 0));
        }
    }

    /// <summary>
    /// 抓起一个物体
    /// </summary>
    /// <param name="grabObj"></param>
    /// <param name="prop"></param>
    public void Grab(Transform grabObj, Transform prop)
    {
        if (grabObj.childCount >= 1)
        {
            PutDown(grabObj.GetComponentInChildren<Transform>());
            return;
        }
        prop.GetComponent<Rigidbody>().useGravity = false;
        prop.GetComponent<Rigidbody>().velocity = Vector3.zero;
        prop.SetParent(grabObj);
        prop.localPosition = Vector3.zero;
        prop.localRotation = Quaternion.identity;
    }
}
