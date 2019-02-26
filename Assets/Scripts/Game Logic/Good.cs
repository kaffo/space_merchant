using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    [Header("Good Setup")]
    public Defs.TradeGoods good;

    [Header("UI Elements")]
    public Text buyText;
    public Button buyButton;
    public Text sellText;
    public Button sellButton;

    [Header("Scripts")]
    public ActiveNode activeNodeScript;

    private int buyPrice;
    private int sellPrice;
    private int buyQuantity;
    private int sellQuantity;

    private Text goodText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (activeNodeScript == null)
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

        // Grab the text element
        Transform buttonTextTransform = transform.GetChild(0);
        goodText = buttonTextTransform.GetComponent<Text>();
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
        buyText.text = buyQuantity + " - B - " + buyPrice;
        if (buyQuantity > 0 && activeNodeScript.getActive())
        {
            buyButton.interactable = true;
        } else
        {
            buyButton.interactable = false;
        }

        // Set sell text and button state
        sellText.text = sellQuantity + " - S - " + sellPrice;
        if (sellQuantity > 0 && activeNodeScript.getActive())
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
            setBuyPrice(buyPrice + 10);
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
            //TODO subclass price change
            setSellPrice(sellPrice - 10);
            return true;
        } else
        {
            return false;
        }
    }
}
