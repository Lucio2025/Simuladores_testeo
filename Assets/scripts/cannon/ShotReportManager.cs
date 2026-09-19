using UnityEngine;
using TMPro;
using System.Collections;

public class ShotReportManager : MonoBehaviour
{
    public static ShotReportManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panelReporte;
    [SerializeField] private TextMeshProUGUI textoReporte;

    [Header("Configuración")]
    [Tooltip("Segundos a esperar tras el impacto antes de contar piezas derribadas, para dar tiempo a que la estructura termine de derrumbarse en cadena.")]
    [SerializeField] private float esperaParaContarDerribos = 1.5f;
    [Tooltip("Puntos otorgados por cada pieza derribada en este tiro.")]
    [SerializeField] private int puntosPorPieza = 100;

    private int piezasDerribadasAlDisparar = 0;
    private bool tiroEnCurso = false;

    private void Awake()
    {
        Instance = this;
        if (panelReporte != null) panelReporte.SetActive(false);
    }

    // Llamado por CannonController justo antes de instanciar la bala
    public void IniciarSeguimientoDeTiro()
    {
        tiroEnCurso = true;
        piezasDerribadasAlDisparar = TargetManager.Instance != null
            ? TargetManager.Instance.ObtenerPiezasDerribadas()
            : 0;

        if (panelReporte != null) panelReporte.SetActive(false);
    }

    // Llamado por Projectile en su OnCollisionEnter
    public void RegistrarImpacto(string objetoGolpeado, float tiempoDeVuelo, Vector3 puntoDeImpacto,
                                  float velocidadRelativa, float impulso)
    {
        if (!tiroEnCurso) return; // evita reportes de balas "viejas" que impactan tarde
        StartCoroutine(GenerarReporteConDelay(objetoGolpeado, tiempoDeVuelo, puntoDeImpacto, velocidadRelativa, impulso));
    }

    private IEnumerator GenerarReporteConDelay(string objetoGolpeado, float tiempoDeVuelo, Vector3 puntoDeImpacto,
                                                float velocidadRelativa, float impulso)
    {
        yield return new WaitForSeconds(esperaParaContarDerribos);

        int piezasDerribadasAhora = TargetManager.Instance != null
            ? TargetManager.Instance.ObtenerPiezasDerribadas()
            : 0;
        int piezasDerribadasEsteTiro = piezasDerribadasAhora - piezasDerribadasAlDisparar;
        int puntuacion = piezasDerribadasEsteTiro * puntosPorPieza;

        MostrarReporte(objetoGolpeado, tiempoDeVuelo, puntoDeImpacto, velocidadRelativa, impulso,
                        piezasDerribadasEsteTiro, puntuacion);

        tiroEnCurso = false;
    }

    private void MostrarReporte(string objetoGolpeado, float tiempoDeVuelo, Vector3 puntoDeImpacto,
                                 float velocidadRelativa, float impulso, int piezasDerribadas, int puntuacion)
    {
        if (panelReporte != null) panelReporte.SetActive(true);

        textoReporte.text =
            $"<b>REPORTE DE TIRO</b>\n" +
            $"Objeto golpeado: {objetoGolpeado}\n" +
            $"Tiempo de vuelo: {tiempoDeVuelo:F2} s\n" +
            $"Punto de impacto: ({puntoDeImpacto.x:F1}, {puntoDeImpacto.y:F1}, {puntoDeImpacto.z:F1})\n" +
            $"Velocidad relativa: {velocidadRelativa:F2} u/s\n" +
            $"Impulso de colisión: {impulso:F2} N·s\n" +
            $"Piezas derribadas: {piezasDerribadas}\n" +
            $"<b>Puntuación: {puntuacion} pts</b>";
    }
}