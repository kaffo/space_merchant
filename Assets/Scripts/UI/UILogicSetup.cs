using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogicSetup : MonoBehaviour
{
    public Button medicineButton;
    public Text medicineText;

    private int daysPerMedicine = 25;

    // Start is called before the first frame update
    void Start()
    {
        if (medicineButton == null || medicineText == null)
        {
            Debug.LogError("UILogic Setup Error");
            this.enabled = false;
            return;
        }

        medicineButton.onClick.AddListener(BuyMedicineButtonClicked);
    }

    private void BuyMedicineButtonClicked()
    {
        PlayerMoney playerMoney = PlayerMoney.Instance;
        if (!playerMoney.checkCash(playerMoney.getPlayerCash() - 500))
        {
            Debug.Log("Medicine Purchase Failed");
            return;
        } else
        {
            Debug.Log("Medicine Purchase Successful");
            TimeCounter.Instance.addTime(daysPerMedicine);
            playerMoney.incrementPlayerCash(-500);

            daysPerMedicine -= 1;
            medicineText.text = "Buy Medicine\n$500\n+" + daysPerMedicine + " days";
        }
    }
}
