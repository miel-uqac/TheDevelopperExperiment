using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ChooseView : MonoBehaviour
{
    public List<Material> screenMaterial;

    public int currentScreen = 0;

    void Start()
    {
        ChangeMaterial(screenMaterial[currentScreen]);
    }

    private void ChangeMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }
}
