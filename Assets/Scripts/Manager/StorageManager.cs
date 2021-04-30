using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    //Material
    public static readonly Material redSkin = Resources.Load("Prefab/Material/Plane/RedPlane") as Material;
    public static readonly Material blueSkin = Resources.Load("Prefab/Material/Plane/BluePlane") as Material;
    public static readonly Material greenkin = Resources.Load("Prefab/Material/Plane/Greenlane") as Material;
    public static readonly Material orangeSkin = Resources.Load("Prefab/Material/Plane/OrangePlane") as Material;

    //Buster
    public static readonly GameObject cubeBuster = Resources.Load("Prefab/Particle/FX_Prefab/Buster/FX_CubeBuster") as GameObject;
    public static readonly GameObject starBuster = Resources.Load("Prefab/Particle/FX_Prefab/Buster/FX_StarBuster") as GameObject;

}
