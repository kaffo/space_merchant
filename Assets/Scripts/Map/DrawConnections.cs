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
            child.transform.localPosition = Vector3.zero;

            LineRenderer lrender = child.AddComponent<LineRenderer>();
            Vector3 myPos = gameObject.transform.position;
            Vector3 otherNodePos = node.gameObject.transform.position;
            Vector3 middlePos = new Vector3((myPos.x + otherNodePos.x)/2, (myPos.y + otherNodePos.y) / 2, 5f);

            Vector3 myPerpVector = Vector3.Cross(middlePos - myPos, otherNodePos - myPos).normalized * 0.4f;
            Vector3 otherPerpVector = Vector3.Cross(myPos - otherNodePos, middlePos - otherNodePos).normalized * 0.4f;

            myPos = myPos + myPerpVector;
            otherNodePos = otherNodePos + otherPerpVector;

            Vector3[] positionList = { myPos, otherNodePos };
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
