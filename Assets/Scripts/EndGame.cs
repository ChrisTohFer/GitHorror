using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndGame : MonoBehaviour
{
    public Image fadePanel, DreadHouseLogo, ClearedLogo;
    public Text StartText, StartTextBG;
    public float fadeTime = 1f;
    public AudioSource music;
    public AudioSource end;
    private float storedVolume;
    public bool restartGame;

    public FirstPersonAIO FPS;
    public Crossbow Crossbow;

    private void Start()
    {
        restartGame = false;
        storedVolume = music.volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(GameEnd());
            end.Play();
        }
    }

    IEnumerator GameEnd()
    {
        FPS.playerCanMove = false;
        FPS.enableCameraMovement = false;
        Crossbow.enabled = false;

        
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadePanel.color = new Color(0.6117647f, 0.07843138f, 0.07843138f, ((i) / fadeTime)*3);
            music.volume = (storedVolume - ((i/fadeTime)*storedVolume)*3);
            yield return null;
        }
        DreadHouseLogo.rectTransform.DOAnchorPosX(-0, 1, false);
        ClearedLogo.rectTransform.DOAnchorPosX(0, 1.5f, false);
        StartText.rectTransform.DOMoveY(125, 2, false);
        StartTextBG.rectTransform.DOMoveY(125, 2, false);
        restartGame = true;
        //SceneManager.LoadScene("GameplayLevel", LoadSceneMode.Single);
    }
}
