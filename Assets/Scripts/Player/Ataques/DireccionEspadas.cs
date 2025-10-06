using UnityEngine;

public class DireccionEspadas : MonoBehaviour
{
    private GameObject espada;
    private bool espadaPrimaria;

    void Awake()
    {
        espada = gameObject;
        espadaPrimaria = gameObject.name == "EspadaPrimaria";
    }

    void OnEnable() { CalcularDireccion(); }

    void CalcularDireccion()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direccion = (mouseWorldPos - transform.parent.position).normalized;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            if (direccion.x > 0)
            {
                espada.transform.localPosition = new Vector3(1, 0, 0);
                espada.transform.rotation = espadaPrimaria ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                espada.transform.localPosition = new Vector3(-1, 0, 0);
                espada.transform.rotation = espadaPrimaria ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.Euler(0f, 0f, 30f);
            }
        }
        else
        {
            if (direccion.y > 0)
            {
                espada.transform.localPosition = new Vector3(0, 1, 0);
                espada.transform.rotation = espadaPrimaria ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.Euler(0f, 0f, 60f);
            }
            else
            {
                espada.transform.localPosition = new Vector3(0, -1, 0);
                espada.transform.rotation = espadaPrimaria ? Quaternion.Euler(0f, 0f, -90f) : Quaternion.Euler(0f, 0f, 120f);
            }
        }
    }
}