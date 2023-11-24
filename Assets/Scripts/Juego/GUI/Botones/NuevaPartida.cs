using IsaacBongos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuevaPartida : MonoBehaviour
{
    private Button miBoton;
    // Start is called before the first frame update
    void Start()
    {
        miBoton = GetComponent<Button>();
        miBoton.onClick.RemoveAllListeners();
        miBoton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.Instance.IniciarJuego();
    }
}
