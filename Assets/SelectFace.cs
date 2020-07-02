using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    CubeState cubeState;
    ReadCube readCube;
    int layerMask = 1 << 9;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {

        //&& !CubeState.autoRotating
        if (!CubeState.autoRotating) {
            if (Input.GetMouseButtonDown(0))
            {
                //지금 큐브상태
                readCube.ReadState(); //패널에 찍어줌

                //2차원 마우스에서 2차원큐브로 레이져쏨 6면중 어느face가 맞았는지 볼수있음
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //지금 ray는 2차원 마우스 찍는포지션
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                {
                    //마우스로 어느면을 클릭한지가 face에 들어감
                    GameObject face = hit.collider.gameObject;

                    //update안에서 눌린동안 한번 cubeState의 6개 리스트를가져옴
                    List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };

                    // 레이져쏴서 맞는게나온다면이라는 조건문후에 나오는 for문
                    foreach (List<GameObject> cubeSide in cubeSides)
                    {
                        //cubeSide는 하나하나가cubeState.up,cubeState.down, ..6개순서대로
                        if (cubeSide.Contains(face)) //face:마우스 왼쪽클릭한거 2차원
                        {
                            cubeState.PickUp(cubeSide); //픽업함수쓰고
                            cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
                            //Rotate() 는 피봇스크립트에있음
                        }
                    }
                }
            }
        } }
}
