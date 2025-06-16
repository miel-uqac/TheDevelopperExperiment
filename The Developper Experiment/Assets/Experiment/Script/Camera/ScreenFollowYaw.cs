using UnityEngine;

public class ScreenFollowYaw : MonoBehaviour
{
    public Transform headTransform; // La tête (souvent la caméra VR)
    public Vector3 offset = new Vector3(0f, -0.3f, 0.5f); // Position relative

    void LateUpdate()
    {
        if (headTransform == null) return;

        // Récupérer uniquement l'angle Y (rotation autour de l'axe vertical)
        float yaw = headTransform.eulerAngles.y;
        Quaternion yawRotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, yaw, gameObject.transform.eulerAngles.z);

        // Appliquer la rotation Y à l'écran
        transform.rotation = yawRotation;

        // Placer l'objet avec un offset selon la rotation Y seulement
        transform.position = headTransform.position + yawRotation * offset;
    }
}
