using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    public Defs.TradeGoods good;
    private int price;
    private int quantity;

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
        Text buttonText = buttonTextTransform.GetComponent<Text>();

        // Set the text
        if (buttonText != null) { buttonText.text = Defs.Instance.goodNames[good] + " - $" + price + " - " + quantity; }
    }

    int getPrice()
    {
        return price;
    }

    void setPrice(int newPrice)
    {
        price = newPrice;
    }

    int getQuantity()
    {
        return quantity;
    }

    void setQuantity(int newQuantity)
    {
        quantity = newQuantity;
    }
}
