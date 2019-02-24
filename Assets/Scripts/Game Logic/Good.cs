using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    public Defs.TradeGoods good;
    private int price;
    private int quantity;

    private Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        // Determine Starting Price
        float startPrice = Defs.Instance.goodStartPrice[good];
        float pDifference = Defs.Instance.goodPriceDifference[good];
        price = (int)Mathf.Floor(Random.Range(startPrice - pDifference, startPrice + pDifference));

        // Determine Starting Price
        int startQuatity = Defs.Instance.goodStartQuantity[good];
        int qDifference = Defs.Instance.goodQuantityDifference[good];
        quantity = (int)Mathf.Floor(Random.Range(startQuatity - qDifference, startQuatity + qDifference));

        // Grab the text element
        Transform buttonTextTransform = transform.GetChild(0);
        buttonText = buttonTextTransform.GetComponent<Text>();
        if (buttonText == null)
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
        buttonText.text = Defs.Instance.goodNames[good] + " - $" + price + " - " + quantity;
    }

    public int getPrice()
    {
        return price;
    }

    public void setPrice(int newPrice)
    {
        price = newPrice;
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

    public bool buyGood(int quantity)
    {
        PlayerMoney playerMoney = PlayerMoney.Instance;
        if (!playerMoney.checkCash(playerMoney.getPlayerCash() - quantity * price))
        {
            Debug.Log("Purchase Failed");
            return false;
        }
        
        if (incrementQuantity(-quantity))
        {
            playerMoney.incrementPlayerCash(-price);
            Debug.Log("Bought " + quantity + " " + Defs.Instance.goodNames[good] + " for $" + price * quantity);
            setPrice(price + 10);
            return true;
        } else
        {
            Debug.Log("Not enough goods to purchase");
            return false;
        }
    }
}
