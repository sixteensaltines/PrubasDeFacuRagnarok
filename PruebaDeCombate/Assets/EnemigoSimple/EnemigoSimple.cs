using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSimple : Enemigo
{
    public GameObject player;
    private Rigidbody2D rbEnemigo;

    void Start()
    {

        rbEnemigo = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (player != null)
        {
            if (!AccionEncontrada) //CUANDO UNA ACCION DEL MODO COMBATE ES ENCONTRADA, SE CANCELAN TODAS LAS ACCIONES! 
            {
                CaminataAPlayer(player.transform.position);
                DibujaRayos();
                BloqueoOcasional(player.GetComponent<RaunerCombate>().NumeroDeAtaque, player.transform.position);
            }
            ModoCombate(player.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (!AccionEncontrada)//CUANDO UNA ACCION DEL MODO COMBATE ES ENCONTRADA, SE CANCELAN TODAS LAS ACCIONES! 
            {
                SaltoDePlataformas(player.transform.position, rbEnemigo);
            }
        }
    }
}
