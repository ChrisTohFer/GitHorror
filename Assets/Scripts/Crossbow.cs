using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public Camera playerCamera, weaponCamera;
    public float defaultFov, aimingFov;
    public float MinimumFireCharge = .75f;

    private bool bowReset = false;
    Vector3 m_anchorStartPos;
    bool m_buttonHeld = false;
    bool m_buttonReleased = false;
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
            bowReset = false;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_buttonHeld = false;
            m_buttonReleased = true;
            AudioSource.Stop();

            //ResetFov
            weaponCamera.DOKill();
            playerCamera.DOKill();
            weaponCamera.DOFieldOfView(defaultFov, DrawTime/2);
            playerCamera.DOFieldOfView(defaultFov, DrawTime/2);
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_buttonHeld = false;
            AudioSource.Stop();
            m_pull = 0f;

            //ResetFov
            weaponCamera.DOKill();
            playerCamera.DOKill();
            weaponCamera.DOFieldOfView(defaultFov, DrawTime/2);
            playerCamera.DOFieldOfView(defaultFov, DrawTime/2);
            bowReset = true;
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (!bowReset)
            {
                //Change FOV
                //playerCamera.DOKill();
                //weaponCamera.DOKill();
                weaponCamera.DOFieldOfView(aimingFov, DrawTime);
                playerCamera.DOFieldOfView(aimingFov, DrawTime);
            }
            else
            {
                //ResetFov
                playerCamera.DOKill();
                weaponCamera.DOKill();
                playerCamera.DOFieldOfView(defaultFov, DrawTime/2);
                weaponCamera.DOFieldOfView(defaultFov, DrawTime/2);
            }
        }

    }

    private void FixedUpdate()
    {
        //Fire
        if (m_pull >= MinimumFireCharge && m_buttonReleased == true && PlayerStats.GetStat("bolts") > 0f)
        {
            Vector3 target;
            RaycastHit hit;
            if (
                Physics.Raycast(
                    Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)),
                    out hit, 100000f,
                    ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Field") + LayerMask.GetMask("Door"))
                    )
                )
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

        m_buttonReleased = false;
    }

    void StringPosition(float pull)
    {
        StringAnchor.localPosition = Vector3.Lerp(m_anchorStartPos, StringFinalPosition, pull);
    }
    void Fire(Vector3 target)
    {
        var go = Instantiate(BoltPrefab, StringAnchor.position, StringAnchor.rotation);
        var rb = go.GetComponent<Rigidbody>();
        var bolt = go.GetComponent<Bolt>();
        go.transform.LookAt(target);
        rb.velocity = (target - go.transform.position).normalized * BoltSpeed;
        bolt.Damage *= m_pull;
    }
}
