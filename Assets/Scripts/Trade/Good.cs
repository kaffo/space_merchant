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
    private Outline sellButtonOutline;

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
        sellButtonOutline = myUIReferences.sellButtonOutline;

        // Setup and enable Buy Button UI handler
        myUIReferences.buyButtonUIHandler.myGoodScript = this;
        myUIReferences.buyButtonUIHandler.enabled = true;

        if (goodText == null || buyText == null || buyButton  == null || sellText == null || sellButton == null
            || sellButtonOutline == null)
        {
            Debug.LogError("Button Text can't be found on " + gameObject.name);
            this.enabled = false;
            return;
        }

        updateUI();
    }

    public void updateUI()
    {
        goodText.text = Defs.Instance.goodNames[good];
        buyText.text = buyQuantity + " - B - $" + buyPrice;
        sellText.text = sellQuantity + " - S - $" + sellPrice;

        //Only activate trade UI on active node
        if (activeNodeScript.getActive())
        {
            int playerCash = PlayerMoney.Instance.getPlayerCash();

            // Set buy button state
            if (buyQuantity > 0 && !PlayerCargo.Instance.CargoFull() && playerCash >= buyPrice && !TimeCounter.Instance.gameOver)
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }

            // Set sell button state
            if (sellQuantity > 0 && PlayerCargo.Instance.HasGood(good) && !TimeCounter.Instance.gameOver)
            {
                sellButton.interactable = true;
            }
            else
            {
                sellButton.interactable = false;
            }
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
        if (!playerMoney.CheckCash(playerMoney.getPlayerCash() - quantity * buyPrice))
        {
            Debug.Log("Purchase Failed");
            return false;
        }
        
        if (!PlayerCargo.Instance.CargoFull() && incrementBuyQuantity(-quantity))
        {
            playerMoney.IncrementPlayerCash(-buyPrice);
            PlayerCargo.Instance.AddSingleCargo(good);
            Debug.Log("Bought " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + buyPrice * quantity);
            setBuyPrice(buyPrice + (int)((float)buyPrice / 100f * 10f));

            // Pass time for each unit bought
            TimeCounter.Instance.passTime(quantity);

            // Disable any outlines to avoid player confusion
            foreach (var goodScript in ObjectManager.Instance.globalGoodList)
            {
                goodScript.SetSellButtonHilight(false);
            }
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
            PlayerMoney.Instance.IncrementPlayerCash(sellPrice);
            PlayerCargo.Instance.RemoveSingleCargo(good);
            Debug.Log("Sold " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + sellPrice * quantity);
            setSellPrice(sellPrice - (int)((float)sellPrice / 100f * 10f));
            TimeCounter.Instance.passTime(quantity);
            return true;
        } else
        {
            return false;
        }
    }

    public IEnumerator CheckSellPriceCheaper(Defs.TradeGoods goodToCheck, int priceToCheck)
    {
        if (goodToCheck == good && sellQuantity > 1 && sellPrice > priceToCheck)
        {
            SetSellButtonHilight(true);
        } else
        {
            SetSellButtonHilight(false);
        }
        yield return null;
    }

    public void SetSellButtonHilight(bool valueToSet)
    {
        sellButtonOutline.enabled = valueToSet;
    }

    public IEnumerator StepEconomy(int timesToStep = 1)
    {
        for (int i = 0; i < timesToStep; i++)
        {
            // A low chance that something happens to each value
            if (Random.value > 0.99)
            {
                incrementBuyQuantity(1);
                setBuyPrice(buyPrice - (int)((float)buyPrice / 100f * 10f));
            }
            if (Random.value > 0.99)
            {
                incrementSellQuantity(1);
                setSellPrice(sellPrice + (int)((float)sellPrice / 100f * 10f));
            }
        }
        updateUI();
        yield return null;
    }
}
