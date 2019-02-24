using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGoods : MonoBehaviour
{
    public List<int> prices;

    public enum TradeGoods
    {
        GOOD_FOOD = 0,
        GOOD_FUEL = 1,
        GOOD_FIREWORKS = 2
    }

    public Dictionary<TradeGoods, string> goodNames;

    public Dictionary<TradeGoods, int> goodPrices;

    // Start is called before the first frame update
    void Start()
    {
        goodPrices = new Dictionary<TradeGoods, int>();
        goodNames = new Dictionary<TradeGoods, string>() {
            { TradeGoods.GOOD_FOOD, "Food"},
            { TradeGoods.GOOD_FUEL, "Fuel"},
            { TradeGoods.GOOD_FIREWORKS, "Fireworks"}
        };

        //Make sure I have prices for everything - Will remove this
        if (prices.Count != (int)TradeGoods.GOOD_FIREWORKS + 1)
        {
            Debug.LogError("Prices not set correctly on " + gameObject.name);
        }

        for (int i = 0; i < prices.Count; i++)
        {
            int newPrice = (int)Mathf.Floor(Random.Range(prices[i] - 25, prices[i] + 25));
            goodPrices.Add((TradeGoods)i, newPrice);
        }
    }
}
