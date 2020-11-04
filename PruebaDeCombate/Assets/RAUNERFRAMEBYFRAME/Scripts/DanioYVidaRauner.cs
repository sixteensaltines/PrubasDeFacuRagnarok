using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanioYVidaRauner : GeneralPlayer
{
    public int Vida;
    public Animator anim;
    private RaunerInputs raunerInputs;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        raunerInputs = GetComponent<RaunerInputs>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (EmpujeOn) Empuje();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo") && DetectaSuelo() && !EmpujeOn)
        {
            EmpujeOn = true;
        }
        if (collision.CompareTag("Enemigo") && !DetectaSuelo() && !EmpujeOn)
        {
            LlegaDanio();
        }
    }

    private bool EmpujeOn;

    public float FuerzaEmpuje;
    public float ContadorEmpujeDefault;
    public float CadenciaEmpuje;

    private float contadorEmpuje;

    private bool izquierda;
    private bool derecha;

    private bool puedenEmpujarlo = true;

    private bool flag1;

    void Empuje()
    {
        //Dash
        if (!flag1)
        {
            contadorEmpuje = ContadorEmpujeDefault; //Variable de dash en deulfat
            flag1 = true;
        }

        if (puedenEmpujarlo)
        {
            LlegaDanio();

            if (transform.eulerAngles.y == 0) //Mira a la derecha
            {
                derecha = false;
                izquierda = true;
            }
            if (transform.eulerAngles.y == 180) //Mira a la izquierda
            {
                izquierda = false;
                derecha = true;
            }
        }

        if (derecha || izquierda)
        {
            EjecutoDesplazamiento();
        }
    }
    void EjecutoDesplazamiento()
    {
        if (contadorEmpuje <= 0)
        {

            izquierda = false;
            derecha = false;
            rb.velocity = Vector2.zero;

            EmpujeOn = false; //Asi cancela el empuje! 

            Invoke("In_SaleDelStun", CadenciaEmpuje);
        }
        else
        {
            raunerInputs.BlockButtons = true;
            raunerInputs.QuitForces();

            Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerPlayer, true);

            anim.SetBool("Empuje", true);

            puedenEmpujarlo = false;

            contadorEmpuje -= Time.deltaTime;


            if (izquierda)
            {
                rb.velocity = Vector2.left * FuerzaEmpuje * Time.deltaTime;
            }
            if (derecha)
            {
                rb.velocity = Vector2.right * FuerzaEmpuje * Time.deltaTime;
            }
        }
    }
    private void In_SaleDelStun()
    {
        contadorEmpuje = ContadorEmpujeDefault;

        raunerInputs.BlockButtons = false;

        Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerPlayer, false);

        puedenEmpujarlo = true;
    }

    public void LlegaDanio()
    {
        if (gameObject.layer == LayerPlayer) DescuentaVida();
    }

    public void DescuentaVida()
    {
        Vida--;
        if (Vida <= 0)
        {
            anim.SetBool("RaunerMuerte", true);
        }
        else { }//Danio
    }
}

    /*
    public Transform PosicionDisparaFlecha; 
    public GameObject FlechaPrefab;

    private float randomTimeDefault;
    private float randomTime;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        randomTimeDefault = Random.Range(2f, 4f);
        randomTime = randomTimeDefault;
    }

    void Update()
    {
        if (randomTime < 0)
        {
            if (player.GetComponent<GeneralPlayer>().DetectaSuelo())
            { LanzaFlecha(); }
        }
        else { randomTime -= Time.deltaTime; }
    }

    void LanzaFlecha()
    {
        Instantiate(FlechaPrefab, PosicionDisparaFlecha.position, Quaternion.identity);
        randomTime = randomTimeDefault;
    }*/

