using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartTitleFadeIn : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Texture2D fallbackImage;
    [SerializeField] private GameObject BackGround;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject Btn;
    [SerializeField] private GameObject txt;

    private string _mainscenename = "MainScene";
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;

        // 비디오 출력 설정
        videoPlayer.targetTexture = renderTexture;
        BackGround.GetComponent<RawImage>().texture = renderTexture;
        videoPlayer.Play();
    }

    IEnumerator FadeIn()
    {
        txt.SetActive(true);
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

    void OnVideoFinished(VideoPlayer vp)
    {
        BackGround.GetComponent<RawImage>().texture = fallbackImage;
        StartCoroutine(FadeIn());
    }
    public void GameStart()
    {
        SceneManager.LoadScene(_mainscenename);
    }
}
