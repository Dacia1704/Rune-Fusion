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
                registerButton.onClick.AddListener(() => UIMainMenuManager.Instance.ChangeToRegisterScreen());
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
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                        if (request.downloadHandler.text == "Dont Exist")
                        {
                                Debug.Log("Dont exist this account");
                                SetAlertText("This username dont exits",2);
                                loginButton.interactable = true;
                        }
                        else if(request.downloadHandler.text == "Wrong Username/Password")
                        {
                                Debug.Log("Login Failed");
                                SetAlertText("Wrong username or password",3);
                                loginButton.interactable = true;
                        }
                        else
                        {
                                Debug.Log(request.downloadHandler.text);
                                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                                SetAlertText($"{returnedAccount._id} Login Successful. Welcome {returnedAccount.username}",1);
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
        
}