using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosEnemigoSimple : AnimacionesEnemigos
{
    public Animator EnemigoSimpleEfectos; 

    public void EfectosDelCombo(int QueGolpe, bool GolpeoAlEscudo)
    {
        if (!GolpeoAlEscudo)
        {
            QueGolpe = DetectaGolpesAnteriores(QueGolpe);

            switch (QueGolpe)
            {
                case 1:
                    EnemigoSimpleEfectos.SetBool("Golpe1", true);
                    break;
                case 2:
                    EnemigoSimpleEfectos.SetBool("Golpe2", true);
                    break;
                case 3:
                    EnemigoSimpleEfectos.SetBool("Golpe3", true); 
                    break;
                default:
                    Debug.Log("El efecto de golpe no pudo ser procesado!, el numero es incorrecto");
                    break;
            }
        }
        else EnemigoSimpleEfectos.SetBool("GolpeAEscudo", true);
    }

    private int DetectaGolpesAnteriores(int QueGolpe)
    {
        if (cantidadDeGolpesDados == 0)
        {
            cantidadDeGolpesDados = 1;
            return 1;
        }
        else if (cantidadDeGolpesDados == 1)
        {
            cantidadDeGolpesDados = 2;
            return 2;
        }
        else if (cantidadDeGolpesDados == 2)
        {
            cantidadDeGolpesDados = 3;
            return 3;
        }
        else return 0;
    }

    private int cantidadDeGolpesDados;
    public void CancelarEfectosDelCombo()
    {
        cantidadDeGolpesDados = 0;

        EnemigoSimpleEfectos.SetBool("Golpe1", false);
        EnemigoSimpleEfectos.SetBool("Golpe2", false);
        EnemigoSimpleEfectos.SetBool("Golpe3", false);
        EnemigoSimpleEfectos.SetBool("GolpeAEscudo", false);
    }
}
