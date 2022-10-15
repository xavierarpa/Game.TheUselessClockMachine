using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_END : MonoBehaviour
{
    [SerializeField] private Button btn_lve = default;


    private void Start()
    {
        btn_lve.onClick.AddListener(A);
    }

    public void A()
    {
        //Carga again el juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
