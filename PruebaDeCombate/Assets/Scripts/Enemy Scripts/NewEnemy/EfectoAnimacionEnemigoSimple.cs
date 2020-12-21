using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoAnimacionEnemigoSimple : MonoBehaviour
{
    //README.: Enlase directo con EnemigoSimple

    private EnemigoSimple en_EnemigoSimple;
    public GameObject SkinFalso;

    void Start()
    {
        SkinFalso.GetComponent<SpriteRenderer>().enabled = false;
        en_EnemigoSimple = GetComponentInParent<EnemigoSimple>();
    }

    public void EnviaDanio() => en_EnemigoSimple.EnviaDanio();

    public void EndAttack() => en_EnemigoSimple.EndAttack();

    public void DestruirObjeto() => en_EnemigoSimple.DestruirObjeto();
}
