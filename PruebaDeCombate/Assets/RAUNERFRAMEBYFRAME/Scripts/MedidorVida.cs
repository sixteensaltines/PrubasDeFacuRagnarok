using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedidorVida : RaunerCombate
{
    public bool LlegaDanio;
    public int VidaRauner;

    private bool cancelaDanio_UnInstante;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo")) LlegaDanio = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
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
