using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickToContinuePopup : MonoBehaviour
{
    public Text popupLetterText;

    public GameObject mapGameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (popupLetterText == null || mapGameObject == null)
        {
            Debug.LogError("ClickToContinue Setup Error");
            this.enabled = false;
            return;
        }
    }

    public void SetupLetter(string letterText)
    {
        if (letterText != null)
        {
            popupLetterText.text = letterText;
        }

        gameObject.SetActive(true);
        mapGameObject.SetActive(false);
        this.enabled = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            this.enabled = false;
            gameObject.SetActive(false);
            mapGameObject.SetActive(true);
        }
    }
}
