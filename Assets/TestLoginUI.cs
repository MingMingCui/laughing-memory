using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestLoginUI : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Debug.LogFormat("Stub:33 isEnemy:true, round:2 cal:{0}", PathFinder.Stub2InitPos(2, true, 33));

        int g1 = 33;

        Debug.LogFormat("Cal border grid of {0} ： LT {1} T {2} RT {3} L {4} R {5} LB {6} B {7} RB {8}", g1, PathFinder.BorderGrid(g1, GridDirection.LT),
            PathFinder.BorderGrid(g1, GridDirection.T), PathFinder.BorderGrid(g1, GridDirection.RT), PathFinder.BorderGrid(g1, GridDirection.L),
            PathFinder.BorderGrid(g1, GridDirection.R), PathFinder.BorderGrid(g1, GridDirection.LB), PathFinder.BorderGrid(g1, GridDirection.B),
            PathFinder.BorderGrid(g1, GridDirection.RB));
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnGUI()
    {

        //if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 110, 200, 500), "进入主界面"))
        //{
        //    SceneMgr.Instance.LoadScene("Main");
        //}
    }
}
