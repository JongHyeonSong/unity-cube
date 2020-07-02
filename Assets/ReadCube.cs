using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    public Transform tUp; //빨콩
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tFront;
    public Transform tBack;

    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 9;
    CubeState cubeState;
    CubeMap cubeMap;
    public GameObject emptyGo;

    // Start is called before the first frame update
    void Start()
    {
        SetRayTransforms();  //시작하자마자 모든빨콩이 중앙을보게


        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();
        ReadState();
        CubeState.started = true;
    }

    // Update is called once per frame
    void Update()
    {
       // ReadState();


    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        //readface는 6번 도니까 6번 타이핑할거임
        //여기 readCube 스크립트에서 바로 CubeState의 up을 업로드해줄거임
        cubeState.up = ReadFace(upRays, tUp); 
        //tUp은 start하자마자 SetRayTransforms()함수에서 buildray()함수로 90도 돌려서 중앙을 바라보고있음
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        // CubeState 구청에 모든 서류가 전달되었으므로 Set()함수씀
        cubeMap.Set(); //CubeState 구청자료를 cubeMap현장 ui패널에 대입


    }

    void SetRayTransforms()
    {
        //upRays에는 9개빨콩과 벡터
        //z로보던걸 y90 돌려서 겹친레이저 갈리게하고 x90돌려서 아래로보게함
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));

    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        //rayTransform=빨콩 , direction=큐브중앙을보게함
        int rayCount = 0;
        //왼쪽위0부터 오른아래8번까지만듬
        List<GameObject> rays = new List<GameObject>();
        for(int y=1; y>-2; y--)
        {
            for(int x=-1; x<2; x++)
            {
                //startPos는 빨콩분신9개 각각의 벡터
                Vector3 startPos = new Vector3(
                    rayTransform.localPosition.x + x,
                    rayTransform.localPosition.y + y,
                    rayTransform.localPosition.z
                    );
                GameObject rayStart = Instantiate(emptyGo,
                    startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);//rays=[ "0", "1"...."8"] 숫자하나하나가 게임오브젝트
                rayCount++;
            }
        }

        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays; //리턴값이 upRays 등으로 들어감

    }
    //한쪽면만 일단함 6번돌겠죠? readFace는 6번돈다
    //rayTransform은 빨콩에서 벡터나가는 친구 이미 중앙을봄
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        //rayStarts 는 upRays 리스트등을 받는다
        List<GameObject> facesHit = new List<GameObject>();

        //rayStart하나하나가 빨콩이고 rayStarts는 빨콩9개모임
        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;//빨콩포지션
            RaycastHit hit;

            //Physics.Raycast( "시작위치" , "레이져방향", "맞는애", "레이져길이", "엄선된 맞는애"
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.DrawRay ( "시작위치", "레이져방향*레이져길이", "색깔")
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
       

        return facesHit; //만든리스트를 cubeState.front리스트에넣음


        //cubeMap.Set(); //cubeState.front랑 패널리스트랑비교해서 색넣음

    }
}
