using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defs : Singleton<Defs>
{
    public enum TradeGoods
    {
        GOOD_FOOD = 0,
        GOOD_FUEL = 1,
        GOOD_FIREWORKS = 2
    }

    // Good Names
    public Dictionary<TradeGoods, string> goodNames = new Dictionary<TradeGoods, string>() {
        { TradeGoods.GOOD_FOOD, "Food"},
        { TradeGoods.GOOD_FUEL, "Fuel"},
        { TradeGoods.GOOD_FIREWORKS, "Fireworks"}
    };

    // Good Prices
    public Dictionary<TradeGoods, float> goodStartPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 100f},
        { TradeGoods.GOOD_FUEL, 120f},
        { TradeGoods.GOOD_FIREWORKS, 300f}
    };

    public Dictionary<TradeGoods, float> goodPriceDifference = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 10f},
        { TradeGoods.GOOD_FUEL, 25f},
        { TradeGoods.GOOD_FIREWORKS, 50f}
    };

    //Good Quantities
    public Dictionary<TradeGoods, int> goodStartQuantity = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 10},
        { TradeGoods.GOOD_FUEL, 10},
        { TradeGoods.GOOD_FIREWORKS, 5}
    };

    public Dictionary<TradeGoods, int> goodQuantityDifference = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 10},
        { TradeGoods.GOOD_FUEL, 5},
        { TradeGoods.GOOD_FIREWORKS, 5}
    };
}
