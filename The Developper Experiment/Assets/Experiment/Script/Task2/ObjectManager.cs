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

    private void Start()
    {
        manager = FindFirstObjectByType<SpawnManager>();
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
            manager.rounds--;
            if (manager.rounds == 0)
            {
                manager.WriteResults();
                manager.EndAllTask();
                return;
            }
            manager.WriteResults();
            manager.FirstTask();
            return;
        }

        if (isFake)
        {
            manager.rounds--;
            if (manager.rounds == 0)
            {
                manager.WriteResults();
                manager.EndAllTask();
                return;
            }
            manager.WriteResults();
            manager.SecondTask();
            return;
        }
        if (isMoving)
        {
            manager.rounds--;
            if (manager.rounds == 0)
            {
                manager.WriteResults();
                manager.EndAllTask();
                return;
            }
            manager.WriteResults();
            manager.ThirdTask();
            return;
        }
        manager.errors++;
        Destroy(gameObject.transform.parent.gameObject);
    }
}
