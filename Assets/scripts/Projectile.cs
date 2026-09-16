using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Auto-destrucción")]
    [SerializeField] private float tiempoDeVida = 30f;

    private float tiempoDeDisparo;
    private float velocidadInicial;
    private float anguloDeLanzamiento;
    private Vector3 posicionInicial;
    private Vector3 posicionAnterior;
    private float distanciaRecorridaReal = 0f;
    private bool yaImpacto = false;

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
            Color colorAleatorio = new Color(Random.value, Random.value, Random.value);
            renderer.material.color = colorAleatorio; // renderer.material ya crea una instancia propia, no pisa el prefab
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
        float distanciaLineaRecta = Vector3.Distance(posicionInicial, transform.position);
        Rigidbody rb = GetComponent<Rigidbody>();
        float velocidadImpacto = rb.linearVelocity.magnitude;

        Debug.Log($"[IMPACTO] Objeto golpeado: {collision.gameObject.name}");
        Debug.Log($"Tiempo de vuelo: {tiempoDeVuelo:F2} s");
        Debug.Log($"Velocidad de salida: {velocidadInicial:F2} u/s");
        Debug.Log($"Velocidad al impactar: {velocidadImpacto:F2} u/s");
        Debug.Log($"Ángulo de lanzamiento: {anguloDeLanzamiento:F1}°");
        Debug.Log($"Distancia recorrida (trayectoria real): {distanciaRecorridaReal:F2} u");
        Debug.Log($"Distancia en línea recta (inicio-impacto): {distanciaLineaRecta:F2} u");
        Debug.Log($"Punto de impacto: {collision.contacts[0].point}");

        // Actualiza el panel de UI, si existe uno en la escena
        if (DebugUIManager.Instance != null)
        {
            DebugUIManager.Instance.MostrarDatosDeImpacto(
                collision.gameObject.name,
                tiempoDeVuelo,
                velocidadInicial,
                velocidadImpacto,
                anguloDeLanzamiento,
                distanciaRecorridaReal,
                distanciaLineaRecta,
                collision.contacts[0].point
            );
        }
    }
}