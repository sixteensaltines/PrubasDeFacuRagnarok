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

    private float largoDelRayo = 2.2f;
    public bool DetectaSuelo()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 3f), Vector2.down * largoDelRayo, Color.blue);
        RaycastHit2D ray;
        return ray = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+3f), Vector2.down, largoDelRayo, LayerMask.GetMask("Piso"));
    }

    public void CambioDeGravedad(Rigidbody2D rb)
    {
        if (rb.velocity.y <= -0.1f) rb.gravityScale = 5f;
        else rb.gravityScale = 3f;
    }
}
