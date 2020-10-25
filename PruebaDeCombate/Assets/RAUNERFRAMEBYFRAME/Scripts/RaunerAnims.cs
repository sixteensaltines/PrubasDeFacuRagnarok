using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerAnims : MonoBehaviour
{
    public void CaminataAnim(Animator anim, bool CaminataActiva)
    {
        anim.SetBool("Caminata", CaminataActiva);
    }

    public void SaltoAnim(Animator anim,Rigidbody2D rb)//Salta
    {
        if (rb.gravityScale == 5)
        { anim.SetBool("Salto", false); anim.SetBool("Caida", true); }
        else { anim.SetBool("Caida", false); anim.SetBool("Salto", true); }
    }

    public void Caida_Salto_Off(Animator anim)
    {
        anim.SetBool("Salto", false);
        anim.SetBool("Caida", false);
    }

    public void DashAnim(Animator anim, bool Encendido_Apagado) => anim.SetBool("Dash", Encendido_Apagado);

    public void AnimBloqueo(Animator anim, bool Encendido_Apagado) => anim.SetBool("Block", Encendido_Apagado);

    public void GolpeAnim(Animator anim, int QueGolpe, bool Encendido_Apagado)
    {
        anim.SetBool("Ataque", Encendido_Apagado);
        anim.SetInteger("QueAtaque", QueGolpe);
    }
}
