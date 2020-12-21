using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desplazamiento : MonoBehaviour
{
    public Inputs En_Inputs;

    //Se Controla desde el modo de juego.
    [HideInInspector]
    public float FuerzaDesplazamiento;
    [HideInInspector]
    public float ContadorDesplazamiento;
    [HideInInspector]
    public float ContadorDefaultDesplazamiento;
    [HideInInspector]
    public bool Derecha;
    [HideInInspector]
    public bool Izquierda;

    public float CadenciaDash;
    private bool puedeDesplazarse = true;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ContadorDesplazamiento = ContadorDefaultDesplazamiento;
    }
    private void Update()
    {
        LecturaInputs();

    }
    private void FixedUpdate()
    {
        if (Izquierda || Derecha)
        {
            EjecutoDesplazamiento();
        }
    }
    //Update
    void LecturaInputs()
    {
        if (puedeDesplazarse)
        {
            if (En_Inputs.BH_Dash && En_Inputs.BH_Right)
            {
                Izquierda = false;
                Derecha = true;
            }
            if (En_Inputs.BH_Dash && En_Inputs.BH_Left)
            {
                Derecha = false;
                Izquierda = true;
            }
        }
    }
    //FixedUpdate
    private void EjecutoDesplazamiento()
    {
        if (ContadorDesplazamiento <= 0)
        {
            Izquierda = false;
            Derecha = false;
            En_Inputs.BlockButtons = false;


            ContadorDesplazamiento = ContadorDefaultDesplazamiento;
            Invoke("In_CadenciaDash", CadenciaDash);

            rb.velocity = Vector2.zero;
        }
        else
        {
            En_Inputs.BlockButtons = true;
            puedeDesplazarse = false;

            ContadorDesplazamiento -= Time.deltaTime;
            if (Izquierda)
            {
                rb.velocity = Vector2.left * FuerzaDesplazamiento;
            }
            if (Derecha)
            {
                rb.velocity = Vector2.right * FuerzaDesplazamiento;
            }
        }
    }

    //Invokes
    private void In_CadenciaDash()
    {
        puedeDesplazarse = true;
    }
}
