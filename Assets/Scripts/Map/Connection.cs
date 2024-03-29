﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    [Header("Connection Setup")]
    public Defs.ConnectionTypes connectionType = Defs.ConnectionTypes.CONNECTIONTYPE_DEFAULT;
    public NodeConnections nodeToConnect;
    public int costToJump = 0;

    [Header("Internal References")]
    public Transform connectionModel;
    public Text distanceText;

    [Header("Materials")]
    public Material defaultConnectionMaterial;
    public Material greenConnectionMaterial;
    public Material redConnectionMaterial;
    public Material blueConnectionMaterial;

    private int initialTravelTime = 10;
    private int currentTravelTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (nodeToConnect == null || connectionModel == null || distanceText == null
            || defaultConnectionMaterial == null || greenConnectionMaterial == null || redConnectionMaterial == null || blueConnectionMaterial == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }

        // Set Distance Panel Size
        if (connectionType == Defs.ConnectionTypes.CONNECTIONTYPE_BLUE || connectionType == Defs.ConnectionTypes.CONNECTIONTYPE_RED)
        {
            RectTransform panelTrasform = distanceText.transform.parent.GetComponent<RectTransform>();
            
            Vector2 sizeDelta = panelTrasform.sizeDelta;
            panelTrasform.sizeDelta = new Vector2(sizeDelta.x * 2, sizeDelta.y);
            distanceText.GetComponent<RectTransform>().sizeDelta = new Vector2(panelTrasform.sizeDelta.y, panelTrasform.sizeDelta.x);
        }

        // Make sure the UI has the right text
        UpdateUI(PlayerCargo.Instance.GetCurrentEngine());

        Vector3 myStartPos = transform.position;
        Vector3 otherNodePos = nodeToConnect.transform.position;
        // Middle position to move the middle of the line to
        Vector3 middlePos = new Vector3((myStartPos.x + otherNodePos.x) / 2, (myStartPos.y + otherNodePos.y) / 2, 0f);
        Vector3 middlePosPerpCalc = new Vector3(middlePos.x, middlePos.y, 5f);

        Vector3 differenceVector = otherNodePos - myStartPos;
        float connectionLength = differenceVector.magnitude / 2;
        float angleToOtherNode = Vector3.Angle(Vector3.up, differenceVector);
        // Angle between calculates left or right depending on which way the vector is facing
        if (differenceVector.x > 0) { angleToOtherNode = -angleToOtherNode; }
        Quaternion quaternionToOtherNode = Quaternion.Euler(new Vector3(0f, 0f, angleToOtherNode));

        // Work out perpendicular vector to nudge connection over a little
        Vector3 myPerpVector = Vector3.Cross(myStartPos - middlePosPerpCalc, otherNodePos - middlePosPerpCalc).normalized * 0.4f;

        transform.position = middlePos + myPerpVector;
        transform.rotation = quaternionToOtherNode;

        // Flip the distance text back so it's always horizontal
        distanceText.transform.parent.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -angleToOtherNode + 90));

        connectionModel.localScale = new Vector3(connectionModel.localScale.x, connectionLength, connectionModel.localScale.x);

        switch (connectionType)
        {
            case Defs.ConnectionTypes.CONNECTIONTYPE_GREEN:
                connectionModel.GetComponent<Renderer>().material = greenConnectionMaterial;
                break;
            case Defs.ConnectionTypes.CONNECTIONTYPE_RED:
                connectionModel.GetComponent<Renderer>().material = redConnectionMaterial;
                break;
            case Defs.ConnectionTypes.CONNECTIONTYPE_BLUE:
                connectionModel.GetComponent<Renderer>().material = blueConnectionMaterial;
                break;
        }
    }

    public void UpdateUI(Defs.EngineUpgrades engineUpgrade = Defs.EngineUpgrades.ENGINEUPRADE_DEFAULT)
    {
        float modifer = Defs.Instance.engineUpgradesSpeeds[engineUpgrade];
        currentTravelTime = (int)((float)initialTravelTime * modifer);
        switch (connectionType)
        {
            case Defs.ConnectionTypes.CONNECTIONTYPE_BLUE:
                distanceText.text = currentTravelTime.ToString() + "\n$" + costToJump.ToString();
                break;
            case Defs.ConnectionTypes.CONNECTIONTYPE_RED:
                string piracyChance = (Defs.Instance.engineUpgradesPiracyChance[engineUpgrade] * 100).ToString();
                distanceText.text = currentTravelTime.ToString() + "\n!" + piracyChance + "%!";
                break;
            default:
                distanceText.text = currentTravelTime.ToString();
                break;
        }
    }

    public IEnumerator EngineUpgrade(Defs.EngineUpgrades engineUpgrade)
    {
        UpdateUI(engineUpgrade);
        yield return null;
    }

    public void SetJumpCost(int costToSet)
    {
        initialTravelTime = costToSet;
        currentTravelTime = initialTravelTime;
        UpdateUI(PlayerCargo.Instance.GetCurrentEngine());
    }

    public int GetCurrentJumpCost()
    {
        return currentTravelTime;
    }
}
