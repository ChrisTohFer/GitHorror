using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchoredString : MonoBehaviour
{
    public Transform Anchor1, Anchor2;

    void Update()
    {
        Vector3 difference = Anchor2.position - Anchor1.position;
        transform.position = Anchor1.position + difference / 2f;

        transform.LookAt(Anchor2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / transform.lossyScale.z * difference.magnitude / 2f);
    }
}
