using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : MonoBehaviour
{
    public Inputs En_Inputs;

    public bool EstaSuelo;
    public Transform T_Pies;
    private float radio = 0.34f;
    public float FuerzaSalto;
    public LayerMask LayerPiso;

    public float ContadorTiempoSalto;
    public float TiempoSaltoDefault;
    [HideInInspector]
    public bool estaSaltando;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        DetectaSuelo();
        ComienzaSalto();
        SueltaSalto();
        CambiaGravedad();
    }
    private void FixedUpdate()
    {
        if (estaSaltando)
        {
            EjecutaSalto();
        }
    }
    //UPDATE
    void DetectaSuelo()
    {
        EstaSuelo = Physics2D.OverlapCircle(T_Pies.position, radio, LayerPiso);

        if (EstaSuelo)
        {
            En_Inputs.BlockButtons = false;
        }
        else
        {
            En_Inputs.BlockButtons = true;
        }
    }
    void ComienzaSalto()
    {
        if (En_Inputs.BD_Jump && EstaSuelo)
        {
                ContadorTiempoSalto = TiempoSaltoDefault;
                estaSaltando = true;   
        }
    }
    void SueltaSalto()
    {
        if (Input.GetKeyUp(KeyCode.Space) && estaSaltando)
        {
            ContadorTiempoSalto = 0;
            estaSaltando = false;
        }
    }
    void CambiaGravedad()
    {
        if (rb.velocity.y <= -0.1f)
        {
            rb.gravityScale = 4f;
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    //FIXED UPDATE
    void EjecutaSalto()
    {
        if (En_Inputs.BH_Jump && estaSaltando)
        {
            if (ContadorTiempoSalto >= 0)
            {
                rb.velocity = Vector2.up * FuerzaSalto;
                ContadorTiempoSalto -= Time.deltaTime;
            }
            else
            {
                estaSaltando = false;
            }
        }
    }
}
