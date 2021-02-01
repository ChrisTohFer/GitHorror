using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        Singleton = this;
        PlayerStats.Reset();
        PlayerStats.SetStat("bolts", StartingBolts);
        PlayerStats.SetStat("medkits", StartingMedKits);
    }

    public void TakeDamage(float damage)
    {
        PlayerStats.ChangeStat("health", -damage);
        if(PlayerStats.GetStat("health") == 0f)
        {
            //lose
            PlayerInteraction.enabled = false;
            Crossbow.enabled = false;
            FP.enabled = false;
        }
        else
        {
            StartCoroutine(FP.CameraShake(CamShakeTime, CamShakeIntensity));
        }
    }
}
