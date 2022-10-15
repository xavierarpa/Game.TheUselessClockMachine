using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using X;
using X.Common;
using X.Common.Know;
using TMPro;

public class Game_Clock : MonoBehaviour
{

    [Header("Settings Game_Buttons")]
    [SerializeField] private Game_Button pref_game_buttons = default;
    [SerializeField] private Transform tr_container_game_buttons = default;
    private const float RADIUS_GAME_BUTTONS = 1.24f;
    private List<Game_IA> list_game_ia = default;

    [Header("Settings IA")]
    [SerializeField] private Game_IA pref_ia = default;
    [SerializeField] private Transform tr_container_ia = default;
    private const float RADIUS_GAME_IA = 1.5f;
    private List<Game_Button> list_game_button = default;


    [Header("Settings Clock")]
    [SerializeField] Button btn_reset = default;
    [SerializeField] Button btn_end = default;
    [SerializeField] Image img_win = default;
    [Space]
    [SerializeField] Image img_circle_timer = default;
    [SerializeField] private float speed_rotation = default;
    private float target_rotation = 0;

    private float count = 0;
    private const float timer = 10; // Ludum Dare: ~Cada 10 segundos...~
    private bool timeRunning = false;
    private static readonly Color color_ok = Color.white;
    [SerializeField] private Color color_ko = default;
    //[SerializeField] private Color color_ko = new Color(4, 107, 255, 255);
    //private static readonly Color color_ko = Color.yellow / 1.1f;

    public TMPro.TMP_Text text = default;


    [Header("Stats")]
    private int wins_player = 0;
    private int wins_ia = 0;

    private void OnEnable() => Subscribe(true);
    private void OnDisable() => Subscribe(false);

    private void Awake()
    {
        btn_reset.gameObject.SetActive(false);
        btn_end.gameObject.SetActive(false);
    }
    private void Update()
    {
        CheckTimer();
        RotateButtons();
    }

    private void RotateButtons()
    {
        if (!timeRunning) return;

        //si ya están cerca
        if (Quaternion.Angle(tr_container_game_buttons.localRotation, Quaternion.Euler(0, 0, target_rotation)).Equals(0))
        {
            target_rotation = 360f.MinusMax();
        }
        else
        {
            tr_container_game_buttons.localRotation = Quaternion.RotateTowards(
                tr_container_game_buttons.localRotation,
                Quaternion.Euler(0,0,target_rotation),
                speed_rotation * Time.deltaTime
            );
        }
    }

    private void Subscribe(bool condition)
    {
        if (condition)
        {
            btn_reset.onClick.AddListener(Resett);
            btn_end.onClick.AddListener(End);
        }
        else
        {
            btn_reset.onClick.RemoveListener(Resett);
            btn_end.onClick.RemoveListener(End);
        }
    }


    public void Play()
    {
        count = 0;
        timeRunning = true;
        PlayStop_Game_IA(true);
        PlayStop_Game_Button(true);
    }
    private void Resett()
    {
        //Resetea 
        GameManager._.PlayGame();
    }
    private void End()
    {
        //Vamos al final del juego
        GameManager._.GoToEnd();
    }
    //modificable
    public void PlayStop_Game_IA(bool condition)
    {
        if (list_game_ia is null)
        {
            //No hay nada
        }
        else
        {
            for (int i = 0; i < list_game_ia.Count; i++)
            {
                list_game_ia[i].isRunning = condition;
            }
        }
    }

    public void PlayStop_Game_Button(bool condition)
    {
        if (list_game_button is null)
        {
            //No hay nada
        }
        else
        {
            for (int i = 0; i < list_game_button.Count; i++)
            {
                list_game_button[i].modificable = condition;
            }
        }
    }

    private void CheckTimer(){
        if (!timeRunning) return;

        if (timer.TimerIn(ref count))
        {
            img_circle_timer.fillAmount = 1;
            img_circle_timer.color = color_ko;
            timeRunning = false;
            PlayStop_Game_IA(false);
            PlayStop_Game_Button(false);


            //Revisamos la partida
            // quíen tiene más puntos ? IA o player
            var list_game_button_isOk = list_game_button.Where(btn => btn.isOk).ToArray();
            var list_game_button_isKo = list_game_button.Where(btn => !btn.isOk).ToArray();
            // SI hay mayor o igual gnaa jugador
            if (list_game_button_isOk.Length >= list_game_button_isKo.Length)
            {
                wins_player++;
            }
            else
            {
                wins_ia++;
            }



            //Revisar si mostrar o nó el trofeo
            if (wins_player.Equals(wins_ia))
            {
                img_win.enabled = false;
            }
            else 
            {
                img_win.enabled = true;
                

                if (wins_player > wins_ia)
                {
                    img_win.color = Game_Button.color_ok;
                }
                else
                {
                    img_win.color = Game_Button.color_ko;
                }

            }


            text.text = $"{wins_player} - {wins_ia}";


            //si alguno llega a 3 puntos
            if (wins_player.Equals(3) || wins_ia.Equals(3))
            {
                EndGame();
            }
            else
            {
                Play();
            }
        }
        else
        {
            //Esperar..

            //Pinta el estado actual
            img_circle_timer.fillAmount = count / timer;
            img_circle_timer.color= Color.Lerp(color_ok, color_ko, count / timer);
        }
    }


    public void EndGame()
    {
        //Pausamos
        timeRunning = false;
        PlayStop_Game_IA(false);
        PlayStop_Game_Button(false);
        bool winPlayer = wins_player.Equals(3);
        btn_reset.gameObject.SetActive(!winPlayer);
        btn_end.gameObject.SetActive(winPlayer);
    }

    

    public void Generate_Game_IA(int qty)
    {
        //Limpiamos antiguos IA
        tr_container_ia.ClearChilds();
        list_game_ia = __Generate_Elements(qty, RADIUS_GAME_IA, pref_ia,tr_container_ia);
        foreach (var item in list_game_ia)
        {
            item.Initialize(ref list_game_button);
        }
    }

    public void Generate_Game_Buttons(int qty)
    {
        //Limpiamos antiguos Botones
        tr_container_game_buttons.ClearChilds();
        //Generamos
        list_game_button = __Generate_Elements(qty, RADIUS_GAME_BUTTONS, pref_game_buttons,tr_container_game_buttons);
    }


    private List<T> __Generate_Elements<T>(int numberOfObjects, float radius, T prefab, Transform parent)
        where T : Component
    {
        //float radius = 5f;
        List<T> list_t = new List<T>();
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            T t = Instantiate(prefab, pos, rot, parent);
            t.transform.localPosition = new Vector3(t.transform.localPosition.x, t.transform.localPosition.y, 0);
            t.transform.localRotation = Quaternion.identity;
            list_t.Add(t);
        }

        return list_t;
    }
}
