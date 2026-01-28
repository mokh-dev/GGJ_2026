using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _lookRotationOffset;
    private Vector2 moveInput;


    [Header("Shooting")]
    [SerializeField] private GameObject _bulletPre;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _firingCooldownTime;
    [SerializeField] private float _bulletLifeSpan;
    private bool canFire = true;


    [Header("Other")]
    private Rigidbody2D rb;
    private Camera cam;






    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        PointPlayerToMouse();

        GetMoveInput();
        PlayerMovement();

        if (Input.GetButtonDown("Fire1")) MouseDown();
    }

    private void MouseDown()
    {
        if (canFire) FireBullet();

    }




    private void PointPlayerToMouse()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mousePosition - (Vector2)transform.position).normalized;

        float playerAngle = (Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg) + _lookRotationOffset;


        rb.SetRotation(playerAngle);
    }

    private void GetMoveInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void PlayerMovement()
    {
        rb.linearVelocity = moveInput.normalized * _runSpeed;
    }

    private void FireBullet()
    {
        Quaternion playerRotation = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward);
        GameObject bullet = Instantiate(_bulletPre, _firePoint.position, playerRotation);
        Destroy(bullet, _bulletLifeSpan);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.up * _bulletSpeed, ForceMode2D.Impulse);
        

        canFire = false;
        StartCoroutine(FiringCooldown());        
    }

    private IEnumerator FiringCooldown()
    {
        yield return new WaitForSeconds(_firingCooldownTime);
        canFire = true;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("player hit");
        }
    }
}
