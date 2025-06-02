using System;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public event Action<bool> OnCubeInAreaChanged;

    public bool inArea = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            inArea = true;
            OnCubeInAreaChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            inArea = false;
            OnCubeInAreaChanged?.Invoke(false);
        }
    }
}
