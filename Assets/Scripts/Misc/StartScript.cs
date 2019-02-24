using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    public ActiveNode startNode;
    // Start is called before the first frame update
    void Start()
    {
        if (startNode) { startNode.setActive(true); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
