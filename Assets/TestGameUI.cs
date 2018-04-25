using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameUI : MonoBehaviour {


    private void OnGUI()
    {
        //if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 100, 200, 100), "战斗"))
        //{
        //    ////创建FightUnit
        //    Dictionary<int, int> testskilldata = new Dictionary<int, int>();
        //    Dictionary<int, int> testskilldata2 = new Dictionary<int, int>();
        //    testskilldata.Add(1001, 0);
        //    testskilldata.Add(1003, 1);
        //    testskilldata.Add(1005, 1);
        //    testskilldata2.Add(1002, 0);
        //    testskilldata2.Add(1004, 1);
        //    testskilldata2.Add(1006, 1);


        //    FightUnit fighter1 = new FightUnit(1001, 100, 3, 1, 51, false, testskilldata);
        //    FightUnit fighter2 = new FightUnit(1002, 100, 3, 1, 53, false, testskilldata);
        //    FightUnit fighter3 = new FightUnit(1003, 100, 3, 1, 55, false, testskilldata);
        //    FightUnit fighter4 = new FightUnit(1004, 100, 3, 1, 22, false, testskilldata2);
        //    FightUnit fighter5 = new FightUnit(1005, 100, 3, 1, 24, false, testskilldata2);

        //    FightUnit fighter6 = new FightUnit(1006, 100, 3, 1, 51, true, testskilldata);
        //    FightUnit fighter7 = new FightUnit(1007, 100, 3, 1, 53, true, testskilldata);
        //    FightUnit fighter8 = new FightUnit(1008, 100, 3, 1, 55, true, testskilldata);
        //    FightUnit fighter9 = new FightUnit(1009, 100, 3, 1, 12, true, testskilldata2);
        //    FightUnit fighter10 = new FightUnit(1010, 100, 3, 1, 14, true, testskilldata2);

        //    FightUnit fighter11 = new FightUnit(1006, 100, 3, 1, 21, true, testskilldata);
        //    FightUnit fighter12 = new FightUnit(1007, 100, 3, 1, 23, true, testskilldata);
        //    FightUnit fighter13 = new FightUnit(1008, 100, 3, 1, 25, true, testskilldata);
        //    FightUnit fighter14 = new FightUnit(1009, 100, 3, 1, 11, true, testskilldata2);
        //    FightUnit fighter15 = new FightUnit(1010, 100, 3, 1, 15, true, testskilldata2);

        //    List<FightUnit> ownFighter = new List<FightUnit> { fighter1, fighter2, fighter3, fighter4, fighter5 };
        //    List<List<FightUnit>> enemyFighter = new List<List<FightUnit>> { new List<FightUnit>() { fighter6, fighter7, fighter8, fighter9, fighter10 },
        //    new List<FightUnit>() { fighter11, fighter12, fighter13, fighter14, fighter15 }};

        //    FightLogic.Instance.CreateFight(ownFighter, enemyFighter, true);
        //}

        //if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height - 100, 200, 100), "退出"))
        //{
        //    FightLogic.Instance.Clear();
        //    SceneMgr.Instance.LoadScene("Main");
        //}
    }
}
