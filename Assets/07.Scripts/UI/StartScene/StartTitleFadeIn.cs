using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTitleFadeIn : MonoBehaviour
{

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject Btn;
    [SerializeField] private GameObject txt;

    private string _mainscenename = "MainScene";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float fadetime = 0f;
        float endtime = 2.5f;
        while(fadetime <= endtime)
        {
            Color titlecolor = title.GetComponent<RawImage>().color;
            Color Btncolor = Btn.GetComponent<Image>().color;
            Color txtcolor = txt.GetComponent<TextMeshProUGUI>().color;
            titlecolor.a = Mathf.Lerp(0f, 1f, fadetime / endtime);
            Btncolor.a = Mathf.Lerp(0f, 1f, fadetime / endtime);
            txtcolor.a = Mathf.Lerp(0f, 1f, fadetime / endtime);
            title.GetComponent<RawImage>().color = titlecolor;
            Btn.GetComponent<Image>().color = Btncolor;
            txt.GetComponent<TextMeshProUGUI>().color = txtcolor;
            fadetime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void GameStart()
    {
        SceneManager.LoadScene(_mainscenename);
    }
}
