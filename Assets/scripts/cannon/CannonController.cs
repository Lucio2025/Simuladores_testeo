using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CannonController : MonoBehaviour
{
    [Header("Referencias del Cañón")]
    [SerializeField] private Transform cannonPivot;
    [SerializeField] private Transform cannonBase;
    [SerializeField] private Transform firePoint;

    [Header("Proyectil")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Sliders")]
    [SerializeField] private Slider sliderAnguloVertical;
    [SerializeField] private Slider sliderAnguloHorizontal;
    [SerializeField] private Slider sliderFuerza;
    [SerializeField] private Slider sliderMasa;

    [Header("Etiquetas de valores")]
    [SerializeField] private TextMeshProUGUI etiquetaAnguloVertical;
    [SerializeField] private TextMeshProUGUI etiquetaAnguloHorizontal;
    [SerializeField] private TextMeshProUGUI etiquetaFuerza;
    [SerializeField] private TextMeshProUGUI etiquetaMasa;

    [Header("Previsualización de trayectoria")]
    [SerializeField] private LineRenderer lineaTrayectoria;
    [SerializeField] private GameObject marcadorImpacto;
    [Tooltip("Cantidad de puntos de la curva. Más puntos = más precisión, pero más costo.")]
    [SerializeField] private int cantidadPuntos = 80;
    [Tooltip("Segundos de simulación entre punto y punto.")]
    [SerializeField] private float pasoDeTiempo = 0.05f;
    [Tooltip("Radio del proyectil, para que el raycast detecte el impacto como lo haría la bala real.")]
    [SerializeField] private float radioProyectil = 0.1f;

    private Quaternion rotacionPivotInicial;
    private Quaternion rotacionBaseInicial;

    private void Start()
    {
        rotacionPivotInicial = cannonPivot.localRotation;
        rotacionBaseInicial = cannonBase.localRotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        AplicarRotacion();
        ActualizarEtiquetas();
        DibujarTrayectoria();
    }

    private void AplicarRotacion()
    {
        float anguloVertical = sliderAnguloVertical.value;
        cannonPivot.localRotation = rotacionPivotInicial * Quaternion.Euler(-anguloVertical, 0f, 0f);

        float anguloHorizontal = sliderAnguloHorizontal.value;
        cannonBase.localRotation = rotacionBaseInicial * Quaternion.Euler(0f, anguloHorizontal, 0f);
    }

    private void ActualizarEtiquetas()
    {
        etiquetaAnguloVertical.text = $"Ángulo vertical: {sliderAnguloVertical.value:F1}°";
        etiquetaAnguloHorizontal.text = $"Ángulo horizontal: {sliderAnguloHorizontal.value:F1}°";
        etiquetaFuerza.text = $"Fuerza: {sliderFuerza.value:F1}";
        etiquetaMasa.text = $"Masa: {sliderMasa.value:F2} kg";
    }

    // Calcula la velocidad de salida real. Como disparamos con ForceMode.Impulse,
    // la velocidad resultante es fuerza / masa.
    private float CalcularVelocidadSalida()
    {
        return sliderFuerza.value / sliderMasa.value;
    }

    private void DibujarTrayectoria()
    {
        if (lineaTrayectoria == null) return;

        Vector3 posicion = firePoint.position;
        Vector3 velocidad = firePoint.forward * CalcularVelocidadSalida();
        Vector3 gravedad = Physics.gravity;

        lineaTrayectoria.positionCount = cantidadPuntos;
        lineaTrayectoria.SetPosition(0, posicion);

        bool huboImpacto = false;

        for (int i = 1; i < cantidadPuntos; i++)
        {
            // Integración simple de la parábola: avanzamos un paso de tiempo
            Vector3 posicionAnterior = posicion;
            velocidad += gravedad * pasoDeTiempo;
            posicion += velocidad * pasoDeTiempo;

            // Chequeamos si el tramo entre el punto anterior y el actual choca con algo
            Vector3 direccionTramo = posicion - posicionAnterior;
            float distanciaTramo = direccionTramo.magnitude;

            if (Physics.SphereCast(posicionAnterior, radioProyectil, direccionTramo.normalized,
                                   out RaycastHit hit, distanciaTramo))
            {
                // Cortamos la línea justo en el punto de impacto
                lineaTrayectoria.positionCount = i + 1;
                lineaTrayectoria.SetPosition(i, hit.point);

                if (marcadorImpacto != null)
                {
                    marcadorImpacto.SetActive(true);
                    // Lo despegamos un poquito de la superficie para que no quede enterrado
                    marcadorImpacto.transform.position = hit.point + hit.normal * 0.02f;
                    // Lo orientamos según la superficie golpeada (piso, pared, rampa...)
                    marcadorImpacto.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }

                huboImpacto = true;
                break;
            }

            lineaTrayectoria.SetPosition(i, posicion);
        }

        // Si la trayectoria no choca con nada (se pierde en el horizonte), ocultamos el marcador
        if (!huboImpacto && marcadorImpacto != null)
        {
            marcadorImpacto.SetActive(false);
        }
    }

    public void Disparar()
    {
        GameObject bala = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bala.GetComponent<Rigidbody>();
        rb.mass = sliderMasa.value;
        rb.AddForce(firePoint.forward * sliderFuerza.value, ForceMode.Impulse);

        Projectile proyectil = bala.GetComponent<Projectile>();
        if (proyectil != null)
        {
            proyectil.Inicializar(CalcularVelocidadSalida(), firePoint.forward);
        }
    }
}