using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayer : PlayerEffects
{
    //Layers
    [HideInInspector]
    public LayerMask LayerPlayer = 12;
    [HideInInspector]
    public LayerMask LayerPlayerBloqueando = 14;
    [HideInInspector]
    public LayerMask LayerEsquive = 16;
    [HideInInspector]
    public LayerMask LayerEnemigo = 13;
    [HideInInspector]
    public LayerMask LayerEnemigoBloqueando = 15;


    public void CambiaLadoSkin(bool BHRight, bool BHLeft)
    {
        if (BHRight && !BHLeft) transform.eulerAngles = new Vector3(0, 0, 0);

        if (BHLeft && !BHRight) transform.eulerAngles = new Vector3(0, 180, 0);
    }

    #region Detectores de suelo
    //El layer donde pisa el player cambia a un "Layer donde esta el player"
    //de esta manera los enemigos no intentan seguirlo cuando este se encuentra en otras plataformas! 

    private string NombreDelPisoAnterior;
    private string NombreDelPisoNuevo;
    private bool SegundaPasada = false;//Flag


    private int tipoDeSueloNuevo;
    private int tipoDeSueloViejo;
    public void CambioLayerDelSuelo()
    {
        tipoDeSueloNuevo = DetectaTipoDeSuelo();

        if (tipoDeSueloNuevo != 0) //0 => En El aire
        {
            NombreDelPisoNuevo = ray.collider.gameObject.name;

            if (NombreDelPisoAnterior != NombreDelPisoNuevo && SegundaPasada) //Segunda pasada es false por defecto!
            {
                GameObject PisoViejo = GameObject.FindGameObjectWithTag("PisoConPlayer_Nuevo");
                PisoViejo.gameObject.tag = "Untagged";
                PisoViejo.gameObject.layer = tipoDeSueloViejo;

                NombreDelPisoAnterior = NombreDelPisoNuevo;
                tipoDeSueloViejo = tipoDeSueloNuevo;

                ray.collider.gameObject.layer = 9; //Cambio el layer que toco y lo transformo en PisoConPlayer
                ray.collider.gameObject.tag = "PisoConPlayer_Nuevo";
            }
            if (!SegundaPasada)
            {
                ray.collider.gameObject.layer = 9; //Cambio el layer que toco y lo transformo en PisoConPlayer
                ray.collider.gameObject.tag = "PisoConPlayer_Nuevo";

                NombreDelPisoAnterior = NombreDelPisoNuevo;
                tipoDeSueloViejo = tipoDeSueloNuevo;

                SegundaPasada = true;//Saco el flag!
            }
        }
    }

    private RaycastHit2D ray;
    public int DetectaTipoDeSuelo()
    {
        ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Pasto"));
        if (ray) return 19;  //Pasto // 19
        else ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Agua"));
        if(ray) return 18; //Agua // 18
        else ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Tierra"));
        if (ray) return 17; //Tierra // 17
        else ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Plataforma"));
        if (ray) return 17; //Tierra // 17
        else return 0; //EnElAire
    }

    private float largoDelRayo = 2.2f;
    public bool DetectaSuelo()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down * largoDelRayo, Color.blue);
        RaycastHit2D ray;
        return ray = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+3f), Vector2.down, largoDelRayo, LayerMask.GetMask("PisoConPlayer"));
    }

    #endregion

    public void CambioDeGravedad(Rigidbody2D rb)
    {
        if (rb.velocity.y <= -0.1f) rb.gravityScale = 5f;
        else rb.gravityScale = 3f;
    }

}
