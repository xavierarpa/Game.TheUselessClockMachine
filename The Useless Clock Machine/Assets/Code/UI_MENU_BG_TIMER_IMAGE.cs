using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using X.Common.Know;
using X.Common;

public class UI_MENU_BG_TIMER_IMAGE : MonoBehaviour
{

    private Color endColor = default;
    private Image img = default;
    [SerializeField] private float timer = default;
    [SerializeField] private Color init_color = default;
    [SerializeField] private AnimationCurve animationCurve_scale = default;
    [SerializeField]

    private float count = 0;
    private bool flagIsDone = false;

    private void Awake()
    {
        this.Component(out img);
        //
        endColor = img.color;
        //
        img.fillAmount = 0;
        img.color = init_color;
    }

    void Update()
    {
        ChangeStatus();
    }

    private void ChangeStatus()
    {
        if (flagIsDone) return;

        if (timer.TimerIn(ref count))
        {
            flagIsDone = true;
            img.fillAmount = 1;
            img.color = endColor;
        }
        else
        {
            img.fillAmount = count / timer;
            img.color = Color.Lerp(init_color,endColor, img.fillAmount);
            // ~Feel
            transform.localScale = Vector3.one * Mathf.LerpUnclamped(0, 1, animationCurve_scale.Evaluate(count / timer));
        }

    }
}
