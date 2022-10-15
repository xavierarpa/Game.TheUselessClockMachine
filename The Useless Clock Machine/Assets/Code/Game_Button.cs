using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using X.Common.Know;
    

public class Game_Button : MonoBehaviour
{
    public static readonly Color color_ok = Color.green;
    public static readonly Color color_ko = Color.red;
    public bool isOk = false;
    public bool modificable = false;

    [SerializeField] private SpriteRenderer render = default;
    //private float timer_scale = 5;
    //private float count_scale = 0;

    private float speed = 45;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (modificable)
        {
            //si es Game_IA
            if (collision.TryGetComponent(out Game_IA _ia))
            {
                render.color = color_ko;
                isOk = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (modificable)
        {
            render.color = color_ok;
            isOk = true;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!isOk) return;
        
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        //timer_scale.TimerIn(ref count_scale);

    }
}
