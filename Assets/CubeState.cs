using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public static bool autoRotating = false;
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(List<GameObject> cubeSide)
    {
        //cubeSide는 하나하나가cubeState.up,cubeState.down, ..6개순서대로
        foreach(GameObject face in cubeSide)
        {
            //지금부터 face는 진짜 한면의 색깔 하나하나
            if( face != cubeSide[4]) //중앙큐브만아니면 들어가
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
                //사이드회전 로직
            }

        }
       
    }
    //PickUp함수랑 반대임
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach(GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }

        }
    }
    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach(GameObject face in side)
        {
            sideString += face.name[0].ToString();
            
        }
        return sideString;
    }

    public string GetStateString() //한면에9개씩 6면 전부합치기
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);
        return stateString;

    }



}
