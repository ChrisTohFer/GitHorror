using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class VictoryScreenController : MonoBehaviour
{

    public Image TitleImage, ClearedImage;
    public Color TitleOutlineColor1, TitleOutlineColor2;
    private float time = 3f;
    public GameObject StartObject;
    private Text StartText;

    private void Awake()
    {
        StartText = StartObject.GetComponent<Text>();
        StartCoroutine(TurnFirstColour(time));
    }

    private IEnumerator TurnFirstColour(float changeTime)
    {
        TitleImage.DOColor(TitleOutlineColor1, changeTime);
        StartText.DOColor(TitleOutlineColor2, changeTime);
        ClearedImage.DOColor(TitleOutlineColor2, changeTime);
        yield return new WaitForSeconds(changeTime);
        StartCoroutine(TurnSecondColour(changeTime));
    }

    private IEnumerator TurnSecondColour(float changeTime)
    {
        TitleImage.DOColor(TitleOutlineColor2, changeTime);
        StartText.DOColor(TitleOutlineColor1, changeTime);
        ClearedImage.DOColor(TitleOutlineColor1, changeTime);
        yield return new WaitForSeconds(changeTime);
        StartCoroutine(TurnFirstColour(changeTime));
    }
}
