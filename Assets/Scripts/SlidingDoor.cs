using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 OpenOffset = new Vector3(0f, 3f, 0f);
    public float OpenTime = 1f;
    public string[] RequiredKeyItems;

    bool m_open = false;
    Vector3 ClosedPosition;
    bool m_coroutineRunning = false;

    private void Awake()
    {
        ClosedPosition = transform.position;
    }

    public void Open()
    {
        if (!m_coroutineRunning && !m_open)
        {
            m_coroutineRunning = true;
            m_open = true;
            StartCoroutine("COpenSlide");
        }
    }
    public void Close()
    {
        if (!m_coroutineRunning && m_open)
        {
            m_coroutineRunning = true;
            m_open = false;
            StartCoroutine("CCloseSlide");
        }
    }
    public void Toggle()
    {
        if (m_open)
            Close();
        else
            Open();
    }
    public void OpenWithKey()
    {
        if (PlayerStats.HasMultipleKeyItems(RequiredKeyItems))
            Open();
    }
    public void CloseWithKey()
    {
        if (PlayerStats.HasMultipleKeyItems(RequiredKeyItems))
            Close();
    }
    public void ToggleWithKey()
    {
        if (PlayerStats.HasMultipleKeyItems(RequiredKeyItems))
            Toggle();
    }

    //
    IEnumerator COpenSlide()
    {
        float time = Time.fixedDeltaTime;
        while(time < OpenTime)
        {
            transform.position = Vector3.Lerp(ClosedPosition, ClosedPosition + OpenOffset, time / OpenTime);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        m_coroutineRunning = false;
    }
    IEnumerator CCloseSlide()
    {
        float time = Time.fixedDeltaTime;
        while(time < OpenTime)
        {
            transform.position = Vector3.Lerp(ClosedPosition + OpenOffset, ClosedPosition, time / OpenTime);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        m_coroutineRunning = false;
    }
}
