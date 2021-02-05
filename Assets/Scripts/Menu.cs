using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image fadePanel;
    public float fadeTimeIn = 3f;
    public float fadeTimeOut = 2f;
    private bool keyPressed = false;
    public AudioSource startSound;
    public GameObject menu;
    public GameObject music;

    public FirstPersonAIO FPS;
    public Crossbow Crossbow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && keyPressed == false)
        {
            keyPressed = true;
            startSound.Play();
            StartCoroutine(FadeImage());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && keyPressed == true)
        {
            Application.Quit();
        }
    }

    IEnumerator FadeImage()
    {
        for (float i = 0; i <= fadeTimeIn; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadePanel.color = new Color(0, 0, 0, (i/ fadeTimeIn));
            yield return null;
        }

        menu.SetActive(false);
        music.SetActive(true);

        for (float i = fadeTimeOut; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            fadePanel.color = new Color(0, 0, 0, (i/ fadeTimeOut));
            yield return null;
        }

        FPS.enabled = true;
        Crossbow.enabled = true;
    }


}
