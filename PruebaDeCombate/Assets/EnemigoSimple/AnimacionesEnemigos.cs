using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesEnemigos : MonoBehaviour
{
    public Animator anim;

    public void AnimAtaque(int QueCombo, int CuantosGolpes) //TODO: FALTA TERMINAR
    {
        if (QueCombo == 1)
        {
            anim.SetBool("Ataque", true);
            anim.SetInteger("CantidadAtaques", CuantosGolpes);
            anim.SetInteger("QueCombo", QueCombo);
        }
        else if (QueCombo == 2)
        {
            //EN CASO DE AGREGAR UN SEGUNDO COMBO
        }
    }

    public void OffAtaque() => anim.SetBool("Ataque", false);

    public void AnimBloqueo_Ocasional(bool ActivarODesactivar) => anim.SetBool("BloqueoOcasional", ActivarODesactivar);

    public void AnimBloqueo_Random(bool ActivarODesactivar) => anim.SetBool("BloqueoRandom", ActivarODesactivar);

    public void AnimCaminata(bool ActivarODesactivar) => anim.SetBool("Caminata", ActivarODesactivar);

    public void AnimSalto(bool ActivarODesactivar) => anim.SetBool("Salto", ActivarODesactivar);

    public void AnimCaida(bool ActivarODesactivar) => anim.SetBool("Caida", ActivarODesactivar);

    public void AnimCaminata(float SpeedAnim)
    {
        anim.speed = SpeedAnim;
        anim.SetBool("Caminata", true);
    }

    public void AnimEsquive(bool ActivarODesactivar) => anim.SetBool("Esquive", ActivarODesactivar);

    public void AnimEstatico()
    {
        anim.SetBool("Caminata", false);
        anim.SetBool("Ataque", false);
        anim.SetBool("BloqueoRandom", false);
        anim.SetBool("Esquive", false);
    }

    public void AnimMuerte()
    {
        anim.SetBool("SimpleEnemyDeath", true);
    }

    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
