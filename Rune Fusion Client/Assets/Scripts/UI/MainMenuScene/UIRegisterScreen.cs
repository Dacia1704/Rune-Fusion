using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIRegisterScreen : UIBase
{
        [field: SerializeField] private TMP_InputField usernameInputField;
        [field: SerializeField] private TMP_InputField passwordInputField;
        [field: SerializeField] private TextMeshProUGUI registerAlertText;
        [field: SerializeField] private Button registerButton;
        [field: SerializeField] private Button loginButton;
        private string authenticationEndPoint = "http://localhost:3000/register";
        
        private void Start()
        {
                registerAlertText.gameObject.SetActive(false);
                registerButton.onClick.AddListener(() =>
                {
                        LoginData loginData = CheckInvalidInput();
                        if (loginData != null)
                        {
                                StartCoroutine(TryRegister(loginData));
                        }
                        else
                        {
                                SetAlertText("Invalid username or password");
                        }
                });
                loginButton.onClick.AddListener(()=> UIMainMenuManager.Instance.ChangeToLoginScreen());
        }

        private void OnEnable()
        {
                registerButton.interactable = true;
                registerAlertText.color = Color.clear;
        }

        private LoginData CheckInvalidInput()
        {
                if (usernameInputField.text != "" && passwordInputField.text != "")
                {
                        return new LoginData(usernameInputField.text, passwordInputField.text);
                }
                return null;
        }
        
        
        private IEnumerator TryRegister(LoginData loginData)
        {
                registerButton.interactable = false;
                WWWForm registerForm = new WWWForm();
                registerForm.AddField("rUsername", loginData.Username);
                registerForm.AddField("rPassword", loginData.Password);
                Debug.Log(loginData);
                UnityWebRequest request = UnityWebRequest.Post(authenticationEndPoint,registerForm);
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
                        if (request.responseCode== 200)
                        {
                                Debug.Log(request.downloadHandler.text);
                                CreateAccountResponse response = JsonUtility.FromJson<CreateAccountResponse>(request.downloadHandler.text);
                                SetAlertText($"{response.user._id} Register Successful. Welcome {response.user.username}",1);
                        }
                        else if(request.responseCode== 401)
                        {
                                Debug.Log("Already exist this account");
                                ErrorResponse response = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                                SetAlertText(response.error,2);
                                registerButton.interactable = true;
                        }
                }
                else
                {
                        Debug.Log("Unable to connect to server...");
                        SetAlertText("Unnable to connect to the server....");
                        registerButton.interactable = true;
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
                registerAlertText.gameObject.SetActive(true);
                switch (typeAlert)
                {
                        case 1: registerAlertText.color = Color.green; break;
                        case 2: registerAlertText.color = Color.yellow; break;
                        case 3: registerAlertText.color = Color.red; break;
                }
                registerAlertText.text = text;
        }
}