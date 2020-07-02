using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    public static List<string> moveList = new List<string>() {  };
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };
    private CubeState cubeState;
    private ReadCube readCube;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }



    // Update is called once per frame
    void Update()
    {
        //큐브스테이트 start에서 큐브리딩먼저하고 started를 트루로바꿈
        if(moveList.Count>0 && !CubeState.autoRotating
            &&CubeState.started)
        {
            //첫번쨰인덱스를 움직이고
            DoMove(moveList[0]);
            //움직인 첫번쨰인덱스 삭제
            moveList.Remove(moveList[0]);
        }
    }

    public void shuffle()
    {
        List<string> moves = new List<string>();
        int shuffleLength = Random.Range(10, 30); //10~30 평균20회전짜리 셔플
        for (int i = 0; i < shuffleLength; i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);//18개 회전중에 하나랜덤
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
    }

    void DoMove(string Move)
    {
        readCube.ReadState(); //시작하자마자 한번읽어줌

        CubeState.autoRotating = true; //자동회전 참으로바꿈

        if (Move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (Move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (Move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
        if (Move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (Move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (Move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
        if (Move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (Move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (Move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
        if (Move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (Move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (Move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
        if (Move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (Move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (Move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
        if (Move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (Move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (Move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
        //@@@@@@@@@@@@@@@@@@@@@@
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        //자동회전
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);


    }

}
