using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCargo : Singleton<PlayerCargo>
{
    public int size = 6;
    public Text cargoText;

    private int[] cargo;
    private Defs.EngineUpgrades currentEngine = Defs.EngineUpgrades.ENGINEUPRADE_DEFAULT;

    // Start is called before the first frame update
    void Start()
    {
        //Cargo defines how many of each good we have in our inventory
        cargo = new int[Enum.GetNames(typeof(Defs.TradeGoods)).Length];
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (cargoText == null)
        {
            return;
        };

        string startText = "Cargo - ";
        string newText = "";
        int total = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Defs.TradeGoods)).Length; i++)
        {
            if (cargo[i] > 0)
            {
                total += cargo[i];
                newText = newText + "\n" + Defs.Instance.goodNames[(Defs.TradeGoods)i] + " - " + cargo[i];
            }
        }

        startText = startText + total.ToString() + "/" + size;
        cargoText.text = startText + newText;
    }

    public bool AddSingleCargo(Defs.TradeGoods good)
    {
        if (!IsCargoFull())
        {
            Debug.Log("Adding new " + Defs.Instance.goodNames[good] + " to cargo");
            cargo[(int)good]++;
            UpdateUI();
            return true;
        } else
        {
            return false;
        }
    }

    public bool RemoveSingleCargo(Defs.TradeGoods good)
    {
        if (HasGood(good))
        {
            Debug.Log("Removing " + Defs.Instance.goodNames[good] + " from cargo");
            cargo[(int)good]--;
            UpdateUI();
            return true;
        } else
        {
            return false;
        }
    }

    public void EmptyCargo()
    {
        cargo = new int[Enum.GetNames(typeof(Defs.TradeGoods)).Length];
        UpdateUI();
    }

    public bool IsCargoFull()
    {
        if (TotalCargo() >= size)
        {
            Debug.Log("Cargo is full, cannot add new item");
            return true;
        } else
        {
            return false;
        }
    }

    public bool HasGood(Defs.TradeGoods good)
    {
        if (cargo[(int)good] > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public int TotalCargo()
    {
        int total = 0;
        foreach (var cargoCount in cargo)
        {
            total += cargoCount;
        }

        return total;
    }

    public bool PlayerHasUpgrade(Defs.EngineUpgrades engineUpgrade)
    {
        return currentEngine == engineUpgrade;
    }

    public Defs.EngineUpgrades GetCurrentEngine()
    {
        return currentEngine;
    }

    public void UpgradeCurrentEngine(Defs.EngineUpgrades engineUpgrade)
    {
        currentEngine = engineUpgrade;
        foreach (ActiveNode currentNode in ObjectManager.Instance.globalNodeList)
        {
            GameObject currentNodeConnectionsGO = currentNode.connections;
            Connection[] currentNodeConnectionsList = currentNodeConnectionsGO.GetComponentsInChildren<Connection>();
            foreach (Connection currentNodeConnectionScript in currentNodeConnectionsList)
            {
                IEnumerator enumerable = currentNodeConnectionScript.EngineUpgrade(engineUpgrade);
                StartCoroutine(enumerable);
            }
        }
    }
}
