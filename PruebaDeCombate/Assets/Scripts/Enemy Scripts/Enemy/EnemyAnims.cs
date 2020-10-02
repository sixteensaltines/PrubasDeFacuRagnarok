using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnims : MonoBehaviour
{
    public void AnimAtaque(Animator anim, bool ActivaODesactiva)
    {
        anim.SetBool("Ataque", ActivaODesactiva);
    }

    public void AnimBloqueo(Animator anim, bool ActivaODesactiva)
    {
        anim.SetBool("Bloqueo", ActivaODesactiva);
    }

    public void AnimEstatico(Animator anim, bool ActivaODesactiva)
    {
        anim.SetBool("Estatico", ActivaODesactiva);

        anim.SetBool("Ataque", false);
        anim.SetBool("Bloqueo", false);
    }


}
