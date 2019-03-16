using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStart : MonoBehaviour
{
    [Header("References")]
    public ActiveNode startNode;
    public int initialTimeToPass = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (startNode)
        {
            startNode.setActive(true);
            ObjectManager.Instance.currentActiveNode = startNode;
        }

        if (initialTimeToPass > 0)
        {
            StartCoroutine(WaitToStepEconomy());
        }
    }

    IEnumerator WaitToStepEconomy()
    {
        yield return new WaitForSeconds(1);
        TimeCounter.Instance.StepEconomy(initialTimeToPass);
        yield return null;
    }
}
