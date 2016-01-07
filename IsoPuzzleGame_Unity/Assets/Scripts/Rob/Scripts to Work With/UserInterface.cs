using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    [SerializeField]
    GameObject theInventorySystem;
    InventorySystemManager theInventory;

    private Canvas m_UICanvas;

    public Image imageOne;
    public Image imageTwo;
    public Image imageThree;

    void OnAwake()
    {
        if(theInventorySystem.GetComponent<InventorySystemManager>())
        {
            theInventory = theInventorySystem.GetComponent<InventorySystemManager>();
        }
        //InventorySystemManager.OnAddItem += AddItemImage;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Image newImage = (Image)Instantiate(Resources.Load("ImageOne"));
            Debug.Log("This works");


        }
    }

    public void AddItemImage (Sprite spr)
    {

        /*
        if (imageOne == null)
            imageOne.sprite = spr;
        else if (imageTwo == null)
            imageTwo.sprite = spr;
        else if (imageThree == null)
            imageThree.sprite = spr;

        Debug.Log("This is running ");  */

        
    }
}
