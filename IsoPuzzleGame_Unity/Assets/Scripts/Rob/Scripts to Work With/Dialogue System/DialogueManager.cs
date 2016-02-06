using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.ootii.Actors;

public class DialogueManager : MonoBehaviour
{
    public GameObject _textBox;

    public Text _textName;
    public Text _theText;

    public TextAsset _textFile; //this one works
    //public string _textFile;//need to find a way to break it up 

    public string[] _textLines;//FIX ERROR: Error occurs when the _currentLines value excees the length in this array. This prevents the box from closing on a button required line 

    public int _currentLine;
    public int _endAtLine;

    public bool _isBoxActive;
    public bool _stopPlayerMovement;

    public GameObject btn_buttonOne;
    public GameObject btn_buttonTwo;
    public GameObject btn_buttonThree;

    private bool buttonFirstLoop = true;

    public Image thePlayerSprite;
    public Image thePlayerSpriteTwo;
    public Image thePlayerSpriteThree;
    private Color thePlayerColor;
    public Image theNPCSprite;
    private Color theNPCColor;

    public Sprite[] spr_playerSprites;
    public Sprite[] spr_npcSprites;

    private int pose = 0;//set so it can convert the thrid part of the line into an in for the image to show 
    private int poseTwo = 0;
    private int poseThree = 0;

    private int a_StartLine;
    private int a_endLine;
    private int b_startLine;
    private int b_endLine;
    private int c_startLine;
    private int c_endLine;

    Button test;

    public GameObject _player;//change to whatever Howard names the controller to

    void Awake()
    {
        DisableSprites();

        DisableButtons();

        //btn_buttonThree.GetComponentInChildren<Text>().text = "Changed the text!";
    }

	void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if(_textFile != null)
        {
            /*will split the lines everytime the text file has a break 
            therefor creating a value in the array*/
            _textLines = (_textFile.text.Split('\n'));
           // _textLines = (_textFile.Split('\n'));//string version 
        }

        if(_endAtLine == 0)
        {
            /*If there is no value set in endAtLine then the script 
            will automatically set a vaule base on the length of the array.
            As the array reads 0,1,2... the number set has to be deducted by
            one to make sure that there are no blank spaces after all the 
            lines have been read*/
            _endAtLine = _textLines.Length - 1;
        }

        if(_isBoxActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunText();


    }

    void RunText()
    {
        if(!_isBoxActive)
        {
            return;
        }


        /*The sprite stuff may need to be organised somewhere else*/


        //Howard Change Solves Index going over array
        if (_currentLine > (_textLines.Length-1))
            return;

        /*End of the sprite organisation*/
        // _theText.text = _textLines[_currentLine];

        string thisLine = _textLines[_currentLine];//prepares the line stored in the array to be split 
        string[] _splitCurrentLine = thisLine.Split('|');//for everytime the code finds a '|' in the line, it will create a new line in this array 

        if (_textLines[_currentLine].Contains("BTN"))
        {
            Debug.Log("This works! You should focus on buttons!");

            EnableButtons();

            btn_buttonOne.GetComponentInChildren<Text>().text = _splitCurrentLine[0];
            btn_buttonTwo.GetComponentInChildren<Text>().text = _splitCurrentLine[1];
            btn_buttonThree.GetComponentInChildren<Text>().text = _splitCurrentLine[2];

            _textName.text = "";
            _theText.text = "";

            if(_splitCurrentLine.Length == 13)
            {
                a_StartLine = int.Parse(_splitCurrentLine[4]);
                a_endLine = int.Parse(_splitCurrentLine[5]);
                b_startLine = int.Parse(_splitCurrentLine[6]);
                b_endLine = int.Parse(_splitCurrentLine[7]);
                c_startLine = int.Parse(_splitCurrentLine[8]);
                c_endLine = int.Parse(_splitCurrentLine[9]);
                pose = int.Parse(_splitCurrentLine[10]);
                thePlayerSprite.sprite = spr_playerSprites[pose];
                poseTwo = int.Parse(_splitCurrentLine[11]);
                thePlayerSpriteTwo.sprite = spr_playerSprites[poseTwo];
                poseThree = int.Parse(_splitCurrentLine[12]);
                thePlayerSpriteThree.sprite = spr_playerSprites[poseThree];
            }

            /*remove when needed, this need to be a btn click
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //moves to the next value down in the array
                _currentLine++;
                Debug.Log("This line is: " + _currentLine + ", you need to reach " + _endAtLine);
            }*/
        }
        else
        {
            EnableSprites(); 
            if(spr_npcSprites.Length == 0)
            {
                theNPCColor.a = 0;
                theNPCSprite.color = theNPCColor;
            }
            _textName.text = _splitCurrentLine[0];//sets the first component in the array as the speaker's name 
        _theText.text = _splitCurrentLine[1];//sets the second component in the array as what the speaker is saying 

        if (_splitCurrentLine.Length >= 3)//checks if there are three components in the current array before running, if this statement is not included then the script will stop working
        {
            pose = int.Parse(_splitCurrentLine[2]);//converts the thrid part of the array into an int
                if(_textLines[_currentLine].Contains("PLRSPR"))
                {
                thePlayerSprite.sprite = spr_playerSprites[pose];
                }
                else
                {
                    theNPCSprite.sprite = spr_npcSprites[pose];
                }
            Debug.Log(pose);//this is where the image will be set 
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //moves to the next value down in the array
            _currentLine++;
            Debug.Log("This line is: " + _currentLine + ", you need to reach " + _endAtLine);
        }
        }

        if (_currentLine > _endAtLine)
        {
            Debug.Log("You have reached this state!");
            spr_npcSprites = null;
            DisableSprites();
            DisableTextBox();
            //sort out error which appears, haven't read it but my guesses are that it's trying to read a line value that isn't there 
        }
         

    }

    public void ChosenAnswer(int choice)//choice is the value that was assigned to the button 
    {
        /*The problem is that the code will run all the code until if finds the right case?*/
        switch(choice)
        {
            case 1:
                Debug.Log("You chose option A!");
                SetNewStartLine(a_StartLine);
                SetNewEndLine(a_endLine);
                //set the new start posistion for the dialogue 
                //set the new end posistion for the dialogue, need to be vars that can be passed through along with the button choice 
                thePlayerSprite.sprite = spr_playerSprites[pose];
                break;
            case 2:
                Debug.Log("You chose option B!");
                SetNewStartLine(b_startLine);
                SetNewEndLine(b_endLine);
                thePlayerSprite.sprite = spr_playerSprites[poseTwo];
                break;
            case 3:
                Debug.Log("You chose option C!");
                SetNewStartLine(c_startLine);
                SetNewEndLine(c_endLine);
                thePlayerSprite.sprite = spr_playerSprites[poseThree];
                break;
        }

        /*
        if(choice == 1)
        {
            Debug.Log("You chose option A!");
            SetNewStartLine(a_StartLine);
            SetNewEndLine(a_endLine);
        }
        else if(choice == 2)
        {
            Debug.Log("You chose option B!");
            SetNewStartLine(b_startLine);
            SetNewEndLine(b_endLine);
        }
        else if(choice == 3)
        {
            Debug.Log("You chose option C!");
            SetNewStartLine(c_startLine);
            SetNewEndLine(c_endLine);
        }
        else
        {
            Debug.Log("ERROR: Something went wrong!");
        }*/
    }

    public void SetNewStartLine(int i)
    {
         _currentLine = i;
        Debug.Log("Start new line is: " + i);
        DisableButtons();
        //_currentLine++;//change to line above 
    }

    public void SetNewEndLine(int i)
    {
        _endAtLine = i;
        Debug.Log("End new line is: " + i);
    }

    public void EnableTextBox()
    {
        _textBox.SetActive(true);
        if(_stopPlayerMovement)
        {

            ActorDriver.IsTalking = true;
            //fix tomorrow
         //   _player._playerCanMove = false;
        }
       // _player._playerCanMove = false;//remove once code above works 
    }

    void DisableTextBox()
    {
        _textBox.SetActive(false);
        // _player._playerCanMove = true;

        ActorDriver.IsTalking = false;
    }

    void EnableButtons()
    {
       

        btn_buttonOne.SetActive(true);
        btn_buttonTwo.SetActive(true);
        btn_buttonThree.SetActive(true);

      

        if(buttonFirstLoop)
        {
            thePlayerColor.a = 0.1f;
           /* thePlayerColor.r = 0.3f;
            thePlayerColor.g = 0.4f;
            thePlayerColor.b = 0.6f;*/
            thePlayerSprite.color = thePlayerColor;
            thePlayerSpriteTwo.color = thePlayerColor;
            thePlayerSpriteThree.color = thePlayerColor;
            Debug.Log("This is looping");
            buttonFirstLoop = false;
        }


    }

    void DisableButtons()
    {
        btn_buttonOne.SetActive(false);
        btn_buttonTwo.SetActive(false);
        btn_buttonThree.SetActive(false);

        Color disableSpirtes = thePlayerSprite.color;
        disableSpirtes.a = 0;
        thePlayerSpriteTwo.color = disableSpirtes;
        thePlayerSpriteThree.color = disableSpirtes;
        buttonFirstLoop = true;
    }

    void EnableSprites()
    {
        thePlayerColor = thePlayerSprite.color;
        thePlayerColor.a = 1;
        thePlayerSprite.color = thePlayerColor;

        theNPCColor = theNPCSprite.color;
        theNPCColor.a = 1;
        theNPCSprite.color = theNPCColor;

    }

    void DisableSprites()
    {
        thePlayerColor = thePlayerSprite.color;//sets the colour in this var to the same colour found in the sprite
        thePlayerColor.a = 0;//sets the alpha colour to 0
        thePlayerSprite.color = thePlayerColor;//overrides the color value found in the sprite 

        theNPCColor = theNPCSprite.color;
        theNPCColor.a = 0;
        theNPCSprite.color = theNPCColor;

        thePlayerSpriteTwo.color = thePlayerColor;
        thePlayerSpriteThree.color = thePlayerColor;
    }

    public void ReloadScript(TextAsset theText)
    {
        if(theText != null)
        {
            //overwrites the currents values in the array, not sure why it starts at 1
            _textLines = new string[1];
            _textLines = (theText.text.Split('\n'));
        }
    }

    public void TestOver(string text)
    {
        Debug.Log(text);
    }

    public  void OverButton(Image img)
    {
        Color spriteAlpha = img.color;
        spriteAlpha.a = 1;
        img.color = spriteAlpha;
    }

    public void ExitButton(Image img)
    {
        Color spriteAlpha = img.color;
        spriteAlpha.a = 0.1f;
        img.color = spriteAlpha;
    }
}
