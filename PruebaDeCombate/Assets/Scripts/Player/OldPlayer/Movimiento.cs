using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public Inputs En_Inputs;
    public float Speed;

    private void FixedUpdate()
    {
        if (En_Inputs.BH_Right)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.8f, transform.position.y), Speed * Time.deltaTime);
        }
        if (En_Inputs.BH_Left)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.8f, transform.position.y), Speed * Time.deltaTime);
        }
    }

    public void BloqueaMovimiento()
    {
        En_Inputs.BlockWalk = true;
        En_Inputs.QuitForces = true;
    }

    public void DesbloquearMovimiento()
    {
         En_Inputs.BlockWalk = false;
         En_Inputs.QuitForces = false;
    }
}
