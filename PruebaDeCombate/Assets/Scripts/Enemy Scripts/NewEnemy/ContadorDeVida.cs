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

    public ParticleSystem Sangre;
    void ContadorVida()
    {
        Vida--;
        Sangre.Play(true);
        if (Vida <= 0)
        {
            GetComponent<Enemigo>().AccionesDeMuerte();
            AnimMuerte();
        }
    }




}
