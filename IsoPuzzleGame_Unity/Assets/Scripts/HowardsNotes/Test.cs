using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Item{//you can put classes into classes 

    public int _ID;
    public Sprite _Image;
    public string _ItemDescription;
}

public class Test : MonoBehaviour {

    //inventory holder
    public Dictionary<int, Item> li_Inventory = new Dictionary< int, Item>();


    //events

    /*delegent, a method that you can add other methods to then run all their function within each method at once
    
        EXAMPLE 

        delegent void Example();

        void Start()
        {
            Example += RunA;//Adds the method 'RunA' to the delegent 'Example'
            Example += RunB;//same as above 
        }

        void RunA()
        {
            Debug.Log("This will run method A");
        }

          void RunB()
        {
            Debug.Log("This will run method B");
        }

        *OUTCOME*

        Log: This will run method A
        Log: This will run method B
        */

    public delegate void E_AddItem(int i, string s);
    public static event E_AddItem OnAddItem;

    public delegate Item E_CheckItem(int i);
    public static event E_CheckItem OnCheckItem;

    public delegate void E_UseItem(int i);
    public static event E_UseItem OnUseItem;

    private void Awake()
    {
        Test.OnAddItem += AddItem;
        Test.OnCheckItem += CheckForItem;
        Test.OnUseItem += UseItem;
    }

    private void AddItem(int ID, string s)
    {
        Item _item = new Item();
        _item._ID = ID;
        _item._ItemDescription = s;
        li_Inventory.Add(_item._ID, _item);
    }

    private Item CheckForItem(int ID)
    {
        if (li_Inventory.ContainsKey(ID))
            return li_Inventory[ID];
        else
            return null;
    }

    private void UseItem(int ID)//passes the value from 'theItemID' into this method
    {
        li_Inventory.Remove(ID);//removes the item from the libary
    }


    public static void CallAddItem(int i, string s)
    {
        Test.OnAddItem(i, s);//similar to callAddItem
        //Debug.Log("Item_ID is: " + i + "Description is: " + s);
    }

    public static void CallUseItem(int i)
    {
        Test.OnUseItem(i);
    }

    public static Item CallCheckForItem(int i)
    {
        return Test.OnCheckItem(i);
    }
}
