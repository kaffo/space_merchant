using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    [Header("Good Setup")]
    public Defs.TradeGoods myGood;
    public Defs.TradeGoodTypes myGoodType;
    
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
        float startPrice = Defs.Instance.goodStartPrice[myGood];
        float pDifference = Defs.Instance.goodPriceDifference[myGood];
        float minPrice = Defs.Instance.goodMinPrice[myGood];
        float maxPrice = Defs.Instance.goodMaxPrice[myGood];
        buyPrice = (int)Mathf.Clamp(Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference)), minPrice, maxPrice);

        // Determine Starting Sell Price
        sellPrice = (int)Mathf.Clamp(Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference)), minPrice, maxPrice);

        // Determine Starting Price
        int startQuatity = Defs.Instance.goodStartQuantity[myGood];
        int qDifference = Defs.Instance.goodQuantityDifference[myGood];
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

        // Disable UI depending on buying or selling
        if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BUY || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_NONE)
        {
            sellButton.gameObject.SetActive(false);
        }

        if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_SELL || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_NONE)
        {
            buyButton.gameObject.SetActive(false);
        }

        updateUI();
    }

    public void updateUI()
    {
        goodText.text = Defs.Instance.goodNames[myGood];
        buyText.text = buyQuantity + " - B - $" + buyPrice;
        sellText.text = sellQuantity + " - S - $" + sellPrice;

        //Only activate trade UI on active node
        if (activeNodeScript.getActive())
        {
            int playerCash = PlayerMoney.Instance.GetPlayerCash();

            // Set buy button state
            if (buyQuantity > 0 && !PlayerCargo.Instance.IsCargoFull() && playerCash >= buyPrice && !TimeCounter.Instance.gameOver)
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }

            // Set sell button state
            if (sellQuantity > 0 && PlayerCargo.Instance.HasGood(myGood) && !TimeCounter.Instance.gameOver)
            {
                sellButton.interactable = true;
            }
            else
            {
                sellButton.interactable = false;
            }
        } else
        {
            buyButton.interactable = false;
            sellButton.interactable = false;
        }
    }

    public int getBuyPrice()
    {
        return buyPrice;
    }

    public void setBuyPrice(int newPrice)
    {
        buyPrice = (int)Mathf.Clamp(newPrice, Defs.Instance.goodMinPrice[myGood], Defs.Instance.goodMaxPrice[myGood]);
        updateUI();
    }

    public int getSellPrice()
    {
        return sellPrice;
    }

    public void setSellPrice(int newPrice)
    {
        sellPrice = (int)Mathf.Clamp(newPrice, Defs.Instance.goodMinPrice[myGood], Defs.Instance.goodMaxPrice[myGood]);
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
        if (!(myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BUY || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH))
        {
            Debug.LogError("Tried to buy " + Defs.Instance.goodNames[myGood] + " but this Good is " + myGoodType);
            return false;
        }

        PlayerMoney playerMoney = PlayerMoney.Instance;
        if (!playerMoney.CheckCash(playerMoney.GetPlayerCash() - quantity * buyPrice))
        {
            Debug.Log("Purchase Failed");
            return false;
        }
        
        if (!PlayerCargo.Instance.IsCargoFull() && incrementBuyQuantity(-quantity))
        {
            playerMoney.IncrementPlayerCash(-buyPrice);
            PlayerCargo.Instance.AddSingleCargo(myGood);
            Debug.Log("Bought " + quantity + " " + Defs.Instance.goodNames[myGood] + " for $" + buyPrice * quantity);
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
        if (!(myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_SELL || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH))
        {
            Debug.LogError("Tried to sell " + Defs.Instance.goodNames[myGood] + " but this Good is " + myGoodType);
            return false;
        }

        // Check player cargo first
        if (PlayerCargo.Instance.HasGood(myGood) && incrementSellQuantity(-quantity))
        {
            PlayerMoney.Instance.IncrementPlayerCash(sellPrice);
            PlayerCargo.Instance.RemoveSingleCargo(myGood);
            Debug.Log("Sold " + quantity + " " + Defs.Instance.goodNames[myGood] + " for $" + sellPrice * quantity);
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
        if (goodToCheck == myGood && sellQuantity > 1 && sellPrice > priceToCheck)
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
            if (Random.value > 0.99 && (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BUY || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH))
            {
                incrementBuyQuantity(1);
                setBuyPrice(buyPrice - (int)((float)buyPrice / 100f * 10f));
            }
            if (Random.value > 0.99 && (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_SELL || myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH))
            {
                incrementSellQuantity(1);
                setSellPrice(sellPrice + (int)((float)sellPrice / 100f * 10f));
            }
        }
        updateUI();
        yield return null;
    }
}
