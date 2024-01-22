using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{

    public abstract void GetCollected(PlayerStatHandler player);
    public abstract void LoseUpgrade(Vector3 lossPosition);
    public abstract void LoseUpgrade(Vector3 lossPosition, PlayerStatHandler player);
    public abstract void DespawnUpgrade();
    

}
