using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defs : Singleton<Defs>
{
    #region TradeGoods
    public enum TradeGoods
    {
        GOOD_FOOD = 0,
        GOOD_FUEL = 1,
        GOOD_FIREWORKS = 2
    }

    // Good Scripts
    public Good AddGoodScript(TradeGoods good, GameObject parent)
    {
        switch (good)
        {
            case TradeGoods.GOOD_FOOD:
                return parent.AddComponent<Food>();
            default:
                return parent.AddComponent<Good>();
        }
    }

    // Good Names
    public Dictionary<TradeGoods, string> goodNames = new Dictionary<TradeGoods, string>() {
        { TradeGoods.GOOD_FOOD, "Food"},
        { TradeGoods.GOOD_FUEL, "Fuel"},
        { TradeGoods.GOOD_FIREWORKS, "Fireworks"}
    };

    // Good Prices
    public Dictionary<TradeGoods, float> goodStartPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 20f},
        { TradeGoods.GOOD_FUEL, 120f},
        { TradeGoods.GOOD_FIREWORKS, 300f}
    };

    // Good Min Prices
    public Dictionary<TradeGoods, float> goodMinPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 10f},
        { TradeGoods.GOOD_FUEL, 50f},
        { TradeGoods.GOOD_FIREWORKS, 175f}
    };

    // Good Max Prices
    public Dictionary<TradeGoods, float> goodMaxPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 30f},
        { TradeGoods.GOOD_FUEL, 180f},
        { TradeGoods.GOOD_FIREWORKS, 525f}
    };

    // Good Prices Flex
    public Dictionary<TradeGoods, float> goodPriceDifference = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 8f},
        { TradeGoods.GOOD_FUEL, 30f},
        { TradeGoods.GOOD_FIREWORKS, 75f}
    };

    //Good Quantities
    public Dictionary<TradeGoods, int> goodStartQuantity = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 15},
        { TradeGoods.GOOD_FUEL, 10},
        { TradeGoods.GOOD_FIREWORKS, 5}
    };

    // Good Quantity Flex
    public Dictionary<TradeGoods, int> goodQuantityDifference = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 10},
        { TradeGoods.GOOD_FUEL, 5},
        { TradeGoods.GOOD_FIREWORKS, 5}
    };
    #endregion
    #region Upgrades
    public enum EngineUpgrades
    {
        ENGINEUPRADE_DEFAULT = 0,
        ENGINEUPRADE_GREEN_ONE = 1,        
    }

    // Engine Upgrade Names
    public Dictionary<EngineUpgrades, string> engineUpgradesNames = new Dictionary<EngineUpgrades, string>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, "Engine - Default"},
        { EngineUpgrades.ENGINEUPRADE_GREEN_ONE, "Engine - Green - 1"}
    };

    // Engine Upgrades Speeds
    public Dictionary<EngineUpgrades, float> engineUpgradesSpeeds = new Dictionary<EngineUpgrades, float>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, 1f},
        {EngineUpgrades.ENGINEUPRADE_GREEN_ONE, 0.9f}
    };

    // Engine Upgrade Prices
    public Dictionary<EngineUpgrades, float> engineUpgradesPrices = new Dictionary<EngineUpgrades, float>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, 100f},
        { EngineUpgrades.ENGINEUPRADE_GREEN_ONE, 400f}
    };
    #endregion
}
