﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

//TODO: Todavia falta agregar el empuje contra el player para que este se desplace hacia atras, falta agregar la caminata hacia atras y aunque sea una animacion simple y su skin. 
public class SimpleEnemy : Enemy
{
    private GameObject player;
    private Rigidbody2D rbEnemigo;
    private CircleCollider2D circleCollider;

    #region Tooltip
    [Tooltip("La velocidad default esta establecida, aun asi es posible multiplicar esta, los numeros permitidos son los mayores a," +
        " en caso de que la velocidad se aumente sera a modo de prueba, puesto que la animacion no cambiara con el aumento de velocidad")]
    #endregion
    public float MultiplicadorDeVelocidadDefault;
    #region Tooltip
    [Tooltip("El esquive puede ser reazalido con distintas velocidades, en caso de que esta fuera regular utilizar un 1")]
    #endregion
    public float MultiplicadorParaEsquive;

    #region Tooltip
    [Tooltip("Define cuanto tiempo se espera para realizar una u otra accion, como esquivar, bloquear o golpear")]
    #endregion
    public float MinTiempoEntreAcciones;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rbEnemigo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); //Puesto entre las animaciones! //Es obligatorio tenerlo! 
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public bool A;
    void Update()
    {
        RotacionSkinEnemigo(player.transform.position);
        Caminata(player.transform.position, MultiplicadorDeVelocidadDefault, anim, MultiplicadorParaEsquive);


        //BloqueoOcasional(player.GetComponent<AtaqueV2>().ActiveCombo, anim, player.transform.position);
        //ModoCombate(anim, MinTiempoEntreAcciones, MedidorDistancia(player.transform.position, transform.position));
    }
    private void FixedUpdate()
    {
        SaltoDePlataformas(player.transform.position, rbEnemigo, anim);
    }

    //Dibujo de distancias basicas del enemigo
    private void OnDrawGizmosSelected()
    {
        //Seguimiento a player//Exp
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, RangoVision);
        //Seguimiento a player//Guard
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DISTANCIA_ENTRADA_MODOGUARDIA);
        //Ataque//Acciones
        Gizmos.color = Color.yellow;

    }
}