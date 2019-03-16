using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defs : Singleton<Defs>
{
    #region Connections
    public enum ConnectionTypes
    {
        CONNECTIONTYPE_NOCONNECTION = -1,
        CONNECTIONTYPE_DEFAULT = 0,
        CONNECTIONTYPE_GREEN = 1,
        CONNECTIONTYPE_RED = 2,
        CONNECTIONTYPE_BLUE = 3
    }
    #endregion
    #region TradeGoods
    public enum TradeGoods
    {
        GOOD_FOOD = 0,
        GOOD_FUEL = 1,
        GOOD_MEDICINE = 2
    }

    public enum TradeGoodTypes
    {
        TRADEGOODTYPE_NONE = -1,
        TRADEGOODTYPE_BUY = 0,
        TRADEGOODTYPE_SELL = 1,
        TRADEGOODTYPE_BOTH = 2
    }

    // Good Scripts
    public Good AddGoodScript(TradeGoods good, GameObject parent)
    {
        switch (good)
        {
            case TradeGoods.GOOD_FOOD:
                return parent.AddComponent<Food>();
            case TradeGoods.GOOD_MEDICINE:
                return parent.AddComponent<Medicine>();
            default:
                return parent.AddComponent<Good>();
        }
    }

    // Good Names
    public Dictionary<TradeGoods, string> goodNames = new Dictionary<TradeGoods, string>() {
        { TradeGoods.GOOD_FOOD, "Food"},
        { TradeGoods.GOOD_FUEL, "Fuel"},
        { TradeGoods.GOOD_MEDICINE, "Medicine"}
    };

    // Good Prices
    public Dictionary<TradeGoods, float> goodStartBuyPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 20f},
        { TradeGoods.GOOD_FUEL, 120f},
        { TradeGoods.GOOD_MEDICINE, 400f}
    };

    public Dictionary<TradeGoods, float> goodStartSellPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 30f},
        { TradeGoods.GOOD_FUEL, 180f},
        { TradeGoods.GOOD_MEDICINE, 400f}
    };

    // Good Min Prices
    public Dictionary<TradeGoods, float> goodMinPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 10f},
        { TradeGoods.GOOD_FUEL, 50f},
        { TradeGoods.GOOD_MEDICINE, 400f}
    };

    // Good Max Prices
    public Dictionary<TradeGoods, float> goodMaxPrice = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 40f},
        { TradeGoods.GOOD_FUEL, 180f},
        { TradeGoods.GOOD_MEDICINE, 500f}
    };

    // Good Prices Flex
    public Dictionary<TradeGoods, float> goodPriceDifference = new Dictionary<TradeGoods, float>() {
        { TradeGoods.GOOD_FOOD, 12f},
        { TradeGoods.GOOD_FUEL, 30f},
        { TradeGoods.GOOD_MEDICINE, 100f}
    };

    //Good Quantities
    public Dictionary<TradeGoods, int> goodStartQuantity = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 15},
        { TradeGoods.GOOD_FUEL, 10},
        { TradeGoods.GOOD_MEDICINE, 2}
    };

    // Good Quantity Flex
    public Dictionary<TradeGoods, int> goodQuantityDifference = new Dictionary<TradeGoods, int>() {
        { TradeGoods.GOOD_FOOD, 10},
        { TradeGoods.GOOD_FUEL, 5},
        { TradeGoods.GOOD_MEDICINE, 2}
    };
    #endregion
    #region EngineUpgrades
    public enum EngineUpgrades
    {
        ENGINEUPRADE_DEFAULT = 0,
        ENGINEUPRADE_GREEN_ONE = 1,
        ENGINEUPRADE_RED_ONE = 2
    }

    // Engine Upgrade Names
    public Dictionary<EngineUpgrades, string> engineUpgradesNames = new Dictionary<EngineUpgrades, string>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, "Engine - Default"},
        { EngineUpgrades.ENGINEUPRADE_GREEN_ONE, "Engine - Green - 1"},
        { EngineUpgrades.ENGINEUPRADE_RED_ONE, "Engine - Red - 1"}
    };

    // Engine Upgrades Speeds
    public Dictionary<EngineUpgrades, float> engineUpgradesSpeeds = new Dictionary<EngineUpgrades, float>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, 1f},
        {EngineUpgrades.ENGINEUPRADE_GREEN_ONE, 0.9f},
        {EngineUpgrades.ENGINEUPRADE_RED_ONE, 0.85f}
    };

    // Engine Upgrade Prices
    public Dictionary<EngineUpgrades, float> engineUpgradesPrices = new Dictionary<EngineUpgrades, float>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, 100f},
        { EngineUpgrades.ENGINEUPRADE_GREEN_ONE, 400f},
        { EngineUpgrades.ENGINEUPRADE_RED_ONE, 350f}
    };

    // Engine Upgrade Prices
    public Dictionary<EngineUpgrades, float> engineUpgradesPiracyChance = new Dictionary<EngineUpgrades, float>() {
        { EngineUpgrades.ENGINEUPRADE_DEFAULT, 0.99f},
        { EngineUpgrades.ENGINEUPRADE_GREEN_ONE, 0.85f},
        { EngineUpgrades.ENGINEUPRADE_RED_ONE, 0.75f}
    };
    #endregion
    #region Events
    public enum Events
    {
        EVENT_PIRATE_ONE = 0,
        EVENT_PIRATE_TWO = 1
    }
    #endregion
}
