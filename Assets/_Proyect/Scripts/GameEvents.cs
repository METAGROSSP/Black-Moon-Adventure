using System;

public static class GameEvents
{
    // Eventos de Navegación y UI / Menús
    public static Action OnSolicitarOpciones; // <--- Añadir esta línea

    // Eventos de Vida y Estado
    public static Action<int, int> OnVidaCambiada; // (vidaActual, vidaMaxima)
    public static Action OnJugadorMuere;

    // Eventos de Acción / Gameplay
    public static Action OnPlayerAtaque;
    public static Action<int> OnPuntosCambiados;
}