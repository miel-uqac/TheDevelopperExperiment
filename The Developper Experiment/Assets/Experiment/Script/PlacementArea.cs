using System;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public event Action<bool> OnCubeInAreaChanged;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("L"))
        {
            OnCubeInAreaChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("L"))
        {
            OnCubeInAreaChanged?.Invoke(false);
        }
    }
}
