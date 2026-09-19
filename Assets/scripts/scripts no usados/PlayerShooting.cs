using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Configuración")]
    [SerializeField] private float shootForce = 25f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        GameObject bala = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * shootForce;

        Projectile proyectil = bala.GetComponent<Projectile>();
        proyectil.Inicializar(shootForce, firePoint.forward);
    }
}