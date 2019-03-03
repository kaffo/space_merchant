using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("Upgrade Setup")]
    public Defs.EngineUpgrades upgrade;

    [Header("UI Elements")]
    public Text buyText;
    public Button buyButton;

    [Header("Scripts")]
    public ActiveNode activeNodeScript;

    private int buyPrice;

    private Text upgradeText;

    // Start is called before the first frame update
    void Start()
    {
        if (activeNodeScript == null)
        {
            Debug.LogError("Upgrade script error on " + gameObject.name);
            this.enabled = false;
            return;
        }

        // Add myself to the global good list
        ObjectManager.Instance.globalUpgradeList.Add(this);

        // Determine Starting Buy Price
        float startPrice = Defs.Instance.engineUpgradesPrices[upgrade];
        /* Add in variable pricing?
        float pDifference = Defs.Instance.goodPriceDifference[upgrade];
        buyPrice = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));*/
        buyPrice = (int)startPrice;

        // Grab the text element
        Transform buttonTextTransform = transform.GetChild(0);
        upgradeText = buttonTextTransform.GetComponent<Text>();
        if (upgradeText == null || buyText == null || buyButton == null)
        {
            Debug.LogError("Button Text can't be found on " + gameObject.name);
            this.enabled = false;
            return;
        }

        updateUI();
    }

    public void updateUI()
    {
        // Set upgrade text
        upgradeText.text = Defs.Instance.engineUpgradesNames[upgrade];

        // Set buy text and button state
        buyText.text = "$" + buyPrice;
        if (activeNodeScript.getActive() && !PlayerCargo.Instance.PlayerHasUpgrade(upgrade) && !TimeCounter.Instance.gameOver)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }

    public int getBuyPrice()
    {
        return buyPrice;
    }

    public void setBuyPrice(int newPrice)
    {
        /* Price difference?
        buyPrice = (int)Mathf.Clamp(newPrice, Defs.Instance.goodMinPrice[upgrade], Defs.Instance.goodMaxPrice[upgrade]); */
        buyPrice = newPrice;
        updateUI();
    }

    public bool BuyUpgrade()
    {
        PlayerMoney playerMoney = PlayerMoney.Instance;
        if (!playerMoney.CheckCash(playerMoney.GetPlayerCash() - buyPrice))
        {
            Debug.Log("Purchase Failed");
            return false;
        }

        playerMoney.IncrementPlayerCash(-buyPrice);
        PlayerCargo.Instance.UpgradeCurrentEngine(upgrade);
        Debug.Log("Bought " + Defs.Instance.engineUpgradesNames[upgrade] + " for $" + buyPrice);
        updateUI();
        TimeCounter.Instance.StepEconomy(1);
        return true;
    }
}
