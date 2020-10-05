using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

//TODO: Todavia falta agregar el empuje contra el player para que este se desplace hacia atras, falta agregar la caminata hacia atras y aunque sea una animacion simple y su skin. 
public class SimpleEnemy : Enemy
{
    private GameObject player;
    private Rigidbody2D rbEnemigo;
    private Animator anim;

    #region Tooltip
    [Tooltip("La velocidad default esta establecida, aun asi es posible multiplicar esta, los numeros permitidos son los mayores a, en caso de que la velocidad se aumente sera a modo de prueba, puesto que la animacion no cambiara con el aumento de velocidad")]
    #endregion
    public float MultiplicadorDeVelocidadDefault;
    #region Tooltip
    [Tooltip("Define cuanto tiempo se espera para realizar una u otra accion, como esquivar, bloquear o golpear")]
    #endregion
    public float MinTiempoEntreAcciones;
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        rbEnemigo = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        RotacionSkinEnemigo(player.transform.position);
        SeguimientoPlayer_Caminata(player.transform.position, MultiplicadorDeVelocidadDefault, anim);
        //ModoCombate(anim, MinTiempoEntreAcciones, MedidorDistancia(player.transform.position, transform.position));
    }
    private void FixedUpdate() => Salto(player.transform.position, rbEnemigo);

    //Dibujo de distancias basicas del enemigo
    private void OnDrawGizmosSelected()
    {
        //Seguimiento a player//Exp
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, RangoVision);
        //Seguimiento a player//Guard
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DISTANCIA_ENTRADA_MODOGUARDIA);
        //MAX Empuje-Ataque
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RANGOATAQUE);
        //MIN Empuje-Ataque
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RANGOATAQUE+0.3f);
    }
}
