using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Singleton;

    public PlayerInteraction PlayerInteraction;
    public Crossbow Crossbow;
    public FirstPersonAIO FP;
    public float CamShakeTime = 0.1f;
    public float CamShakeIntensity = 0.05f;
    public int StartingBolts = 6;
    public int StartingMedKits = 0;
    public float MedKitHeal = 50f;

    // Law Code (Start)
    public TextMeshProUGUI UI_Health;

    public GameObject UI_Kit1;
    public GameObject UI_Kit2;
    public GameObject UI_Kit3;
    public GameObject UI_Kit4;
    public GameObject UI_Kit5;

    public TextMeshProUGUI UI_Bolts;

    public GameObject UI_KeyA;
    public GameObject UI_Keya;
    public GameObject UI_KeyB;
    public GameObject UI_Keyb;
    // Law Code (End)

    private void Awake()
    {
        Singleton = this;
        PlayerStats.Reset();
        PlayerStats.SetStat("bolts", StartingBolts);
        PlayerStats.SetStat("medkits", StartingMedKits);
    }

    private void Update()
    {
        if(PlayerStats.GetStat("health") == 0f)
        {

            return;
        }

        if(Input.GetKeyDown(KeyCode.Q) && PlayerStats.GetStat("medkits") > 0f && PlayerStats.GetStat("health") < 100f)
        {
            //Use medkit
            PlayerStats.ChangeStat("medkits", -1);
            PlayerStats.ChangeStat("health", MedKitHeal);

            AudioManager.PlayOnPlayer(AudioManager.AudioClips.PlayerHeal);

        }

        // Law Code (Start)
        //Medkit UI
        if (PlayerStats.GetStat("medkits") <= 0)
        {
            UI_Kit1.SetActive(false);
            UI_Kit2.SetActive(false);
            UI_Kit3.SetActive(false);
            UI_Kit4.SetActive(false);
            UI_Kit5.SetActive(false);
        }

        if (PlayerStats.GetStat("medkits") == 1)
        {
            UI_Kit1.SetActive(true);
            UI_Kit2.SetActive(false);
            UI_Kit3.SetActive(false);
            UI_Kit4.SetActive(false);
            UI_Kit5.SetActive(false);
        }

        if (PlayerStats.GetStat("medkits") == 2)
        {
            UI_Kit1.SetActive(true);
            UI_Kit2.SetActive(true);
            UI_Kit3.SetActive(false);
            UI_Kit4.SetActive(false);
            UI_Kit5.SetActive(false);
        }

        if (PlayerStats.GetStat("medkits") == 3)
        {
            UI_Kit1.SetActive(true);
            UI_Kit2.SetActive(true);
            UI_Kit3.SetActive(true);
            UI_Kit4.SetActive(false);
            UI_Kit5.SetActive(false);
        }

        if (PlayerStats.GetStat("medkits") == 4)
        {
            UI_Kit1.SetActive(true);
            UI_Kit2.SetActive(true);
            UI_Kit3.SetActive(true);
            UI_Kit4.SetActive(true);
            UI_Kit5.SetActive(false);
        }

        if (PlayerStats.GetStat("medkits") == 5)
        {
            UI_Kit1.SetActive(true);
            UI_Kit2.SetActive(true);
            UI_Kit3.SetActive(true);
            UI_Kit4.SetActive(true);
            UI_Kit5.SetActive(true);
        }

        //Health UI
        if (PlayerStats.GetStat("health") <= 0f)
        {
            UI_Health.SetText("DEAD");
            UI_Health.color = new Color32(102, 0, 0, 255);
        }

        if (PlayerStats.GetStat("health") > 0f && PlayerStats.GetStat("health") <= 30f)
        {
            UI_Health.SetText("DANGER");
            UI_Health.color = new Color32(204, 0, 0, 255);
        }

        if (PlayerStats.GetStat("health") > 30f && PlayerStats.GetStat("health") <= 70f)
        {
            UI_Health.SetText("CAUTION");
            UI_Health.color = new Color32(204, 102, 0, 255);
        }

        if (PlayerStats.GetStat("health") > 70f && PlayerStats.GetStat("health") <= 100f)
        {
            UI_Health.SetText("FINE");
            UI_Health.color = new Color32(102, 204, 0, 255);
        }

        //Bolts UI
        UI_Bolts.SetText("BOLTS " + PlayerStats.GetStat("bolts"));

        //Keys UI
        if (PlayerStats.HasKeyItem("MajorA"))
        {
            UI_KeyA.SetActive(true);
        }

        if (PlayerStats.HasKeyItem("MinorA"))
        {
            UI_Keya.SetActive(true);
        }

        if (PlayerStats.HasKeyItem("MajorB"))
        {
            UI_KeyB.SetActive(true);
        }

        if (PlayerStats.HasKeyItem("MinorB"))
        {
            UI_Keyb.SetActive(true);
        }

        // Law Code (End)
    }

    public void TakeDamage(float damage)
    {
        PlayerStats.ChangeStat("health", -damage);
        AudioManager.PlayOnPlayer(AudioManager.AudioClips.PlayerTakeDamage);

        if (PlayerStats.GetStat("health") == 0f)
        {
            //lose
            PlayerInteraction.enabled = false;
            Crossbow.enabled = false;
            FP.enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            StartCoroutine(FP.CameraShake(CamShakeTime, CamShakeIntensity));
        }
    }
}
