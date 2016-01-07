using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SetItemProperties
{
    //don't get the _ part 
    public int itemID;
    public Sprite itemImage;
    public string idDescription;
}

public class InventorySystemManager : MonoBehaviour {

    //really need to get my head around this 
    //li_ short for libary, sounds better than dictornary
    public Dictionary<int,  SetItemProperties> li_inventoryHolder = new Dictionary<int, SetItemProperties>();

    public delegate void E_AddItem(int i, string s, Sprite spr);
    public static event E_AddItem OnAddItem;

    public delegate SetItemProperties E_CheckItem(int i);
    public static event E_CheckItem OnCheckItem;

    public delegate void E_UseItem(int i);
    public static event E_UseItem OnUseItem;

    /*Linked to "AddImage" method*/
    //UserInterface ui_UserInterface = GameObject.Find("Canvas").GetComponent<UserInterface>();

    void Update()
    {
        /*
        foreach(int id in li_inventoryHolder.Keys)
        {
            Debug.Log("Does this work? " + id);
        }*/

        /*Create a new KeyValuePair var that carries an int and sources SetItemProperties class.
        It will then run this function for how many objects are in the dictonary*/
        foreach(KeyValuePair<int, SetItemProperties> pair in li_inventoryHolder)
        {
            Debug.Log("This is " + pair);
        }

    }

    void Awake()
    {
        /*Since there is no plans to destroy this gameObject in game, there is no need to remove
        these methods from the events*/
        InventorySystemManager.OnAddItem += AddItem;
        InventorySystemManager.OnCheckItem += CheckForItem;
        InventorySystemManager.OnUseItem += UseItem;
    }

    private void AddItem(int i, string s, Sprite spr)
    {
        SetItemProperties newItem = new SetItemProperties();
        newItem.itemID = i;
        newItem.idDescription = s;
        newItem.itemImage = spr;
        li_inventoryHolder.Add(newItem.itemID, newItem);
        //ui_UserInterface.AddItemImage(spr);
    }

    /*added this part yet it doesn't do anything yet*/
    public void AddImage(int i , string s, Sprite spr)
    {
       // ui_UserInterface.AddItemImage(spr);
    }

    private SetItemProperties CheckForItem(int i)
    {
        if (li_inventoryHolder.ContainsKey(i))
            return li_inventoryHolder[i];
        else
            return null;
    }

    private void UseItem(int i)
    {
        li_inventoryHolder.Remove(i);
    }

    public static void CallAddItem(int i, string s, Sprite spr)
    {
        InventorySystemManager.OnAddItem(i, s, spr);
    }

    public static void CallUseItem(int i)
    {
        InventorySystemManager.OnUseItem(i);
    }

    public static SetItemProperties CallCheckForItem(int i)
    {
        return InventorySystemManager.OnCheckItem(i);
    }
}
