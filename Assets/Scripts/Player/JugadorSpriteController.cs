using UnityEngine;
using System.Collections;

public class JugadorSpriteController : MonoBehaviour
{
    private Animator animacion;
    private MovimientoJugador movimiento;
    private int ultimaDireccion = 3;
    public int UltimaDireccion => ultimaDireccion;
    private readonly float deadzone = 0.01f;
    private Coroutine ataqueCorutina = null;
    public bool EstaAtacando => ataqueCorutina != null;


    void Awake()
    {
        movimiento = GetComponent<MovimientoJugador>();
        animacion = transform.Find("Cuerpo")?.GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 mov = movimiento.Movimiento;
        bool seMueve = mov.sqrMagnitude > deadzone;

        if (ataqueCorutina != null && seMueve)
        {
            StopCoroutine(ataqueCorutina);
            ataqueCorutina = null;
            animacion.SetInteger("accion", 1);
            return;
        }

        if (ataqueCorutina != null) return;

        if (seMueve) ultimaDireccion = CalcularDireccion(mov);

        animacion.SetInteger("accion", seMueve ? 1 : 0);
        animacion.SetInteger("direccion", ultimaDireccion);
    }

    public void IniciarAtaque(int tipoAtaque)
    {
        if (movimiento.Movimiento.sqrMagnitude > deadzone) return;
        if (ataqueCorutina != null) return;
        ataqueCorutina = StartCoroutine(ReproducirAtaque(tipoAtaque));
    }

    private IEnumerator ReproducirAtaque(int tipo)
    {   
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido(tipo == 2 ? "Ataque_Lanza" : "Ataque_Latigo"), Camera.main.transform.position);

        int direccion = CalcularDireccionMouse();
        animacion.SetInteger("accion", tipo);
        animacion.SetInteger("direccion", direccion);

        string clip = ResolverClipAtaque(tipo, direccion);
        animacion.Play(clip, 0, 0f);
        animacion.speed = 1f;

        while (true)
        {
            var st = animacion.GetCurrentAnimatorStateInfo(0);
            if (!st.IsName(clip)) yield return null;
            else if (st.normalizedTime >= 1f) break;
            else yield return null;
            if (ataqueCorutina == null) yield break;
        }

        ataqueCorutina = null;
    }

    private string ResolverClipAtaque(int tipo, int dir)
    {
        string[] direccion = { "norte", "oeste", "este", "sur" };
        return (tipo == 2) ? $"{direccion[dir]}_lanza" : $"{direccion[dir]}_espada";
    }

    private int CalcularDireccion(Vector2 mov)
    {
        if (Mathf.Abs(mov.x) > Mathf.Abs(mov.y)) return mov.x > 0 ? 2 : 1;
        else return mov.y > 0 ? 0 : 3;
    }

    private int CalcularDireccionMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3 direccion = (mouseWorldPos - transform.position).normalized;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y)) return direccion.x > 0 ? 2 : 1; // Este / Oeste
        else return direccion.y > 0 ? 0 : 3; // Norte / Sur
    }
}