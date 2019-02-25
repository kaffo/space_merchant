using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    public Defs.TradeGoods good;
    private int buyPrice;
    private int sellPrice;
    private int quantity;

    private Text goodText;

    // Start is called before the first frame update
    void Start()
    {
        // Determine Starting Buy Price
        float startPrice = Defs.Instance.goodStartPrice[good];
        float pDifference = Defs.Instance.goodPriceDifference[good];
        buyPrice = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));

        // Determine Starting Sell Price
        sellPrice = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));

        // Determine Starting Price
        int startQuatity = Defs.Instance.goodStartQuantity[good];
        int qDifference = Defs.Instance.goodQuantityDifference[good];
        quantity = (int)Mathf.Floor(Random.Range(startQuatity - qDifference, startQuatity + qDifference));

        // Grab the text element
        Transform buttonTextTransform = transform.GetChild(0);
        goodText = buttonTextTransform.GetComponent<Text>();
        if (goodText == null)
        {
            Debug.LogError("Button Text can't be found on " + gameObject.name);
            this.enabled = false;
            return;
        }

        updateUI();
    }

    public void updateUI()
    {
        // Set the text
        goodText.text = Defs.Instance.goodNames[good] + " - B" + buyPrice + " - S" + sellPrice + " - " + quantity;
    }

    public int getBuyPrice()
    {
        return buyPrice;
    }

    public void setBuyPrice(int newPrice)
    {
        buyPrice = newPrice;
        updateUI();
    }

    public int getSellPrice()
    {
        return sellPrice;
    }

    public void setSellPrice(int newPrice)
    {
        sellPrice = newPrice;
        updateUI();
    }

    public int getQuantity()
    {
        return quantity;
    }

    public void setQuantity(int newQuantity)
    {
        quantity = newQuantity;
        updateUI();
    }

    public bool incrementQuantity(int incrementQuantity)
    {
        if (quantity + incrementQuantity >= 0)
        {
            quantity += incrementQuantity;
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
        
        if (incrementQuantity(-quantity))
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
        if (PlayerCargo.Instance.HasGood(good))
        {
            PlayerMoney.Instance.incrementPlayerCash(sellPrice);
            PlayerCargo.Instance.RemoveSingleCargo(good);
            Debug.Log("Sold " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + sellPrice * quantity);
            setSellPrice(sellPrice - 10);
            return true;
        } else
        {
            return false;
        }
    }
}
