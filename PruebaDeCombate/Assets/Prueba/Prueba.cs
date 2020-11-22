using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    public Animator Anim;

    void Start()
    {
        Anim = GetComponent<Animator>();
    }


    private bool puedeCambiar = true;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && puedeCambiar)
        {
            puedeCambiar = false;
            Anim.SetBool("CambioDeReino", true);
        }
    }

    private bool ReinoMidgardActivo = true;

    void CambiaReino()
    {
        puedeCambiar = true;
        Anim.SetBool("CambioDeReino", false);

        if (ReinoMidgardActivo)
        {
            ReinoMidgardActivo = false;
            ReinoHell();
        }
        else
        {
            ReinoMidgardActivo = true;
            ReinoMidgard();
        }

    }

    public GameObject T_ReinoMidgard;

    void ReinoMidgard()
    {
        T_ReinoHell.transform.localPosition = new Vector2(20f, 20f);
        T_ReinoMidgard.transform.localPosition = new Vector2(0f, 0f);

    }
    public GameObject T_ReinoHell;
    void ReinoHell()
    {
        T_ReinoMidgard.transform.localPosition = new Vector2(20f, 20f);
        T_ReinoHell.transform.localPosition = new Vector2(0f, 0f);
    }
}
