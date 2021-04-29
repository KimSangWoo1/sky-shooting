using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
public class GoogleGameLoginManager : Singleton<GoogleGameLoginManager>
{
    public Text Firebase_Text;
    private string authCode;

    //현재 Firebase를 구동시킬 수 있는지
    public bool IsFirebaseReady { get; private set; }

    public Button signInButton;

    //static으로 한 이유는 임시적으로 다른곳에서 직접 접근 가능하게 하려고 원래는 SingleTon으로 해줘야함
    public static FirebaseApp firebaseApp; // firebas Application을 관리
    public static FirebaseAuth firebaseAuth; // firebas Application을 중 Auth를 관리
    public static FirebaseUser User;

    private void Awake()
    {

    }
    void Start()
    {
        signInButton.interactable = false;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                                                   .RequestServerAuthCode(false /* Don't force refresh */)
                                                   .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        IsFirebaseReady = true;
        signInButton.interactable = IsFirebaseReady;
        Firebase_Text.text = "Firebase Ready";
        OnLogin();
    }

    public void OnLogin()
    {
        if (IsFirebaseReady)
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    Firebase_Text.text = Social.localUser.userName;
                }
                else
                {
                    Firebase_Text.text = "Fail";
                }
            });

            firebaseAuth = FirebaseAuth.DefaultInstance;
            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
            firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    Firebase_Text.text = "Canceled";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    Firebase_Text.text = "Faulted";
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                Firebase_Text.text = "new" + newUser.DisplayName;
            });

            User = firebaseAuth.CurrentUser;
            if (User != null)
            {
                string playerName = User.DisplayName;

                // The user's Id, unique to the Firebase project.
                // Do NOT use this value to authenticate with your backend server, if you
                // have one; use User.TokenAsync() instead.
                string uid = User.UserId;

                Firebase_Text.text = playerName + uid;
            }
        }
        else
        {
            Firebase_Text.text = "not Ready";
        }

    }
    public void OnLogout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        firebaseAuth.SignOut();
        Firebase_Text.text = "Logout";
    }
}
