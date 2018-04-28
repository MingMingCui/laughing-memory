using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.CorpsMsg;

public class CorpsView : UICorpsViewBase {

    public RectTransform JoinNode = null;

    private List<CorpsNodeView> _joinNodeBuffer = new List<CorpsNodeView>();

    public void InitCorpsList()
    {
        List<CorpsInfo> corpList = CorpsMgr.Instance.CorpsList;
        int needCreateNode = corpList.Count - _joinNodeBuffer.Count;

        if (needCreateNode > 0)
        {
            for (int idx = 0; idx < needCreateNode; ++idx)
            {
                GameObject node = GameObject.Instantiate(JoinNode.gameObject, this.corpsnodes_obj.transform);
                CorpsNodeView nodeView = node.GetComponent<CorpsNodeView>();
                _joinNodeBuffer.Add(nodeView);
            }
        }
        for (int idx = 0; idx < _joinNodeBuffer.Count; ++idx)
        {
            CorpsNodeView nodeView = _joinNodeBuffer[idx];
            if (idx < corpList.Count)
            {
                CorpsInfo info = corpList[idx];
                nodeView.SetInfo(info.UID, info.flag, info.camp, info.name, info.level, info.members, info.leader, info.limit, info.state);
                nodeView.gameObject.SetActive(true);
            }
            else
                nodeView.gameObject.SetActive(false);
        }
        
        RectTransform nodesRect = this.corpsnodes_obj.GetComponent<RectTransform>();
        nodesRect.sizeDelta = new Vector2(nodesRect.sizeDelta.x, corpList.Count * JoinNode.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void ChangeFlag(int flagid)
    {
        this.flag_img.sprite = ResourceMgr.Instance.LoadSprite(flagid);
    }

    public void SetCost(int gold, bool enough)
    {
        this.cost_txt.text = gold.ToString();
        this.cost_txt.color = enough ? ColorMgr.EnoughGreen : ColorMgr.LackRed;
    }
}

