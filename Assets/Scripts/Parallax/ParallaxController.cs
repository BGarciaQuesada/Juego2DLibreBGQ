using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Transform cam;            // Cámara (coge la de player)
    public float parallaxEffect;     // Intensidad del efecto (0 = fijo, 1 = sigue la cámara)

    private float startPos;          // Posición inicial del fondo
    private float camStartX;         // Posición inicial de la cámara

    void Start()
    {
        // Guardar posiciones iniciales
        startPos = transform.position.x;
        camStartX = cam.position.x;
    }

    void Update()
    {
        // Movimiento horizontal real de la cámara
        float camDeltaX = cam.position.x - camStartX;

        // Cantidad que debe moverse la capa según el efecto de parallax
        float dist = camDeltaX * parallaxEffect;

        // Aplicar movimiento al fondo
        transform.position = new Vector3(
            startPos + dist,
            transform.position.y,
            transform.position.z
        );
    }
}
