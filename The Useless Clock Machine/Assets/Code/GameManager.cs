using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using X;
using X.Common;
using X.Common.Know;

public class GameManager : MonoBehaviour
{
    public static GameManager _ = default;
    
    [Header("Settings UI")]
    [Space]
    [Header("Settings UI MENU")]
    [Space]
    [SerializeField] private GameObject obj_ui_menu;
    [Header("Settings UI HUD")]
    [Space]
    [SerializeField] private GameObject obj_ui_hud;
    [Header("Settings UI END")]
    [Space]
    [SerializeField] private GameObject obj_ui_end;

    [Header("Settings Clock")]
    [SerializeField] private Game_Clock pref_game_clock = default;// TODO
    [SerializeField] private Transform tr_container_game_clock = default;// TODO
    public int QTY_GAME_BUTTONS = 12;
    public int QTY_GAME_IA = 1;
    private Game_Clock currentCLock = default;

    private void Awake()
    {
        this.Singleton(ref _);
        MENU();
    }



    [ContextMenu("Play Game!")]
    public void PlayGame()
    {
        //limpiamos los clocks anteriores
        tr_container_game_clock.ClearChilds();
        Game_Clock _clock = Instantiate(pref_game_clock, Vector3.zero, Quaternion.identity, tr_container_game_clock);
        currentCLock = _clock;
        _clock.Generate_Game_Buttons(QTY_GAME_BUTTONS);
        _clock.Generate_Game_IA(QTY_GAME_IA);
        _clock.Play();

        HUD();
    }

    [ContextMenu("GoToMenu")]
    public void GoToMenu()
    {
        currentCLock = null;
        tr_container_game_clock.ClearChilds();
        MENU();
    }

    [ContextMenu("GoToEnd")]
    public void GoToEnd()
    {
        tr_container_game_clock.ClearChilds();
        END();
        //INIT END
        // CODE HERE
    }


    [ContextMenu("- HUD")]
    public void HUD()
    {
        obj_ui_menu.SetActive(false);
        obj_ui_hud.SetActive(true);
        obj_ui_end.SetActive(false);
    }

    [ContextMenu("- MENU")]
    public void MENU()
    {
        obj_ui_menu.SetActive(true);
        obj_ui_hud.SetActive(false);
        obj_ui_end.SetActive(false);
    }

    [ContextMenu("- END")]
    public void END()
    {
        obj_ui_menu.SetActive(false);
        obj_ui_hud.SetActive(false);
        obj_ui_end.SetActive(true);
    }
}
