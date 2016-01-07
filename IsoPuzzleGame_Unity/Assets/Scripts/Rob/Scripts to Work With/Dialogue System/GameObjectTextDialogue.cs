using UnityEngine;
using System.Collections;

public class GameObjectTextDialogue : MonoBehaviour
{

    public GameObject _npcUI;
    private GameObject _player;
    public Sprite[] _spritesToPassThrough;
    //public GameObject _thisGameObject;//confused why it doesn't work properly so I'm trying the lazy way round

    public DialogueManager _dialogueManager;

    public TextAsset _theText;//convert to string 
    public int _startLine;
    public int _endLine;//atm this needs to be manually set, needs to be one less than the lines of dialogue in the text example if there are 4 lines, put 3. if there are 7, put 6

    //required if _requiredComponent is true
    public TextAsset _altText;//cpnvert to string 
    public int _altStartLine = 0;
    public int _altEndLine = 0;

    //required if _rnewText is true, really need to find a new name to call this 
    public TextAsset _newTextText;//cpnvert to string 
    public int _newStartLine = 0;
    public int _newEndLine = 0;

    public bool _requiredComponent;
    public int _requestedItemNumber;
    public bool _requireButtonPress;
    public bool _waitForPress;
    public bool _destroyWhenActive;
    public bool _firstConversation;//if the original text is only needed to be repeated once, sorry for the awful naming I had no idea what to call it 

    [SerializeField]
    private GameObject inventoryManager;

    private InventorySystemManager _inventoryLibary;
    private bool firstTimeTalking = true;

    // Use this for initialization
    void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _dialogueManager = FindObjectOfType<DialogueManager>();

        if (inventoryManager.GetComponent<InventorySystemManager>())
        {
            _inventoryLibary = inventoryManager.GetComponent<InventorySystemManager>();//assigns the InventorySystemManager script assigned on the inventoryManager gameObject 

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckPlayerDistance();
       // PlayerInput();
	}

    void PlayerInput()
    {
        if(_waitForPress && Input.GetKeyDown(KeyCode.R))
        {
            PassThroughData();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.name == "Player")
        {
            if(_requireButtonPress)
            {
                _waitForPress = true;
                return;
            }

            //PassThroughData();//? should work 
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.name == "Player")
        {
            _waitForPress = false;
        }
    }

    void PassThroughData()
    {
        if(_firstConversation)
        {
            if(firstTimeTalking)
            {
                if(_requiredComponent)
                {
                    if(_inventoryLibary.li_inventoryHolder.ContainsKey(_requestedItemNumber))
                    {
                        //too lazy to add a new component so resusing it for this purpose
                        _dialogueManager.ReloadScript(_theText);
                        _dialogueManager._currentLine = _startLine;
                        _dialogueManager._endAtLine = _endLine;
                        firstTimeTalking= false;//find a way to toggle this on/off
                    }
                    else
                    {
                        //future - pop all dialogue in an array, it'll make it easier to store loads of components
                        _dialogueManager.ReloadScript(_altText);
                        _dialogueManager._currentLine = _altStartLine;
                        _dialogueManager._endAtLine = _altEndLine;
                    }
                }
                else
                {
                    _dialogueManager.ReloadScript(_theText);
                    _dialogueManager._currentLine = _startLine;
                    _dialogueManager._endAtLine = _endLine;
                    firstTimeTalking = false;//find a way to toggle this on/off
                }
            }
            else
            {
                _dialogueManager.ReloadScript(_newTextText);
                _dialogueManager._currentLine = _newStartLine;
                _dialogueManager._endAtLine = _newEndLine;
            }
        }
        else
        {
            if(_requiredComponent)
            {
                if (_inventoryLibary.li_inventoryHolder.ContainsKey(_requestedItemNumber))
                {
                    //future - pop all dialogue in an array, it'll make it easier to store loads of components
                    _dialogueManager.ReloadScript(_altText);
                    _dialogueManager._currentLine = _altStartLine;
                    _dialogueManager._endAtLine = _altEndLine;
                }
                else
                {
                    //too lazy to add a new component so resusing it for this purpose
                    _dialogueManager.ReloadScript(_theText);
                    _dialogueManager._currentLine = _startLine;
                    _dialogueManager._endAtLine = _endLine;
                }
            }
            else
            {
                _dialogueManager.ReloadScript(_theText);
                _dialogueManager._currentLine = _startLine;
                _dialogueManager._endAtLine = _endLine;
            }
        }

            _dialogueManager.EnableTextBox();
            _dialogueManager.spr_npcSprites = null;
            _dialogueManager.spr_npcSprites = _spritesToPassThrough;

            //howard change <= was causing crash on re engaging the convo
            for(int i = 0; i < _spritesToPassThrough.Length; i++)
            {
                _dialogueManager.spr_npcSprites[i] = _spritesToPassThrough[i];
            }

        if(_destroyWhenActive)//why am i destroying the gameObject? Shouldn't it just be this component?!
        {
            Destroy(gameObject);
        }
    }

    void CheckPlayerDistance() //Didn't make sense to use since it is only used for one object 
    {
        Vector3 _dir = _player.transform.position - this.transform.position;
        Debug.Log("this is working" + _dir);
        Debug.DrawRay(this.transform.position, _dir,  Color.red);
        if (Vector3.Distance(_player.transform.position, this.transform.position) <= 2.5f) //needs explaining 
        {
            Debug.Log("you are close enough!");

                PlayerInput();
            _npcUI.SetActive(true);

            /*messy, but gets the job done in the meantime!*/
            if (_requireButtonPress)
                {
                    _waitForPress = true;
                    return;
                }
        }
        else
            _npcUI.SetActive(false);
    }
}
