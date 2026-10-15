using UnityEngine; // Permite utilizar los objetos y componentes de Unity.

// Representa una carta: su pareja, su dibujo y su estado actual.
public class Carta : MonoBehaviour
{
    public SpriteRenderer dibujo; // Componente que muestra el dibujo de la carta.
    public Sprite reverso; // Imagen visible cuando la carta está oculta.
    public Sprite frente; // Imagen que identifica a la pareja de la carta.
    public int pareja; // Número de pareja: 0 = calavera, 1 = poción roja, 2 = espada, 3 = poción azul.
    public bool revelada; // Indica si actualmente podemos ver el frente.
    public bool encontrada; // Indica si su pareja ya fue completada.

    public void Preparar(int numero, Sprite imagen) // Asigna una pareja al empezar una partida.
    {
        pareja = numero; // Guarda el identificador recibido.
        frente = imagen; // Guarda el dibujo recibido.
        encontrada = false; // La carta todavía no pertenece a una pareja completada.
        dibujo.color = Color.white; // Quita el color verde de la partida anterior.
        Ocultar(); // Comienza mostrando el reverso.
    }

    public void Revelar() // Muestra el símbolo de esta carta.
    {
        revelada = true; // Marca la carta como descubierta.
        dibujo.sprite = frente; // Cambia la imagen al dibujo de su pareja.
    }

    public void Ocultar() // Vuelve a mostrar el reverso.
    {
        revelada = false; // Marca la carta como oculta.
        dibujo.sprite = reverso; // Coloca la imagen del reverso.
    }

    public void MarcarPareja() // Conserva descubierta una carta acertada.
    {
        encontrada = true; // Impide volver a seleccionar esta carta.
        dibujo.color = new Color(0.75f, 1f, 0.82f); // Le da un tono verde para indicar el acierto.
    }
}
