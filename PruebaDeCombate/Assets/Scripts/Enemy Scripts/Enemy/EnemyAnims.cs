using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnims : MonoBehaviour
{
    public void AnimAtaque(Animator anim) => anim.SetBool("Ataque", true);

    public void AnimBloqueo(Animator anim) => anim.SetBool("Bloqueo", true);

    private float VelocidadAnimCaminata;
    public void AnimCaminata(Animator anim, float distanciaPlayer, float DistanciaEntradaGuardia)
    {
        if (distanciaPlayer < DistanciaEntradaGuardia) VelocidadAnimCaminata = 1f;
        else VelocidadAnimCaminata = 2f;

        anim.speed = VelocidadAnimCaminata;
        anim.SetBool("Caminata", true);
    }
    

    public void AnimEstatico(Animator anim)
    {
        anim.SetBool("Caminata", false);
        anim.SetBool("Ataque", false);
        anim.SetBool("Bloqueo", false);
    }
}
