using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Animator m_Animator;
    private SpawnManager manager;

    public bool isRectangle = false;
    public bool isFake = false;
    public bool isMoving = false;
    public float speed = 100;

    private void Awake()
    {
        m_Animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.parent.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    public void HoverEntered()
    {
        m_Animator.SetBool("Hovered", true);
    }

    public void HoverExited()
    {
        m_Animator.SetBool("Hovered", false);
    }

    public void Selected()
    {
        if (isRectangle)
        {
            manager = FindFirstObjectByType<SpawnManager>();
            manager.SecondTask();
            return;
        }

        if (isFake) 
        {
            manager = FindFirstObjectByType<SpawnManager>();
            manager.ThirdTask();
            return;
        }

        if (isMoving)
        {
            manager = FindFirstObjectByType<SpawnManager>();
            manager.EndAllTask();
            return;
        }

        Destroy(gameObject.transform.parent.gameObject);
    }
}
