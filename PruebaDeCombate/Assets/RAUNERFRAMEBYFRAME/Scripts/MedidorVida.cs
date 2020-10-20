using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedidorVida : RaunerCombate
{
    [HideInInspector]
    public bool LlegaDanio;
    public int VidaRauner;

    private bool cancelaDanio_UnInstante;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo")) LlegaDanio = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo")) LlegaDanio = false;
    }

    private void Update()
    {
        if (!cancelaDanio_UnInstante)
        {
            if (LlegaDanio && !CancelaDanio())
            {
                VidaRauner--;
                cancelaDanio_UnInstante = true;
                Invoke("Activa_PosibleDanio", 0.1f);
            }
        }
    }

    void Activa_PosibleDanio()
    {
        cancelaDanio_UnInstante = false;
    }
}
