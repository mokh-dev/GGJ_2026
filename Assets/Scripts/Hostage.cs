using NUnit.Framework;
using UnityEngine;

public class Hostage : MonoBehaviour
{

    [SerializeField] private Color _infectedColor;
    [SerializeField] private Color _curedColor;
    [SerializeField] private float _followDistance;
    [SerializeField] private float _moveSpeed;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private Player player;

    public bool IsSaved;
    public Transform SafeAreaPos;




    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        joint = gameObject.GetComponent<DistanceJoint2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        sr.color = _infectedColor;
    }

    void Update()
    {
        if (IsSaved == true)
        {
            Vector2 safeAreaDirection = (SafeAreaPos.position - transform.position).normalized;
            rb.AddForce(safeAreaDirection * _moveSpeed);
        }
    }

    public void SaveHostage()
    {
        joint.enabled = false;
        IsSaved = true;
    }

    public void CureHostage(GameObject objToConnect)
    {
        GameManager.Instance.HostageCured();

        sr.color = _curedColor;

        joint.enabled = true;
        joint.connectedBody = objToConnect.GetComponent<Rigidbody2D>();
        joint.distance = _followDistance;
    }


    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("SafeArea"))
        {
            player.SaveAllConnectedHostages(collision.gameObject.transform);
            GameManager.Instance.HostageSaved();
        }
    }
}
