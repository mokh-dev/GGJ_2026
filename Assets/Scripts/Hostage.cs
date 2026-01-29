using UnityEngine;

public class Hostage : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Color _infectedColor;
    [SerializeField] private Color _curedColor;


    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color = _infectedColor;
    }

    void Update()
    {
        
    }

    public void CureHostage()
    {
        GameManager.Instance.HostageCured();

        sr.color = _curedColor;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
