using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaNodeView : MonoBehaviour {

    public Image head_img;
    public Text level_txt;
    public Text name_txt;
    public Text rank_txt;
    public Text fightpower_txt;
    public Button fight_btn;

    private uint _roleId = 0;
    public void SetInfo(uint roleId, int headId, string roleName, int level, int rank, int fightPower)
    {
        this._roleId = roleId;
        //TODO:headId;
        this.level_txt.text = level.ToString();
        this.name_txt.text = roleName;
        this.rank_txt.text = string.Format("排名:{0}", rank);
        this.fightpower_txt.text = string.Format("战斗力:{0}", fightPower);
        EventListener.Get(this.fight_btn.gameObject).OnClick = e => {
            //TODO:挑战roleid
            Debug.LogFormat("挑战 {0}", this._roleId);
        };
    }
}
