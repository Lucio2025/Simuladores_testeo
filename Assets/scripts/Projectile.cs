using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Auto-destrucción")]
    [SerializeField] private float tiempoDeVida = 30f;

    private Rigidbody rb;
    private float tiempoDeDisparo;
    private float velocidadInicial;
    private float anguloDeLanzamiento;
    private Vector3 posicionInicial;
    private Vector3 posicionAnterior;
    private float distanciaRecorridaReal = 0f;
    private bool yaImpacto = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Inicializar(float velocidad, Vector3 direccionDisparo)
    {
        velocidadInicial = velocidad;
        tiempoDeDisparo = Time.time;
        posicionInicial = transform.position;
        posicionAnterior = transform.position;

        Vector3 direccionHorizontal = new Vector3(direccionDisparo.x, 0f, direccionDisparo.z);
        anguloDeLanzamiento = Vector3.SignedAngle(direccionHorizontal, direccionDisparo, Vector3.Cross(direccionHorizontal, Vector3.up));
    }

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
        AsignarColorAleatorio();
    }

    private void AsignarColorAleatorio()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

    private void FixedUpdate()
    {
        if (yaImpacto) return;

        distanciaRecorridaReal += Vector3.Distance(posicionAnterior, transform.position);
        posicionAnterior = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (yaImpacto) return;
        yaImpacto = true;

        float tiempoDeVuelo = Time.time - tiempoDeDisparo;
        Vector3 puntoDeImpacto = collision.contacts[0].point;

        // Datos que pide la consigna: velocidad relativa e impulso, directo del motor de física
        float velocidadRelativa = collision.relativeVelocity.magnitude;
        float impulso = collision.impulse.magnitude;

        Debug.Log($"[IMPACTO] {collision.gameObject.name} | Vuelo: {tiempoDeVuelo:F2}s | " +
                   $"Vel. relativa: {velocidadRelativa:F2} | Impulso: {impulso:F2}");

        if (ShotReportManager.Instance != null)
        {
            ShotReportManager.Instance.RegistrarImpacto(
                collision.gameObject.name,
                tiempoDeVuelo,
                puntoDeImpacto,
                velocidadRelativa,
                impulso
            );
        }
    }
}