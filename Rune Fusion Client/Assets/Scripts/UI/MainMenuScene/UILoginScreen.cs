using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class UILoginScreen: UIBase
{
        [field: SerializeField] private TMP_InputField usernameInputField;
        [field: SerializeField] private TMP_InputField passwordInputField;
        [field: SerializeField] private TextMeshProUGUI loginAlertText;
        [field: SerializeField] private Button loginButton;
        [field: SerializeField] private Button registerButton;
        private string authenticationEndPoint = "http://localhost:3000/account";
        
        private void Start()
        {
                loginAlertText.gameObject.SetActive(false);
                loginButton.onClick.AddListener(() =>
                {
                        LoginData loginData = CheckInvalidInput();
                        if (loginData != null)
                        {
                                StartCoroutine(TryLogin(loginData));
                        }
                        else
                        {
                                SetAlertText("Invalid username or password");
                        }
                });
                registerButton.onClick.AddListener(() => UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance.UIRegisterScreen));
        }

        private void OnEnable()
        {
                loginButton.interactable = true;
                loginAlertText.color = Color.clear;
        }

        private LoginData CheckInvalidInput()
        {
                if (usernameInputField.text != "" && passwordInputField.text != "")
                {
                        return new LoginData(usernameInputField.text, passwordInputField.text);
                }
                return null;
        }
        
        
        private IEnumerator TryLogin(LoginData loginData)
        {
                loginButton.interactable = false;
                WWWForm loginForm = new WWWForm();
                loginForm.AddField("rUsername", loginData.Username);
                loginForm.AddField("rPassword", loginData.Password);
                UnityWebRequest request = UnityWebRequest.Post(authenticationEndPoint,loginForm);
                var handler = request.SendWebRequest();
                float startTime = 0.0f;
                while (handler.isDone == false)
                {
                        startTime += Time.deltaTime;
                        if (startTime > 10.0f)
                        {
                                break;
                        }
                        yield return null;
                }
                Debug.Log(request.responseCode);
                if (request.result == UnityWebRequest.Result.Success)
                {
                        if (request.responseCode == 200)
                        {
                                LoginResponse returnedAccount = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                                Debug.Log("Login successful. Token: "+ returnedAccount.token);
                                SetAlertText($"{returnedAccount.user._id} Login Successful. Welcome {returnedAccount.user.username}",1);
                                SocketManager.Instance.SetPlayerNetworkData(returnedAccount.user.username, returnedAccount.user._id,-1);
                                SocketManager.Instance.SetToken(returnedAccount.token);
                                UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance
                                        .UITabManager);  
                                SocketManager.Instance.SetUpConnectSocket();
                        }
                        else if(request.responseCode == 401)
                        {
                                ErrorResponse response = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                                SetAlertText(response.error,3);
                                loginButton.interactable = true;
                        }
                }
                else
                {
                        Debug.Log("Unable to connect to server...");
                        SetAlertText("Unnable to connect to the server....");
                        loginButton.interactable = true;
                }
                
                yield return null;
        }
        
        /// <summary>
        /// Set Alert Text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeAlert">1:Success,2:Warning,3:Error</param>
        private void SetAlertText(string text,int typeAlert=3)
        {
                loginAlertText.gameObject.SetActive(true);
                switch (typeAlert)
                {
                        case 1: loginAlertText.color = Color.green; break;
                        case 2: loginAlertText.color = Color.yellow; break;
                        case 3: loginAlertText.color = Color.red; break;
                }
                loginAlertText.text = text;
        }


        public void DaciaLogin()
        {
                usernameInputField.text = "Dacia";
                passwordInputField.text = "12345";
        }  
        public void SeichiLogin()
        {
                usernameInputField.text = "Seichi";
                passwordInputField.text = "12345";
        }  
}