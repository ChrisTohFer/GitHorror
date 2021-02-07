using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Image fadePanel;
    public float fadeTime = 3f;
    public AudioSource music;
    public AudioSource end;
    private float storedVolume;

    private void Start()
    {
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
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadePanel.color = new Color(0, 0, 0, ((i) / fadeTime)*3);
            music.volume = (storedVolume - ((i/fadeTime)*storedVolume)*3);
            yield return null;
        }
        SceneManager.LoadScene("GameplayLevel", LoadSceneMode.Single);
    }
}
