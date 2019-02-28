using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButtonUIHandler : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public Good myGoodScript;

    // Start is called before the first frame update
    void Start()
    {
        if (myGoodScript == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IEnumerator currentSellPriceMethod;
        foreach (var goodScript in ObjectManager.Instance.globalGoodList)
        {
            currentSellPriceMethod = goodScript.CheckSellPriceCheaper(myGoodScript.good, myGoodScript.getBuyPrice());
            StartCoroutine(currentSellPriceMethod);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var goodScript in ObjectManager.Instance.globalGoodList)
        {
            goodScript.SetSellButtonHilight(false);
        }
    }
}
