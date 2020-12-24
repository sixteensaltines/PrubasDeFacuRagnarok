using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContadorDeVida : AnimacionesEnemigos
{
    public int Vida;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void LlegaDanio()
    {
        if (gameObject.layer != 15) //layer EnemigoBloqueando
        {
            ContadorVida();
        }
    }

    void ContadorVida()
    {
        Vida--;
        if (Vida <= 0)
        {
            GetComponent<Enemigo>().AccionesDeMuerte();
            AnimMuerte();
        }
    }




}
