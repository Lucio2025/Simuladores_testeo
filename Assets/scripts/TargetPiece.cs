using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetPiece : MonoBehaviour
{
    public bool EstaDerribada { get; private set; } = false;

    [Header("Umbrales para considerar 'derribada'")]
    [Tooltip("Si la pieza rota más que esto respecto a su orientación inicial, cuenta como caída.")]
    [SerializeField] private float anguloParaContarCaida = 45f;
    [Tooltip("Si la pieza se desplaza más que esto respecto a su posición inicial, cuenta como caída.")]
    [SerializeField] private float distanciaParaContarCaida = 1.5f;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    private void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    private void Update()
    {
        if (EstaDerribada) return;

        float angulo = Quaternion.Angle(rotacionInicial, transform.rotation);
        float distancia = Vector3.Distance(posicionInicial, transform.position);

        if (angulo > anguloParaContarCaida || distancia > distanciaParaContarCaida)
        {
            MarcarComoDerribada();
        }
    }

    // Unity llama esto automáticamente cuando un Joint de este objeto se rompe
    private void OnJointBreak(float breakForce)
    {
        MarcarComoDerribada();
    }

    private void MarcarComoDerribada()
    {
        if (EstaDerribada) return;
        EstaDerribada = true;

        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.NotificarPiezaDerribada(this);
        }
    }
}