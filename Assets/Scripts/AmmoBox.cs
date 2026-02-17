
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 60;
    public AmmoType ammoType;

    public enum AmmoType
    {
        pistolAmmo,
        RifleAmmo
    }


}
