using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Nuevo Input System Package de Unity 6

public class TransicionNivel : MonoBehaviour
{
    [Header("Referencias de UI (UX)")]
    [SerializeField] private GameObject promptInteraccionUI; // Texto "[ESPACIO] Entrar"
    [SerializeField] private GameObject panelDemoTerminada; // Panel emergente con los botones

    [Header("Configuración de Escena")]
    [SerializeField] private string nombreMenuPrincipal = "MainMenu";

    [Header("Juice & Feedback (VFX & SFX)")]
    [SerializeField] private GameObject vfxVictoriaPrefab;
    [SerializeField] private AudioClip victoriaSFX;

    private bool jugadorEnRango = false;
    private bool demoTerminada = false;
    private GameObject jugadorRef;

    private void Start()
    {
        // Garantizamos que los elementos de UI inicien ocultos
        if (promptInteraccionUI != null) promptInteraccionUI.SetActive(false);
        if (panelDemoTerminada != null) panelDemoTerminada.SetActive(false);
    }

    private void Update()
    {
        // Lectura exclusiva con el nuevo Input System (Keyboard.current.spaceKey)
        if (jugadorEnRango && !demoTerminada && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ActivarFinDeDemo();
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player") && !demoTerminada)
        {
            jugadorEnRango = true;
            jugadorRef = otro.gameObject;

            // Encender el prompt flotante de [ESPACIO]
            if (promptInteraccionUI != null)
            {
                promptInteraccionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otro)
    {
        if (otro.CompareTag("Player") && !demoTerminada)
        {
            jugadorEnRango = false;
            jugadorRef = null;

            // Ocultar el prompt al alejarse
            if (promptInteraccionUI != null)
            {
                promptInteraccionUI.SetActive(false);
            }
        }
    }

    private void ActivarFinDeDemo()
    {
        demoTerminada = true;

        // Ocultar el aviso visual [ESPACIO]
        if (promptInteraccionUI != null)
        {
            promptInteraccionUI.SetActive(false);
        }

        // 1. Congelar físicas e Input del personaje en Unity 6 (linearVelocity)
        if (jugadorRef != null)
        {
            var rb = jugadorRef.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            var scriptMovimiento = jugadorRef.GetComponent<MonoBehaviour>();
            if (scriptMovimiento != null) scriptMovimiento.enabled = false;
        }

        // 2. Feedback Sensorial (Juice: Audio & VFX / Hito 2)
        if (vfxVictoriaPrefab != null)
        {
            Instantiate(vfxVictoriaPrefab, transform.position, Quaternion.identity);
        }

        if (AudioManager.Instance != null && victoriaSFX != null)
        {
            AudioManager.Instance.PlaySFX(victoriaSFX);
        }

        // 3. Desplegar el Panel UI y pausar el tiempo
        if (panelDemoTerminada != null)
        {
            panelDemoTerminada.SetActive(true);
        }

        // Regla: Se congela el tiempo al final para no cortar el disparo del audio/VFX
        Time.timeScale = 0f;
    }

    // --- MÉTODOS PÚBLICOS PARA ENLAZAR A LOS BOTONES ---

    public void BotonReiniciarNivel()
    {
        Time.timeScale = 1f; // Restaurar el tiempo siempre antes de recargar escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BotonIrAlMenu()
    {
        Time.timeScale = 1f; // Restaurar el tiempo siempre antes de ir al menú
        SceneManager.LoadScene(nombreMenuPrincipal);
    }
}