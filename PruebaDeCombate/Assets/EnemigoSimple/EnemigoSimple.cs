using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSimple : Enemigo
{
    private GameObject player;
    private Rigidbody2D rbEnemigo;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rbEnemigo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        CaminataAPlayer(player.transform.position);
        DibujaRayos();
        BloqueoOcasional(player.GetComponent<RaunerCombate>().NumeroDeAtaque, player.transform.position);
    }

    private void FixedUpdate()
    {
        SaltoDePlataformas(player.transform.position, rbEnemigo);
    }
}
