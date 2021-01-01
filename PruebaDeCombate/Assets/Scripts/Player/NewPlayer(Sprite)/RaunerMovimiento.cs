using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerMovimiento : GeneralPlayer
{
    private RaunerInputs raunerInputs;
    private Rigidbody2D rb;
    private Animator anim;

    public GameObject CollidersObject;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        raunerInputs = GetComponent<RaunerInputs>();
    }

    void Update()
    {
        CambioLayerDelSuelo();
        Caminata(); //Con sus animaciones respectivas
        CambiaLadoSkin(raunerInputs.BH_Right, raunerInputs.BH_Left);
        Salto(); //Con sus animaciones respectivas
        CambioDeGravedad(rb);
        Dash();
    }

    void FixedUpdate()
    {
 //Con sus animaciones respectivas
    }

    //Caminata
    public float VelocidadCaminata;
    void Caminata()
    {
        if (raunerInputs.BH_Right)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.8f, transform.position.y), VelocidadCaminata * Time.deltaTime);
            if (DetectaSuelo()) CaminataAnim(anim, true);
            else CaminataAnim(anim, false);
        }
        else if (raunerInputs.BH_Left)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.8f, transform.position.y), VelocidadCaminata * Time.deltaTime);
            if (DetectaSuelo()) CaminataAnim(anim, true);
            else CaminataAnim(anim, false);
        }
        else CaminataAnim(anim, false);
    }

    //Salto
    public float FuerzaSalto;
    void Salto()
    {
        if (DetectaSuelo() && raunerInputs.BD_Jump)
        {
            rb.AddForce(new Vector2(0f, FuerzaSalto), ForceMode2D.Impulse);
        }
        if (DetectaSuelo())
        {
            Caida_Salto_Off(anim);
        }
        else
        {
            SaltoAnim(anim, rb);
        }
    }

    //Dash
    public float FuerzaDesplazamiento;
    public float ContadorDesplazamientoDefault;
    public float CadenciaDash;

    private float contadorDesplazamiento;

    private bool izquierda;
    private bool derecha;

    private bool puedeDesplazarse = true;

    private bool flag1;


    void Dash()
    {
        if (!flag1)
        {
            contadorDesplazamiento = ContadorDesplazamientoDefault; //Variable de dash en deulfat
            flag1 = true;
        }

        if (puedeDesplazarse)
        {
            if (raunerInputs.BD_Dash && transform.eulerAngles.y == 0) //Mira a la derecha
            {
                izquierda = false;
                derecha = true;
            }
            if (raunerInputs.BD_Dash && transform.eulerAngles.y == 180) //Mira a la izquierda
            {
                derecha = false;
                izquierda = true;
            }
        }

        if (derecha || izquierda)
        {
            EjecutoDesplazamiento();
        }
    }


    void EjecutoDesplazamiento()
    {
        if (contadorDesplazamiento <= 0)
        {
            izquierda = false;
            derecha = false;

            raunerInputs.BlockButtons = false;

            Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerEsquive, false);

            CollidersObject.layer = 12;

            contadorDesplazamiento = ContadorDesplazamientoDefault;
            Invoke("In_CadenciaDash", CadenciaDash);

            DashAnim(anim, false);

            rb.velocity = Vector2.zero;
        }
        else
        {
            raunerInputs.BlockButtons = true;
            raunerInputs.QuitForces();

            Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerEsquive, true);


            CollidersObject.layer = 16; //PlayerBloqueando

            puedeDesplazarse = false;

            DashAnim(anim, true);

            contadorDesplazamiento -= Time.deltaTime;
            if (izquierda)
            {
                rb.velocity = Vector2.left * FuerzaDesplazamiento;
            }
            if (derecha)
            {
                rb.velocity = Vector2.right * FuerzaDesplazamiento;
            }
        }
    }
    private void In_CadenciaDash()
    {
        puedeDesplazarse = true;
    }
}
