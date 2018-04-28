using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonData;

public class TestRecruitingUI : MonoBehaviour {

	
	void Start () {
        List<Item> itemlist = new List<Item>();    
        List<Item> itemUnitlist = new List<Item>();
        Item item2 = new Item();
        item2.itemId = 30055;
        item2.itemNum = 1;
        RecruitingMgr.Instance.ServerItem(item2);

        for (int i = 0; i < 10; i++)
        {
            Item item1 = new Item();
            item1.itemId = 30001 + i;
            item1.itemNum = 21 + i;
            itemlist.Add(item1);
        }
        RecruitingMgr.Instance.ServerTen(itemlist);

        List<int> heros = new List<int>();
        for (int i = 0; i < 89; i++)
        {
            int hero = 0;
            hero = 11000 + i * 10;
            heros.Add(hero);
        }
        RecruitingMgr.Instance.ServerHero(heros);

    }
	
	
	void Update () {
		
	}
}
