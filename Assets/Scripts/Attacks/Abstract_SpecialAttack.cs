using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abstract_SpecialAttack : MonoBehaviour
{

    public abstract void SetPlayer(PlayerMovement player);
    public abstract float GetYOffset();

}
