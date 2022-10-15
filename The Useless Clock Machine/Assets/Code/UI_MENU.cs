using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using X.Common;
using X.Common.Know;
using TMPro;
public class UI_MENU : MonoBehaviour
{

    [SerializeField] private TMP_Text tmp_text_title = default;
    [SerializeField] private Button btn_play = default;
    private float count_title = 0;
    private float timer = 10;
    private bool flagged_title_timer = false;

    private void Awake()
    {
        btn_play.gameObject.SetActive(false);
    }

    void Update()
    {
        AnimateTitle();
    }

    void AnimateTitle()
    {
        if (flagged_title_timer) return;

        flagged_title_timer = timer.TimerIn(ref count_title);

        if (flagged_title_timer) btn_play.gameObject.SetActive(true);

        var value = flagged_title_timer ? 0 : (int)(timer - count_title);
        var colorrMark = flagged_title_timer ? "#FF0000AA" : "#FFFFFF00";
        
        tmp_text_title.text =
            $"<b>The</b>\n" +
            $"<b><s>Useless</s></b>\n" +
            $"<i>Clock</b><sup><mark={colorrMark}>{ value }</mark></sup></i>\n" +
            $"<b>Machine</b>";
    }


    [ContextMenu("OpenRefGame")]
    public void OpenReferenceGame()
    {
        Application.OpenURL("https://en.wikipedia.org/wiki/Useless_machine");
        Application.OpenURL("https://www.youtube.com/watch?v=MCRXVcXYtfk");
    }
}
/*
 <b>The</b>
<b><s>Useless</s></b>
<i>Clock</b><sup>10</sup></i>
<b>Machine</b>
 */