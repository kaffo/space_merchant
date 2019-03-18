using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Good
{
    public override IEnumerator StepEconomy(int timesToStep = 1)
    {
        for (int i = 0; i < timesToStep; i++)
        {
            // If both, empty reserves first
            if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH && buyQuantity > 0 && sellQuantity > 0)
            {
                incrementBuyQuantity(-1);
                setBuyPrice(buyPrice + (int)((float)buyPrice / 100f * 10f));
                incrementSellQuantity(-1);
                setSellPrice(sellPrice - (int)((float)sellPrice / 100f * 10f));
            }

            // Food moves fast, high produce and high sell
            // If both, either produce up to 5 buy or up to 5 sell
            if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BOTH && Random.value > 0.97)
            {
                float diceRoll = Random.value;
                // Increment Buy
                if (diceRoll < 0.50)
                {
                    int buyIncrease = Mathf.CeilToInt(diceRoll * 10f);
                    if (incrementSellQuantity(-buyIncrease))
                    {
                        setSellPrice(sellPrice - (int)((float)sellPrice / 100f * 10f));
                    }
                    else
                    {
                        incrementBuyQuantity(buyIncrease);
                        setBuyPrice(buyPrice - (int)((float)buyPrice / 100f * 10f));
                    }
                }
                else if (diceRoll > 0.50)
                {
                    int sellIncrease = Mathf.CeilToInt((diceRoll - 0.5f) * 10f);
                    if (incrementBuyQuantity(-sellIncrease))
                    {
                        setBuyPrice(buyPrice + (int)((float)buyPrice / 100f * 10f));
                    }
                    else
                    {
                        incrementSellQuantity(sellIncrease);
                        setSellPrice(sellPrice + (int)((float)sellPrice / 100f * 10f));
                    }
                }
            }
            // Food moves fast, high produce and high sell
            // A low chance that something happens to each value
            if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_BUY && Random.value > 0.97)
            {
                int buyIncrease = Mathf.CeilToInt(Random.value * 5f);
                incrementBuyQuantity(buyIncrease);
                setBuyPrice(buyPrice - (int)((float)buyPrice / 100f * 10f));
            }
            if (myGoodType == Defs.TradeGoodTypes.TRADEGOODTYPE_SELL && Random.value > 0.97)
            {
                int sellIncrease = Mathf.CeilToInt(Random.value * 5f);
                incrementSellQuantity(sellIncrease);
                setSellPrice(sellPrice + (int)((float)sellPrice / 100f * 10f));
            }
        }
        updateUI();
        yield return null;
    }
}