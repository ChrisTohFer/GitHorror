using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public Transform StringAnchor;
    public Transform Crank;
    public GameObject SlottedBolt;
    public Vector3 StringFinalPosition;
    public float DrawTime = 1f;
    public float FireTime = .1f;
    public float CrankSpinSpeed = 60f;
    public GameObject BoltPrefab;
    public float BoltSpeed;
    public float ChargingWalkSpeedModifier = .5f;
    public FirstPersonAIO FP;
    public AudioSource AudioSource;

    Vector3 m_anchorStartPos;
    bool m_buttonHeld = false;
    float m_pull = 0f;
    float m_normalWalkSpeed;
    float m_normalSprintSpeed;

    void Start()
    {
        m_normalWalkSpeed = FP.walkSpeed;
        m_normalSprintSpeed = FP.sprintSpeed;
        m_anchorStartPos = StringAnchor.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_buttonHeld = true;
            var clip = AudioManager.GetAudioClip(AudioManager.AudioClips.CrossbowWindup);
            AudioSource.Stop();
            AudioSource.volume = clip.volume;
            AudioSource.clip = clip.clip;
            AudioSource.Play();
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_buttonHeld = false;
            AudioSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        //Fire
        if (m_pull == 1f && m_buttonHeld == false && PlayerStats.GetStat("bolts") > 0f)
        {
            Vector3 target;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)), out hit, 100000f, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Field"))))
            {
                target = hit.point;
            }
            else
            {
                target = StringAnchor.position + (Camera.main.transform.rotation * Vector3.forward);
            }
   
            Fire(target);
            PlayerStats.ChangeStat("bolts", -1f);
            AudioManager.PlayOnPlayer(AudioManager.AudioClips.CrossbowFire);
        }

        //Bolt visibility and walk speed
        if(m_buttonHeld && PlayerStats.GetStat("bolts") > 0f)
        {
            SlottedBolt.SetActive(true);
            FP.walkSpeed = m_normalWalkSpeed * ChargingWalkSpeedModifier;
            FP.sprintSpeed = m_normalSprintSpeed * ChargingWalkSpeedModifier;
        }
        else
        {
            SlottedBolt.SetActive(false);
            FP.walkSpeed = m_normalWalkSpeed;
            FP.sprintSpeed = m_normalSprintSpeed;
        }

        //Update pull
        m_pull += Time.fixedDeltaTime * 1f / (m_buttonHeld ? DrawTime : FireTime) * (m_buttonHeld ? 1f : -1f);
        m_pull = Mathf.Clamp(m_pull, 0f, 1f);
        StringPosition(m_pull);
        
        //Spin the crank
        if (m_buttonHeld && m_pull < 1f)
            Crank.eulerAngles = new Vector3(Crank.eulerAngles.x, Crank.eulerAngles.y, Crank.eulerAngles.z + Time.fixedDeltaTime * CrankSpinSpeed);
    }

    void StringPosition(float pull)
    {
        StringAnchor.localPosition = Vector3.Lerp(m_anchorStartPos, StringFinalPosition, pull);
    }
    void Fire(Vector3 target)
    {
        var go = Instantiate(BoltPrefab, StringAnchor.position, StringAnchor.rotation);
        var rb = go.GetComponent<Rigidbody>();
        go.transform.LookAt(target);
        rb.velocity = (target - go.transform.position).normalized * BoltSpeed;
    }
}
