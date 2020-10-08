using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnims : MonoBehaviour
{
    public void AnimAtaque(Animator anim) => anim.SetBool("Ataque", true);

    public void AnimBloqueo(Animator anim) => anim.SetBool("Bloqueo", true);

    public void AnimCaminata(Animator anim)
    {
        anim.SetBool("Caminata", true);
    }

    public void AnimSalto(Animator anim, bool ActivarODesactivar) => anim.SetBool("Salto", ActivarODesactivar);

    public void AnimCaida(Animator anim, bool ActivarODesactivar) => anim.SetBool("Caida", ActivarODesactivar);

    public void AnimCaminata(Animator anim, float SpeedAnim)
    {
        anim.speed = SpeedAnim;
        anim.SetBool("Caminata", true);
    }


    public void AnimEstatico(Animator anim)
    {
        anim.SetBool("Caminata", false);
        anim.SetBool("Ataque", false);
        anim.SetBool("Bloqueo", false);
    }
}
