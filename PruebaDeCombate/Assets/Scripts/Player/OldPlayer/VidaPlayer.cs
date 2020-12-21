using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaPlayer : MonoBehaviour
{
    public int Vida;
    public BloqueoV2 En_BloqueoV2;
    public void Atacado(int Danio)
    {
        if (!En_BloqueoV2.ActivarEscudo)
        {
            Vida = Vida - Danio;
            if (Vida <= 0) { Destroy(gameObject); }
        }
    }
}
