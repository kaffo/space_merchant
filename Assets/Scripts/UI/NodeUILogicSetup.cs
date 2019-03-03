using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUILogicSetup : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject panelCanvasParentObject;
    public GameObject nodeNameCanvasObject;

    [Header("UI Elements")]
    public Button jumpButton;
    public GameObject goodsParent;
    public Text nodeNameText;

    [Header("Scripts")]
    public NodeParameters nodeParametersScript;
    public ActiveNode nodeScript;
    public NodeConnections myConnections;
    public NodeClick nodeClickScript;

    [Header("Prefabs")]
    public GameObject goodsUIParent;
    public GameObject upgradeUIParent;
    public GameObject panelPrefab;
    public GameObject nodeNamePrefab;

    private NodePanelReferences myPanelReferenceScript;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (panelCanvasParentObject != null && nodeNameCanvasObject != null) { break; }
            GameObject currentChild = transform.parent.GetChild(i).gameObject;
            if (currentChild.CompareTag("PanelCanvas")) { panelCanvasParentObject = currentChild; }
            if (currentChild.CompareTag("NodeNameCanvas")) { nodeNameCanvasObject = currentChild; }
        }

        if (nodeParametersScript == null || nodeScript == null || myConnections == null || goodsUIParent == null || panelPrefab == null
            || upgradeUIParent == null || panelCanvasParentObject == null || nodeNameCanvasObject  == null || nodeClickScript == null || nodeNamePrefab == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        GameObject myPanelObject = Instantiate(panelPrefab, panelCanvasParentObject.transform);
        myPanelObject.transform.position = transform.position;

        GameObject myNodeNameObject = Instantiate(nodeNamePrefab, nodeNameCanvasObject.transform);
        myNodeNameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z);
        NodeNameReferences nodeNameReferencesScript = myNodeNameObject.GetComponent<NodeNameReferences>();
        if (nodeNameReferencesScript != null) { nodeNameReferencesScript.nodeNameText.text = gameObject.name; }

        myPanelReferenceScript = myPanelObject.GetComponent<NodePanelReferences>();
        if(myPanelReferenceScript == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        jumpButton = myPanelReferenceScript.jumpButton;
        goodsParent = myPanelReferenceScript.goodsParent;
        nodeNameText = myPanelReferenceScript.nodeNameText;

        if (jumpButton == null || nodeNameText == null || goodsParent == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        nodeClickScript.nodePanel = myPanelObject;
        nodeScript.jumpButton = jumpButton;
        nodeScript.goodsParent = goodsParent;

        nodeNameText.text = gameObject.name;

        jumpButton.onClick.AddListener(JumpButtonClick);

        float currentHeight = 0f;
        foreach (Defs.TradeGoods good in nodeParametersScript.avaliableTradeGoods)
        {
            // Make a new prefab for each good
            GameObject goodsPrefab = Instantiate<GameObject>(goodsUIParent, goodsParent.transform);

            // Move the prefab so it lists 
            goodsPrefab.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 80f; //TODO make this not static

            GoodUIReferences goodUIReferences = goodsPrefab.GetComponent<GoodUIReferences>();
            if (goodUIReferences == null)
            {
                Debug.LogError("Setup error on " + gameObject.name);
                this.enabled = false;
                return;
            }

            // Set the Good script up
            Good goodScript = Defs.Instance.AddGoodScript(good, goodsPrefab);
            if (goodScript != null) {
                goodScript.good = good;
                goodScript.activeNodeScript = nodeScript;
                goodScript.myUIReferences = goodUIReferences;
                goodScript.enabled = true;
            }

            Button buyButton = goodUIReferences.buyButton;
            Button sellButton = goodUIReferences.sellButton;

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

        foreach (Defs.EngineUpgrades engineUpgrade in nodeParametersScript.avalaibleEngineUpgrades)
        {
            GameObject upgradePrefab = Instantiate(upgradeUIParent, goodsParent.transform);

            // Move the prefab so it lists 
            upgradePrefab.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 80f; //TODO make this not static

            // Set the Good script up
            Upgrade upgradeScript = upgradePrefab.GetComponent<Upgrade>();
            if (upgradeScript != null)
            {
                upgradeScript.upgrade = engineUpgrade;
                upgradeScript.activeNodeScript = nodeScript;
                upgradeScript.enabled = true;
            }

            Button buyButton = upgradeScript.buyButton;

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


    private void JumpButtonClick()
    {
        if (nodeScript.getRingActive() && !TimeCounter.Instance.gameOver)
        {
            // Get the current active node and disable it
            ActiveNode currentActiveNode = ObjectManager.Instance.currentActiveNode;

            if (currentActiveNode != null && currentActiveNode.getActive())
            {
                NodeConnections currentActiveNodeConnections = currentActiveNode.GetComponent<NodeConnections>();
                if (currentActiveNodeConnections != null && currentActiveNodeConnections.connectedNodes.ContainsKey(myConnections))
                {
                    Connection connectionScriptToMe = currentActiveNodeConnections.connectedNodes[myConnections];

                    // If this is a Blue jump, work out if the player can afford the jump and deduct it from their account
                    if (connectionScriptToMe.connectionType == Defs.ConnectionTypes.CONNECTIONTYPE_BLUE)
                    {
                        if (PlayerMoney.Instance.GetPlayerCash() < connectionScriptToMe.costToJump)
                        {
                            Debug.Log("Not Enough Cash to make Jump");
                            return;
                        }
                        PlayerMoney.Instance.IncrementPlayerCash(-connectionScriptToMe.costToJump);
                    }

                    // If this is a Red jump, there's a chance an event will happen
                    if (connectionScriptToMe.connectionType == Defs.ConnectionTypes.CONNECTIONTYPE_RED)
                    {
                        float randomChance = UnityEngine.Random.value;
                        // Engine type has a factor in piracy chance
                        if (randomChance < Defs.Instance.engineUpgradesPiracyChance[PlayerCargo.Instance.GetCurrentEngine()])
                        {
                            // 50/50 chance of either pirate event
                            if (UnityEngine.Random.value > 0.5f) { EventsManager.Instance.StartEvent(Defs.Events.EVENT_PIRATE_ONE); }
                            else { EventsManager.Instance.StartEvent(Defs.Events.EVENT_PIRATE_TWO); }
                            
                        }
                    }

                    currentActiveNode.setActive(false);

                    // Work out the time to pass based off the distance and the engine speed
                    int timeToPass = connectionScriptToMe.GetCurrentJumpCost();
                    TimeCounter.Instance.passTime(timeToPass);
                } else
                {
                    Debug.LogError("Error getting Current Active Node Connections on " + transform.parent.name);
                }
            }

            //Enable myself
            nodeScript.setActive(true);

            // Update all Goods UI
            foreach (var good in ObjectManager.Instance.globalGoodList)
            {
                good.updateUI();
            }

            // Update all Upgrades UI
            foreach (var upgrade in ObjectManager.Instance.globalUpgradeList)
            {
                upgrade.updateUI();
            }
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
