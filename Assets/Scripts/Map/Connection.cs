﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    [Header("Connection Setup")]
    public int cost = 10;
    public Defs.ConnectionTypes connectionType = Defs.ConnectionTypes.CONNECTIONTYPE_DEFAULT;
    public NodeConnections nodeToConnect;

    [Header("Internal References")]
    public Transform connectionModel;
    public Text distanceText;


    // Start is called before the first frame update
    void Start()
    {
        if (nodeToConnect == null || connectionModel == null || distanceText == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }

        distanceText.text = cost.ToString();

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

        connectionModel.localScale = new Vector3(connectionModel.localScale.x, connectionLength, connectionModel.localScale.x);
    }
}
