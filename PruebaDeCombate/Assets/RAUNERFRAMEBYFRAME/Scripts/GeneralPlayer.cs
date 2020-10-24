using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayer : PlayerEffects
{
    public void CambiaLadoSkin(bool BHRight, bool BHLeft)
    {
        if (BHRight && !BHLeft) transform.eulerAngles = new Vector3(0, 0, 0);

        if (BHLeft && !BHRight) transform.eulerAngles = new Vector3(0, 180, 0);
    }

    #region Detectores de suelo
    //El layer donde pisa el player cambia a un "Layer donde esta el player", de esta manera los enemigos no intentan seguirlo cuando este se encuentra en otras plataformas! 

    private string NombreDelPisoAnterior;
    private string NombreDelPisoNuevo;
    private bool SegundaPasada = false;//Flag
    public void CambioLayerDelSuelo()
    {
        RaycastHit2D ray;
        ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Piso"));
        if (ray)
        {
            NombreDelPisoNuevo = ray.collider.gameObject.name;

            if (NombreDelPisoAnterior != NombreDelPisoNuevo && SegundaPasada)
            {
                GameObject PisoViejo = GameObject.FindGameObjectWithTag("PisoConPlayer_Nuevo");
                PisoViejo.gameObject.tag = "Untagged";
                PisoViejo.gameObject.layer = 8;

                NombreDelPisoAnterior = NombreDelPisoNuevo;

                ray.collider.gameObject.layer = 9; //Cambio el layer que toco y lo transformo en PisoConPlayer
                ray.collider.gameObject.tag = "PisoConPlayer_Nuevo";
            }
            if (!SegundaPasada)
            {
                ray.collider.gameObject.layer = 9; //Cambio el layer que toco y lo transformo en PisoConPlayer
                ray.collider.gameObject.tag = "PisoConPlayer_Nuevo";

                NombreDelPisoAnterior = NombreDelPisoNuevo;

                SegundaPasada = true;//Saco el flag!
            }
        }
    }

    private float largoDelRayo = 2.2f;
    public bool DetectaSuelo()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down * largoDelRayo, Color.blue);
        RaycastHit2D ray;
        return ray = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+3f), Vector2.down, largoDelRayo, LayerMask.GetMask("PisoConPlayer"));
       
        /*if (!ray) 
        {
            if (SaltoCoyote()) return true;
            else return false;
        }
        else
        {
            Tiempo_SaltoCoyote = Tiempo_SaltoCoyoteDefault;
            return true;
        }*/
    }

    /*private float Tiempo_SaltoCoyote = 0.3f;
    private float Tiempo_SaltoCoyoteDefault = 0.3f;
    bool SaltoCoyote() //Puede saltar un tiempo despues de no tocar el suelo. 
    {
        if (Tiempo_SaltoCoyote > 0)
        {
            Tiempo_SaltoCoyote -= Time.deltaTime;
            return true;
        }
        else return false;
        
    }*/
    #endregion

    public void CambioDeGravedad(Rigidbody2D rb)
    {
        if (rb.velocity.y <= -0.1f) rb.gravityScale = 5f;
        else rb.gravityScale = 3f;
    }
}
