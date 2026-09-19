using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("Configuración de la pared")]
    [SerializeField] private GameObject cajaPrefab;
    [SerializeField] private Transform contenedor;
    [SerializeField] private Transform anclaBase;
    [SerializeField] private int filas = 3;
    [SerializeField] private int columnas = 4;
    [SerializeField] private float espaciado = 1.05f;
    [Tooltip("Fuerza necesaria para romper la unión entre cajas. Más alto = pared más resistente.")]
    [SerializeField] private float fuerzaRuptura = 500f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI etiquetaDerribadas;

    private readonly List<TargetPiece> piezas = new List<TargetPiece>();
    private int piezasDerribadas = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerarPared();
    }

    // Este método es el que va a llamar el botón "Regenerar Cajas"
    public void GenerarPared()
    {
        // Borramos la pared anterior, si existe
        foreach (Transform hijo in contenedor)
        {
            Destroy(hijo.gameObject);
        }
        piezas.Clear();
        piezasDerribadas = 0;

        for (int fila = 0; fila < filas; fila++)
        {
            for (int columna = 0; columna < columnas; columna++)
            {
                Vector3 posicionLocal = new Vector3(columna * espaciado, fila * espaciado, 0f);
                Vector3 posicionMundo = contenedor.TransformPoint(posicionLocal);

                GameObject caja = Instantiate(cajaPrefab, posicionMundo, contenedor.rotation, contenedor);
                TargetPiece pieza = caja.GetComponent<TargetPiece>();
                piezas.Add(pieza);

                Rigidbody rbActual = caja.GetComponent<Rigidbody>();
                int indiceActual = piezas.Count - 1;

                if (fila == 0)
                {
                    // Fila de abajo: se ancla al objeto fijo (AnclaBase)
                    FixedJoint jointBase = caja.AddComponent<FixedJoint>();
                    jointBase.connectedBody = anclaBase.GetComponent<Rigidbody>();
                    jointBase.breakForce = fuerzaRuptura;
                    jointBase.breakTorque = fuerzaRuptura;
                }
                else
                {
                    // Filas de arriba: se conectan a la caja de abajo suyo
                    int indiceAbajo = indiceActual - columnas;
                    Rigidbody rbAbajo = piezas[indiceAbajo].GetComponent<Rigidbody>();

                    FixedJoint joint = caja.AddComponent<FixedJoint>();
                    joint.connectedBody = rbAbajo;
                    joint.breakForce = fuerzaRuptura;
                    joint.breakTorque = fuerzaRuptura;
                }
            }
        }

        ActualizarEtiqueta();
    }

    public void NotificarPiezaDerribada(TargetPiece pieza)
    {
        piezasDerribadas++;
        ActualizarEtiqueta();
    }

    private void ActualizarEtiqueta()
    {
        if (etiquetaDerribadas != null)
        {
            etiquetaDerribadas.text = $"Cajas derribadas: {piezasDerribadas} / {piezas.Count}";
        }
    }

    public int ObtenerPiezasDerribadas() => piezasDerribadas;
    public int ObtenerTotalPiezas() => piezas.Count;
}