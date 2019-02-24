using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnections : MonoBehaviour
{
    public List<NodeConnections> nodesToConnect;
    public List<int> timeToTravel;

    public Dictionary<NodeConnections, int> connectedNodes;
    // Start is called before the first frame update
    void Start()
    {
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
