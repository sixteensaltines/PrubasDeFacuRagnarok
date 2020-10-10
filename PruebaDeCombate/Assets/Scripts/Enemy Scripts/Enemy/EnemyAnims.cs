using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnims : MonoBehaviour
{
    public Animator anim;

    public void AnimAtaque() => anim.SetBool("Ataque", true);

    public void AnimBloqueo(bool ActivarODesactivar) => anim.SetBool("Bloqueo", ActivarODesactivar);

    public void AnimCaminata(bool ActivarODesactivar)
    {
        anim.SetBool("Caminata", ActivarODesactivar);
    }

    public void AnimSalto(bool ActivarODesactivar) => anim.SetBool("Salto", ActivarODesactivar);

    public void AnimCaida(bool ActivarODesactivar) => anim.SetBool("Caida", ActivarODesactivar);

    public void AnimCaminata(float SpeedAnim)
    {
        anim.speed = SpeedAnim;
        anim.SetBool("Caminata", true);
    }

    public void AnimFarStun(bool ActivarODesactivar) => anim.SetBool("FarStun", ActivarODesactivar);

    public void AnimCloseStun(bool ActivarODesactivar) => anim.SetBool("CloseStun", ActivarODesactivar);

    public void AnimEstatico()
    {
        anim.SetBool("Caminata", false);
        anim.SetBool("Ataque", false);
        anim.SetBool("Bloqueo", false);
    }
}
