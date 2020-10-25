using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoProvisional : MonoBehaviour
{ //Medidor De Vida Del enemigo

    public int Vida_Total;
    private int VidaRestante;

    private Animator Anim;
    void Start()
    {
        VidaRestante = Vida_Total;
        Anim = GetComponent<Animator>();
    }



    public void LlegaDanio(int Danio)
    {
        if (VidaRestante <= 0)
        {
            Anim.SetBool("Muerte", true);
        }
        else
        {
            Anim.SetBool("Danio", true);
            VidaRestante -= Danio;
        }
    }

    public void DanioOff() => Anim.SetBool("Danio", false);

    void Muerte()
    {
        Destroy(gameObject);
    }
}
