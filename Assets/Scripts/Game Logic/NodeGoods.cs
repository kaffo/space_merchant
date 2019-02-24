using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGoods : MonoBehaviour
{
    public List<int> prices;

    public Dictionary<Defs.TradeGoods, int> goodPrices;

    // Start is called before the first frame update
    void Start()
    {
        goodPrices = new Dictionary<Defs.TradeGoods, int>();

        //Make sure I have prices for everything - Will remove this
        if (prices.Count != (int)Defs.TradeGoods.GOOD_FIREWORKS + 1)
        {
            Debug.LogError("Prices not set correctly on " + gameObject.name);
        }

        for (int i = 0; i < prices.Count; i++)
        {
            int newPrice = (int)Mathf.Floor(Random.Range(prices[i] - 25, prices[i] + 25));
            goodPrices.Add((Defs.TradeGoods)i, newPrice);
        }
    }
}
