using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuiNavigation : MonoBehaviour
{
    [SerializeField]
    private Transform ParentBackPack;
    [SerializeField]
    private Transform ParentShop;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetFirstELementBackPack()
    {
        if(ParentBackPack.childCount > 0)
            EventSystem.current.SetSelectedGameObject(ParentBackPack.GetChild(0).gameObject);
    }
    public void SetFirstELementShop(bool tinedaShow)
    {
        print("tiendaShow");
        if(tinedaShow)
            if (ParentBackPack.childCount > 0)
                EventSystem.current.SetSelectedGameObject(ParentShop.GetChild(0).gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
