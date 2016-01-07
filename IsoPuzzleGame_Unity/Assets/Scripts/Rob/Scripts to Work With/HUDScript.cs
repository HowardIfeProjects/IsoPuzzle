using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

    [SerializeField]
    Image _imageOne;

    [SerializeField]
    Image _imageTwo;

    [SerializeField]
    Image _imageThree;

    private int _imageOneID;
    private int _imageTwoID;
    private int _imageThreeID;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddItemImage(Sprite spr, int i)// need to pass the sprite found in the inventory to the 
    {
        if (_imageOne.sprite == null)//if there is no sprite found in the image component...
        {
            _imageOne.sprite = spr;//add image here 
            _imageOneID = i;//assigns the id from the picked up item to the image
        }
        else if (_imageTwo.sprite == null)//if there is no sprite found in the image component...
        {
            _imageTwo.sprite = spr;//add image here 
            _imageTwoID = i;
        }
        else if (_imageThree.sprite == null)//if there is no sprite found in the image component...
        {
            _imageThree.sprite = spr;//add image here 
            _imageThreeID = i;
        }
    }

    public static void Test()
    { 
}

    public void RemoveItemImage(int i)
    {
        if(i == _imageOneID)
        {
            _imageOne.sprite = null;
            _imageOneID = -1;//to make sure that the libary doesn't assign anything as 0
        }
        else if (i == _imageTwoID)
        {
            _imageTwo.sprite = null;
            _imageTwoID = -1;//to make sure that the libary doesn't assign anything as 0
        }
        else if (i == _imageThreeID)
        {
            _imageThree.sprite = null;
            _imageThreeID = -1;//to make sure that the libary doesn't assign anything as 0
        }
    }
}
