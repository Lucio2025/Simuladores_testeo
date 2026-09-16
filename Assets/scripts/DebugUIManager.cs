using UnityEngine;
using TMPro;

public class DebugUIManager : MonoBehaviour
{
    public static DebugUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textoDatos;

    private void Awake()
    {
        // Singleton simple: solo nos interesa tener una única referencia accesible
        Instance = this;
    }

    public void MostrarDatosDeImpacto(
        string objetoGolpeado,
        float tiempoDeVuelo,
        float velocidadSalida,
        float velocidadImpacto,
        float angulo,
        float distanciaReal,
        float distanciaLineaRecta,
        Vector3 puntoDeImpacto)
    {
        textoDatos.text =
            $"<b>ÚLTIMO IMPACTO</b>\n" +
            $"Objeto: {objetoGolpeado}\n" +
            $"Tiempo de vuelo: {tiempoDeVuelo:F2} s\n" +
            $"Velocidad de salida: {velocidadSalida:F2} u/s\n" +
            $"Velocidad al impactar: {velocidadImpacto:F2} u/s\n" +
            $"Ángulo de lanzamiento: {angulo:F1}°\n" +
            $"Distancia real: {distanciaReal:F2} u\n" +
            $"Distancia en línea recta: {distanciaLineaRecta:F2} u\n" +
            $"Punto: {puntoDeImpacto}";
    }
}