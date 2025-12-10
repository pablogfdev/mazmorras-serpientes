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
                espada.transform.localPosition = espadaPrimaria ? new Vector3(0.75f, -0.13f, 0) : new Vector3(0.50f, -0.13f, 0);
                espada.transform.rotation =  Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                espada.transform.localPosition = espadaPrimaria ? new Vector3(-0.75f, -0.13f, 0) : new Vector3(-0.50f, -0.13f, 0);
                espada.transform.rotation =  Quaternion.Euler(0f, 0f, 180f);
            }
        }
        else
        {
            if (direccion.y > 0)
            {
                espada.transform.localPosition = espadaPrimaria ? new Vector3(0.13f, 0.75f, 0) : new Vector3(0.13f, 0.50f, 0);
                espada.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                espada.transform.localPosition = espadaPrimaria ? new Vector3(-0.13f, -0.75f, 0) : new Vector3(-0.13f, -0.50f, 0);
                espada.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
        }
    }
}