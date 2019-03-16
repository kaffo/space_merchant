using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeGoodSetup : MonoBehaviour
{
    [Header("Good Setup")]
    public List<GoodUIReferences> myGoodUIReferencesList;

    [Header("Upgrade Setup")]
    public List<UpgradeUIReferences> myUpgradeReferencesList;

    [Header("References")]
    public ActiveNode myActiveNodeScript;
    public NodeParameters myNodeParametersScript;
    public GameObject myGoodParentObject;

    // Start is called before the first frame update
    void Start()
    {
        if (myNodeParametersScript == null || myGoodParentObject == null || myGoodUIReferencesList == null || myActiveNodeScript == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject.name + " setup error!");
            this.enabled = false;
            return;
        }

        foreach (GoodUIReferences currentGoodReferenceScript in myGoodUIReferencesList)
        {
            Defs.TradeGoods good = currentGoodReferenceScript.myGood;
            Defs.TradeGoodTypes goodType = currentGoodReferenceScript.myGoodType;

            // Set the Good script up
            Good goodScript = Defs.Instance.AddGoodScript(good, myGoodParentObject);
            if (goodScript != null)
            {
                goodScript.myGood = good;
                goodScript.myGoodType = goodType;
                goodScript.activeNodeScript = myActiveNodeScript;
                goodScript.myUIReferences = currentGoodReferenceScript;
                goodScript.enabled = true;
            }

            Button buyButton = currentGoodReferenceScript.buyButton;
            Button sellButton = currentGoodReferenceScript.sellButton;

            if (buyButton == null || sellButton == null)
            {
                Debug.LogError("Button setup error on " + gameObject.name);
                this.enabled = false;
                return;
            }

            // Setup the click logic
            buyButton.onClick.AddListener(() => GoodsBuyButtonClicked(goodScript));
            sellButton.onClick.AddListener(() => GoodsSellButtonClicked(goodScript));
        }

        foreach (UpgradeUIReferences currentUpgradeReferencesScript in myUpgradeReferencesList)
        {
            Defs.EngineUpgrades engineUpgrade = currentUpgradeReferencesScript.upgrade;

            // Set the Upgrade script up
            Upgrade upgradeScript = myGoodParentObject.AddComponent<Upgrade>();
            if (upgradeScript != null)
            {
                upgradeScript.upgrade = engineUpgrade;
                upgradeScript.activeNodeScript = myActiveNodeScript;
                upgradeScript.buyButton = currentUpgradeReferencesScript.buyButton;
                upgradeScript.buyText = currentUpgradeReferencesScript.buyText;
                upgradeScript.upgradeNameText = currentUpgradeReferencesScript.upgradeNameText;
                upgradeScript.enabled = true;
            }

            Button buyButton = currentUpgradeReferencesScript.buyButton;

            if (buyButton == null)
            {
                Debug.LogError("Button setup error on " + gameObject.name);
                this.enabled = false;
                return;
            }

            // Setup the click logic
            buyButton.onClick.AddListener(() => UpgradeBuyButtonClicked(upgradeScript));
        }
    }

    private void GoodsBuyButtonClicked(Good goodScript)
    {
        goodScript.BuyGood(1);
    }

    private void GoodsSellButtonClicked(Good goodScript)
    {
        goodScript.SellGood(1);
    }

    private void UpgradeBuyButtonClicked(Upgrade upgradeScript)
    {
        upgradeScript.BuyUpgrade();
    }
}
