using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TextManager : MonoBehaviour
{
    private LoseScript hpVal;
    [SerializeField] private TextMeshProUGUI hpText; // Reference to the HP text UI
    private LaunchCannonball powerVal;

    [SerializeField] private TextMeshProUGUI powerText; // Reference to the HP text UI
    
    [SerializeField] private WaveController waveVal;

    [SerializeField] private TextMeshProUGUI waveText; 

    [SerializeField] private TextMeshProUGUI infoText;

    // Start is called before the first frame update
    void Start()
    {
        hpVal = GameObject.Find("CannonHitbox").GetComponent<LoseScript>();
        hpText.text = "HP: " + hpVal.hp; // Update the UI text to reflect the current HP

        powerVal = GameObject.Find("cannon (angle and force Changer)").GetComponent<LaunchCannonball>();
        powerText.text = "Power: "+powerVal._power;

        waveText.text = "Wave: "+waveVal.wave;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfoText();
    }

    public void UpdateHPText()
    {
        hpText.text = "HP: " + hpVal.hp; // Update the UI text to reflect the current HP
    }

    public void UpdatePowerText()
    {
        powerText.text = "Power: " + powerVal._power; // Update the UI text to reflect the current HP
    }

    public void UpdateWaveText()
{
    if (waveVal == null)
    {
        Debug.LogError("waveVal is not assigned!");
        return;
    }
    waveText.text = "Wave: " + waveVal.wave;
    }

    public void UpdateInfoText(){
        if(waveVal.wave == 1){
            infoText.text = "The Grounded.\nFloats towards you, gets knocked back when you hit them.\nKeep shooting!";
        }else if(waveVal.wave == 2){
            infoText.text = "The Caged.\nOnly move when you charge up your shots.\nPrioritize targets!";
        }else if(waveVal.wave == 3){
            infoText.text = "The Pillar.\nMoves back and forth alongside your cannon's angle and shoots red cannonballs.\nBe strategic!";
        }
    }
}
