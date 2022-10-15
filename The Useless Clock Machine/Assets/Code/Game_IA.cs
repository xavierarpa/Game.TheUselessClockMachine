using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using X.Common.Know;

public class Game_IA : MonoBehaviour
{
    [HideInInspector] public bool isRunning = false;
    private bool rotateRight = default;
    private List<Game_Button> list_game_button = default;
    private Game_Button target = null;


    [Header("Settings Timer Movement")]
    [SerializeField] private Vector2 v2_range_timer_speed = default;
    [SerializeField] private Vector2 v2_range_speed = default;
    private float currentSpeed = 0;
    private float currentTimerSpeed = 0;
    private float count_TimerSpeed = 0;


    [Header("Settings movement Sprite effect")]
    [Space]
    [SerializeField] private Transform tr_movement_sprite = default;
    [SerializeField] private AnimationCurve animationCurve_movement_sprite = default;
    [SerializeField] private float timer_movememnt_sprite = 3;
    private float count_movement_sprite = 0;

    private void Update()
    {
        TryTarget();
        Rotate();
        //MovementSprite();
    }

    //private void MovementSprite(){
    //    if (!isRunning) return;

    //    timer_movememnt_sprite.TimerIn(ref count_movement_sprite);

    //    var val_percent =animationCurve_movement_sprite.Evaluate(count_movement_sprite / timer_movememnt_sprite);
        
    //    tr_movement_sprite.localPosition = Vector3.MoveTowards(
    //        Vector3.zero,
    //        new Vector3(0.1f, 0,0), //HARDCODED HACK
    //        val_percent
    //    );


    //}

    private void TryTarget()
    {
        
        if (target is null)
        {
            var arr_ok = list_game_button.Where(b => b.isOk).ToArray();
            if (arr_ok.Equals(0))
            {
                //Nada, no hay quien buscar
            }
            else
            {
                //Buscar el más cercano
                for (int i = 0; i < arr_ok.Length; i++)
                {
                    if (target)
                    {
                        //si la distancia con el nuevo es menor al antiguo target cambia
                        if (Vector3.Distance(transform.position, arr_ok[i].transform.position) < Vector3.Distance(transform.position, target.transform.position))
                        {
                            target = arr_ok[i];
                        }
                        else
                        {
                            //Seguimos buscando, este no es
                        }

                    }
                    else
                    {
                        //Por defecto
                        target = arr_ok[i];
                    }
                    
                }


                if (target)
                {
                    //A qué dirección se encuentra target?
                    if (transform.position.x < target.transform.position.x)
                    {
                        rotateRight = true;
                    }
                    else
                    {
                        rotateRight = false;
                    }
                }
            }
        }
        else
        {
            if (!target.isOk)
            {
                target = null;
            }
            else
            {
                //Nada, que siga al target
            }
        }
        
    }

    private void Rotate(){
        if (!isRunning) return;
        if (list_game_button is null) return;


        //Cuando pasa el tiempo del current timer speed
        if (currentTimerSpeed.TimerIn(ref count_TimerSpeed))
        {
            currentSpeed = v2_range_speed.Range();
            currentTimerSpeed = v2_range_timer_speed.Range();
        }


        if (rotateRight)
        {
            transform.RotateAround(transform.parent.position, Vector3.forward, -currentSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.parent.position, Vector3.forward, currentSpeed * Time.deltaTime);
        }
    }

    public void Initialize(ref List<Game_Button> list)
    {
        this.list_game_button = list;
        isRunning = true;
    }

}
