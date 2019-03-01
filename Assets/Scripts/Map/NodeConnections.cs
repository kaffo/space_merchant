﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnections : MonoBehaviour
{
    public List<NodeConnections> nodesToConnect;
    public List<int> timeToTravel;

    public GameObject connectionParent;

    public GameObject connectionPrefab;

    public Dictionary<NodeConnections, int> connectedNodes;
    // Start is called before the first frame update
    void Start()
    {
        if (connectionParent == null || connectionPrefab == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }

        connectedNodes = new Dictionary<NodeConnections, int>();
        if ( nodesToConnect.Count != timeToTravel.Count)
        {
            Debug.LogError(gameObject.name + " has incorrectly setup data!");
            this.enabled = false;
            return;
        }

        for (int i = 0; i < nodesToConnect.Count; i++)
        {
            connectedNodes.Add(nodesToConnect[i], timeToTravel[i]);
            GameObject currentConnection = Instantiate(connectionPrefab, connectionParent.transform);
            Connection currentConnectionScript = currentConnection.GetComponent<Connection>();

            currentConnectionScript.nodeToConnect = nodesToConnect[i];
            currentConnectionScript.cost = timeToTravel[i];
            currentConnectionScript.enabled = true;
        }
    }
}
