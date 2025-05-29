
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DG.Tweening
{
    public static class DOTweenTextMeshProExtensions
    {
        public static Tweener DOText(this TextMeshProUGUI target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        {
            return DOTween.To(() => target.text, x => target.text = x, endValue, duration)
                .SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
        }
    }
}