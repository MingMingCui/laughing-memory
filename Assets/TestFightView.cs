using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFightView : MonoBehaviour {

    public FightUnit unit = null;
    public int GridPos = -1;
    Color32 lineColor = new Color32();
    Material mat = null;
	// Use this for initialization
	void Start () {
        lineColor.r = (byte)Random.Range(0, 255);
        lineColor.g = (byte)Random.Range(0, 255);
        lineColor.b = (byte)Random.Range(0, 255);
    }
	
	// Update is called once per frame
	void Update () {
        if (unit != null)
        {
            this.transform.position = new Vector3(this.unit.CurPos.x, 0, unit.CurPos.y);
            this.transform.rotation = this.unit.CurRot;
            if (mat == null)
            {
                mat = this.transform.Find("Cylinder").GetComponent<MeshRenderer>().material;
            }
            mat.SetColor("_Color", unit.IsEnemy ? new Color32(255, 0, 0, 255) : new Color32(0, 255, 0, 255));
            this.GridPos = unit.GridPos;
            //if(this.unit.FightTarget != null)
             //   Debug.DrawLine(new Vector3(this.unit.CurPos.x, 1, unit.CurPos.y), new Vector3(this.unit.SkillTargetPos.x, 1, this.unit.SkillTargetPos.y));
            Vector2 targetPos = PathFinder.Grid2Pos(this.unit.GridPos);
            Debug.DrawLine(new Vector3(this.unit.CurPos.x, 1, unit.CurPos.y), new Vector3(targetPos.x, 1, targetPos.y), Color.red);
            if (unit.IsDead)
            {
                this.gameObject.SetActive(false);
            }
        }
	}
}
