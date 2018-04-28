using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShopUI : MonoBehaviour {

    Dictionary<int, int> num = new Dictionary<int, int>();
	void Awake () {
    //    PreGoods();
    }
	
	
	void Update () {
      
	}

    public void PreGoods()
    {
        Dictionary<int, List<Goods>> goodsDic = new Dictionary<int, List<Goods>>();
        for (int i = 0; i < 5; i++)
        {
            List<Goods> _goods = new List<Goods>();            
            for (int j = 0; j < 12; j++)
            {
                Goods goods = new Goods();
                goods.currencyType = Random.Range(1, 3);
                goods.goodsId = 2000 + j;
                goods.goodsNum = 1 + j;
                goods.goodsPrice = i * 100 + j * 10;
                _goods.Add(goods);
            }
            goodsDic.Add(i, _goods);
        }
       // ShopMgr.Instance.Init();
      //  ShopMgr.Instance.ServerGoods(goodsDic);
    }
   
}
