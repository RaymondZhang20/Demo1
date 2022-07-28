using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public TMPro.TextMeshProUGUI energyText;
    public static int currentEnergy;
    public static int maxEnergy;

    private Image energyBar;
    // Start is called before the first frame update
    void Start()
    {
        energyBar = GetComponent<Image>();
        currentEnergy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        energyBar.fillAmount = (float)currentEnergy/(float)maxEnergy;
        energyText.text = currentEnergy.ToString() + "/" + maxEnergy.ToString();
    }
}
