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

    Vector3 m_anchorStartPos;
    bool m_buttonHeld = false;
    float m_pull = 0f;

    void Start()
    {
        m_anchorStartPos = StringAnchor.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            m_buttonHeld = true;
        else if(Input.GetKeyUp(KeyCode.Mouse0))
            m_buttonHeld = false;
    }

    private void FixedUpdate()
    {
        //Fire
        if (m_pull == 1f && m_buttonHeld == false && PlayerStats.GetStat("bolts") > 0f)
        {
            Fire();
            PlayerStats.ChangeStat("bolts", -1f);
        }

        //Bolt visibility
        SlottedBolt.SetActive(m_buttonHeld && PlayerStats.GetStat("bolts") > 0f);

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
    void Fire()
    {
        var gameObject = Instantiate(BoltPrefab, StringAnchor.position, StringAnchor.rotation);
        var rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = (Camera.main.transform.rotation * Vector3.forward) * BoltSpeed;
    }
}