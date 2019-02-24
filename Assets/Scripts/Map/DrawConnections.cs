using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawConnections : MonoBehaviour
{
    public Material lineWhite;
    private NodeConnections myNodeConnections;
    // Start is called before the first frame update
    void Start()
    {
        myNodeConnections = GetComponent<NodeConnections>(); 
        if (myNodeConnections == null)
        {
            return;
        }

        foreach (var node in myNodeConnections.connectedNodes.Keys)
        {
            GameObject child = new GameObject(node.gameObject.name + " Connection");
            child.transform.SetParent(transform);
            LineRenderer lrender = child.AddComponent<LineRenderer>();
            Vector3[] positionList = { gameObject.transform.position, node.gameObject.transform.position };
            lrender.SetPositions(positionList);
            lrender.startWidth = lrender.endWidth = 0.2f;
            if (lineWhite != null)
            {
                lrender.material = lineWhite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
