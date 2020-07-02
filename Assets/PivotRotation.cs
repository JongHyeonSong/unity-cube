using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float speed = 300f;

    private float sensitivity = 0.4f;
    private Vector3 rotation;

    private Quaternion targetQuarternion;

    private ReadCube readCube;
    private CubeState cubeState;
    

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dragging && !autoRotating)
        {
            SpinSide(activeSide);//드래그중이면 > 활성화면을 스핀 사이드함수에넣어서 돌림

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();//오른마우스가 떼지면 가까운각으로 각잡기
            }
        }
        if (autoRotating)
        {
            AutoRotate();
        }
    }

    private void SpinSide(List<GameObject> side)
    {
        //로테이션리셋
        rotation = Vector3.zero;

        //현재 마우스 - 마지막마우스 해서 마우스차이만큼 얼마나 돌려야할지정함
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);


        //매개변수가 front면 앞면을돌린다 
        //앞 /뒤는 x축이다
        
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }

        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }

        if (side == cubeState.front)
        {
            //여기서 벡터방향과 민감도를정하고 바로밑에서 돌림
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            //여기서 벡터방향과 민감도를정하고 바로밑에서 돌림
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        //rotate 여기는 아직도 피봇스크립트
        transform.Rotate(rotation, Space.Self);

        mouseRef = Input.mousePosition;

    }

    public void Rotate(List<GameObject> side)
    {
        activeSide = side; //받은 side리스트를 활성화activeSide에담음
        mouseRef = Input.mousePosition;
        dragging = true;

        //회전값 벡터를 만들자
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;


    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuarternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    //근사한 각으로 정리해주기
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;

        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuarternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()
    {
        dragging = false; //자동회전중에는 드래그불가
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation, targetQuarternion, step);
        //1도미만이면 자동시행
        if(Quaternion.Angle(transform.localRotation, targetQuarternion) <= 1)
        {
            transform.localRotation = targetQuarternion;
            //unperant 작은큐브
            cubeState.PutDown(activeSide, transform.parent);//언팔하고
            readCube.ReadState();//큐브상태읽어주고
            CubeState.autoRotating = false;
            autoRotating = false;//변수들바꿔주고
            dragging = false;
        }

    }


}
