using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    [Header("Good Setup")]
    public Defs.TradeGoods good;
    
    [Header("Scripts")]
    public ActiveNode activeNodeScript;
    public GoodUIReferences myUIReferences;

    private int buyPrice;
    private int sellPrice;
    private int buyQuantity;
    private int sellQuantity;

    private Text goodText;
    private Text buyText;
    private Button buyButton;
    private Text sellText;
    private Button sellButton;

    // Start is called before the first frame update
    void Start()
    {
        if (activeNodeScript == null || myUIReferences == null)
        {
            Debug.LogError("Good script error on " + gameObject.name);
            this.enabled = false;
            return;
        }

        // Add myself to the global good list
        ObjectManager.Instance.globalGoodList.Add(this);

        // Determine Starting Buy Price
        float startPrice = Defs.Instance.goodStartPrice[good];
        float pDifference = Defs.Instance.goodPriceDifference[good];
        buyPrice = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));

        // Determine Starting Sell Price
        sellPrice = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));

        // Determine Starting Price
        int startQuatity = Defs.Instance.goodStartQuantity[good];
        int qDifference = Defs.Instance.goodQuantityDifference[good];
        buyQuantity = (int)Mathf.Floor(Random.Range(startQuatity - qDifference, startQuatity + qDifference));
        sellQuantity = (int)Mathf.Floor(Random.Range(startQuatity - qDifference, startQuatity + qDifference));

        // Get UI elements from reference script
        goodText = myUIReferences.goodText;
        buyText = myUIReferences.buyText;
        buyButton = myUIReferences.buyButton;
        sellText = myUIReferences.sellText;
        sellButton = myUIReferences.sellButton;

        if (goodText == null || buyText == null || buyButton  == null || sellText == null || sellButton == null)
        {
            Debug.LogError("Button Text can't be found on " + gameObject.name);
            this.enabled = false;
            return;
        }

        updateUI();
    }

    public void updateUI()
    {
        // Set good text
        goodText.text = Defs.Instance.goodNames[good];

        // Set buy text and button state
        buyText.text = buyQuantity + " - B - $" + buyPrice;
        if (buyQuantity > 0 && activeNodeScript.getActive() && !TimeCounter.Instance.gameOver)
        {
            buyButton.interactable = true;
        } else
        {
            buyButton.interactable = false;
        }

        // Set sell text and button state
        sellText.text = sellQuantity + " - S - $" + sellPrice;
        if (sellQuantity > 0 && activeNodeScript.getActive() && !TimeCounter.Instance.gameOver)
        {
            sellButton.interactable = true;
        }
        else
        {
            sellButton.interactable = false;
        }
    }

    public int getBuyPrice()
    {
        return buyPrice;
    }

    public void setBuyPrice(int newPrice)
    {
        buyPrice = (int)Mathf.Clamp(newPrice, Defs.Instance.goodMinPrice[good], Defs.Instance.goodMaxPrice[good]);
        updateUI();
    }

    public int getSellPrice()
    {
        return sellPrice;
    }

    public void setSellPrice(int newPrice)
    {
        sellPrice = (int)Mathf.Clamp(newPrice, Defs.Instance.goodMinPrice[good], Defs.Instance.goodMaxPrice[good]);
        updateUI();
    }

    public int getBuyQuantity()
    {
        return buyQuantity;
    }

    public void setBuyQuantity(int newQuantity)
    {
        buyQuantity = newQuantity;
        updateUI();
    }

    public bool incrementBuyQuantity(int incrementQuantity)
    {
        if (buyQuantity + incrementQuantity >= 0)
        {
            buyQuantity += incrementQuantity;
            updateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getSellQuantity()
    {
        return sellQuantity;
    }

    public void setSellQuantity(int newQuantity)
    {
        sellQuantity = newQuantity;
        updateUI();
    }

    public bool incrementSellQuantity(int incrementQuantity)
    {
        if (sellQuantity + incrementQuantity >= 0)
        {
            sellQuantity += incrementQuantity;
            updateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuyGood(int quantity)
    {
        PlayerMoney playerMoney = PlayerMoney.Instance;
        if (!playerMoney.checkCash(playerMoney.getPlayerCash() - quantity * buyPrice))
        {
            Debug.Log("Purchase Failed");
            return false;
        }
        
        if (incrementBuyQuantity(-quantity))
        {
            playerMoney.incrementPlayerCash(-buyPrice);
            PlayerCargo.Instance.AddSingleCargo(good);
            Debug.Log("Bought " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + buyPrice * quantity);
            setBuyPrice(buyPrice + (int)((float)buyPrice / 100f * 10f));
            TimeCounter.Instance.StepEconomy(quantity);
            return true;
        } else
        {
            Debug.Log("Not enough goods to purchase");
            return false;
        }
    }

    public bool SellGood(int quantity)
    {
        // Check player cargo first
        if (PlayerCargo.Instance.HasGood(good) && incrementSellQuantity(-quantity))
        {
            PlayerMoney.Instance.incrementPlayerCash(sellPrice);
            PlayerCargo.Instance.RemoveSingleCargo(good);
            Debug.Log("Sold " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + sellPrice * quantity);
            setSellPrice(sellPrice - (int)((float)sellPrice / 100f * 10f));
            TimeCounter.Instance.StepEconomy(quantity);
            return true;
        } else
        {
            return false;
        }
    }

    public IEnumerator StepEconomy(int timesToStep = 1)
    {
        timesToStep = timesToStep / 10;
        for (int i = 0; i < timesToStep; i++)
        {
            // A low chance that something happens to each value
            if (Random.value > 0.95)
            {
                incrementBuyQuantity(1);
                setBuyPrice(buyPrice - (int)((float)buyPrice / 100f * 10f));
            }
            if (Random.value > 0.95)
            {
                incrementSellQuantity(1);
                setSellPrice(sellPrice + (int)((float)sellPrice / 100f * 10f));
            }
        }
        yield return null;
    }
}
