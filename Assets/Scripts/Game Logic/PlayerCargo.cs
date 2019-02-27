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

    // Start is called before the first frame update
    void Start()
    {
        //Cargo defines how many of each good we have in our inventory
        cargo = new int[Enum.GetNames(typeof(Defs.TradeGoods)).Length];
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
        if (!CargoFull())
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

    public bool CargoFull()
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

    private int TotalCargo()
    {
        int total = 0;
        foreach (var cargoCount in cargo)
        {
            total += cargoCount;
        }

        return total;
    }
}
