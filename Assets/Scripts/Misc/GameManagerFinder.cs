using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFinder : MonoBehaviour
{
    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager == null)
        {
            Debug.LogError("Couldn't find GameManager on " + gameObject.name);
        }
    }
}
