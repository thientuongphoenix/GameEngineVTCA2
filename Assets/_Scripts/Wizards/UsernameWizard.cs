using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameWizard : MonoBehaviour
{
    public Text username;
    public Text gold;
    public Text diamond;

    public GameObject usernameWizard;
    public InputField ipUsername;
    public Button buttonOk;

    private FirebaseDatabaseManager databaseManager;

    // Start is called before the first frame update
    void Start()
    {
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<FirebaseDatabaseManager>();

        if(LoadDataManager.userInGame.Name == "")
        {
            usernameWizard.SetActive(true);
        }
        else
        {
            username.text = LoadDataManager.userInGame.Name;
        }

        gold.text = "Gold: " + LoadDataManager.userInGame.Gold.ToString();
        diamond.text = "Diamond: " + LoadDataManager.userInGame.Diamon.ToString();

        buttonOk.onClick.AddListener(SetNewUsername);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewUsername()
    {
        if(ipUsername.text != "")
        {
            LoadDataManager.userInGame.Name = ipUsername.text;

            databaseManager.WriteDatabase("Users/" + LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());

            username.text = ipUsername.text;

            usernameWizard.SetActive(false);
        }
    }
}
