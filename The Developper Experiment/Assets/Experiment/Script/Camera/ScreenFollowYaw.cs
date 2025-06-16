using UnityEngine;

public class ScreenFollowYaw : MonoBehaviour
{
    public Transform headTransform; // La t�te (souvent la cam�ra VR)
    public Vector3 offset = new Vector3(0f, -0.3f, 0.5f); // Position relative

    void LateUpdate()
    {
        if (headTransform == null) return;

        // R�cup�rer uniquement l'angle Y (rotation autour de l'axe vertical)
        float yaw = headTransform.eulerAngles.y;
        Quaternion yawRotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, yaw, gameObject.transform.eulerAngles.z);

        // Appliquer la rotation Y � l'�cran
        transform.rotation = yawRotation;

        // Placer l'objet avec un offset selon la rotation Y seulement
        transform.position = headTransform.position + yawRotation * offset;
    }
}
