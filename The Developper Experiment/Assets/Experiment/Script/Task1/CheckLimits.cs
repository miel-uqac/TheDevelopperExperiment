using UnityEngine;

public class CheckLimits : MonoBehaviour
{

    public PlacementChecker Manager;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "L")
        {
           Manager.RestartWave();
        }
    }
}
