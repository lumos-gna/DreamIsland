using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    public TextMeshProUGUI text;
    [TextArea(3, 10)]
    public List<string> lines;
    public float typingSpeed = 0.05f;   // 한 글자당 시간
    public float lineInterval = 1f;     // 줄 간 간격

    public GameObject endButton;
    private void Start()
    {
        PlayEndingText();
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 정지
        #else
            Application.Quit(); // 빌드된 게임에서 종료
        #endif
    }

    private void PlayEndingText()
    {
        text.text = "";
        string cumulativeText = ""; // 누적 문자열을 따로 관리
        Sequence sequence = DOTween.Sequence();

        foreach (string line in lines)
        {
            string fullLine = line + "\n"; // 줄 끝에 줄바꿈 추가
            float duration = line.Length * typingSpeed;

            // 누적 텍스트에 새 줄 추가
            cumulativeText += fullLine;

            sequence.Append(
                text.DOText(cumulativeText, duration).SetEase(Ease.Linear)
            );

            sequence.AppendInterval(lineInterval);
        }
        
        sequence.OnComplete(() =>
        {
            endButton.SetActive(true);
        });
    }
    
    
    
}
