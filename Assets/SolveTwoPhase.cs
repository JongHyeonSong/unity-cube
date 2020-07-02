using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

public class SolveTwoPhase : MonoBehaviour
{
    public ReadCube readCube;
    public CubeState cubeState;

    private bool doOnce = true;

    private bool justOne = true;
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if( CubeState.started && doOnce)
        {
            doOnce = false;
            Solver();
        }
    }


    public void Solver()
    {
        readCube.ReadState();

        //지금상태를 스트링으로받고
        string moveString = cubeState.GetStateString();

        print(moveString);
        //큐브를풀고



        string info = "";
        //SearchRunTime,Search 모두 코시엠바 클래스  


        string solution = Search.solution(moveString, out info);

        if (justOne)
        {
            solution = SearchRunTime.solution(moveString, out info, buildTables: true);
            justOne = false;
        }
       
       
        List<string> solutionList = StringToList(solution); //코시엠바 해답에서 공백을빼고 정리함

        Automate.moveList = solutionList; //무브리스트에 리스트를 새로넣어서 업데이트에서 자동으로돌아감
        //해법무빙을 리스트로받고
        //Automate에 리스트넣고


    }
    List<string> StringToList(string solution)
    {
        List<string> solutionList =
            new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

}
