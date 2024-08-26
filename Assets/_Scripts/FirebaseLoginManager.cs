using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLoginManager : MonoBehaviour
{
    //Đăng ký
    [Header("Register")]
    public InputField ipRegisterEmail;
    public InputField ipRegisterPassword;

    public Button buttonRegister;

    //Đăng nhập
    [Header("Sign In")]
    public InputField ipLogin;
    public InputField ipLoginPassword;

    public Button buttonLogin;
 
    //Firebase Authentication --> Đăng ký, đăng nhập
    private FirebaseAuth auth;

    //Chuyển đổi qua lại giữa đăng ký và đăng nhập
    [Header("Switch form")]
    public Button moveToSignIn;
    public Button moveToRegister;

    public GameObject loginForm;
    public GameObject registerForm;

    //Upload data người chơi lên firebase khi đăng ký 
    private FirebaseDatabaseManager databaseManager;


    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        buttonRegister.onClick.AddListener(RegisterAccountWithFirebase);
        buttonLogin.onClick.AddListener(SignInAccountWithFirebase);

        moveToSignIn.onClick.AddListener(SwitchForm);
        moveToRegister.onClick.AddListener(SwitchForm);

        databaseManager = GetComponent<FirebaseDatabaseManager>();
    }

    public void RegisterAccountWithFirebase()
    {
        string email = ipRegisterEmail.text;
        string password = ipRegisterPassword.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Dang ky bi huy!");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.Log("Dang ky that bai!");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("Dang ky thanh cong!");

                Map mapInGame = new Map();
                User userInGame = new User("", 100, 50, mapInGame);

                FirebaseUser firebaseUser = task.Result.User;
                databaseManager.WriteDatabase("Users/" + firebaseUser.UserId, userInGame.ToString());

                LoadingManager.NEXT_SCENE = "_PlayScene";
                SceneManager.LoadScene("LoadingScene");
            }
        });
    }

    public void SignInAccountWithFirebase()
    {
        string email = ipLogin.text;
        string password = ipLoginPassword.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Dang nhap bi huy!");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Dang nhap that bai!");
            }
            if (task.IsCompleted)
            {
                Debug.Log("Dang nhap thanh cong!");
                FirebaseUser user = task.Result.User;

                LoadingManager.NEXT_SCENE = "_PlayScene";
                SceneManager.LoadScene("LoadingScene");
            }
        });
    }

    public void SwitchForm()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }
}
