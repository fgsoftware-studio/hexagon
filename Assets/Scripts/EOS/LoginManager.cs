using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public class UILoginMenu : MonoBehaviour
    {
        public Dropdown loginTypeDropdown;

        public Text idText;
        public ConsoleInputField idInputField;

        public Text tokenText;
        public ConsoleInputField tokenInputField;

        public Button loginButton;
        public Button logoutButton;

        public UnityEvent OnLogin;
        public UnityEvent OnLogout;

        private EventSystem system;
        private GameObject selectedGameObject;

        private LoginCredentialType loginType = LoginCredentialType.Developer;

        public static string IdGlobalCache = string.Empty;
        public static string TokenGlobalCache = string.Empty;

        private void Awake()
        {
            idInputField.InputField.onEndEdit.AddListener(CacheIdInputField);
            tokenInputField.InputField.onEndEdit.AddListener(CacheTokenField);

            loginType = LoginCredentialType.AccountPortal;
        }

        private static void CacheIdInputField(string value)
        {
            IdGlobalCache = value;
        }

        private void CacheTokenField(string value)
        {
            TokenGlobalCache = value;
        }

        public void OnDropdownChange(int value)
        {
            switch (value)
            {
                case 1:
                    loginType = LoginCredentialType.AccountPortal;
                    ConfigureUIForAccountPortalLogin();
                    break;
                case 2:
                    loginType = LoginCredentialType.PersistentAuth;
                    ConfigureUIForPersistentLogin();
                    break;
                default:
                    loginType = LoginCredentialType.Developer;
                    ConfigureUIForDevAuthLogin();
                    break;
            }
        }

        public void Start()
        {
            ConfigureUIForLogin();

            system = EventSystem.current;

            tokenInputField.InputField.onEndEdit.AddListener(EnterPressedToLogin);
        }

        private void EnterPressedToLogin(string arg0)
        {
            OnLoginButtonClick();
        }

#if ENABLE_INPUT_SYSTEM
        public void Update()
        {
            var keyboard = Keyboard.current;

            // Disable game input if Overlay is visible and has exclusive input
            if (EventSystem.current != null && EventSystem.current.sendNavigationEvents != !EOSManager.Instance.IsOverlayOpenWithExclusiveInput())
            {
                if (EOSManager.Instance.IsOverlayOpenWithExclusiveInput())
                {
                    Debug.LogWarning("UILoginMenu (Update): Game Input (sendNavigationEvents) Disabled.");
                    EventSystem.current.sendNavigationEvents = false;
                    return;
                }
                else
                {
                    Debug.Log("UILoginMenu (Update): Game Input (sendNavigationEvents) Enabled.");
                    EventSystem.current.sendNavigationEvents = true;
                }
            }

            // Tab between input fields
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame
                && system.currentSelectedGameObject != null)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

                if (next != null)
                {
                    InputField inputfield = next.GetComponent<InputField>();
                    if (inputfield != null)
                    {
                        inputfield.OnPointerClick(new PointerEventData(system));
                        system.SetSelectedGameObject(next.gameObject);
                    }

                    ConsoleInputField consoleInputField = next.GetComponent<ConsoleInputField>();
                    if(consoleInputField != null)
                    {
                        consoleInputField.InputField.OnPointerClick(new PointerEventData(system));
                        system.SetSelectedGameObject(consoleInputField.InputField.gameObject);
                    }
                }
                else
                {
                    next = FindTopUISelectable();
                    system.SetSelectedGameObject(next.gameObject);
                }
            }

            // Controller: Detect if nothing is selected and controller input detected, and set default
            bool nothingSelected = EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null;
            bool inactiveButtonSelected =
 EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null && !EventSystem.current.currentSelectedGameObject.activeInHierarchy;

            var gamepad = Gamepad.current;
            if ((nothingSelected || inactiveButtonSelected)
                && gamepad != null && gamepad.wasUpdatedThisFrame)
            {
                if (UIFirstSelected.activeSelf == true)
                {
                    EventSystem.current.SetSelectedGameObject(UIFirstSelected);
                }
                else if (UIFindSelectable.activeSelf == true)
                {
                    EventSystem.current.SetSelectedGameObject(UIFindSelectable);
                }

                Debug.Log("Nothing currently selected, default to UIFirstSelected: EventSystem.current.currentSelectedGameObject = " + EventSystem.current.currentSelectedGameObject);
            }
        }
#else

        public void Update()
        {
            if (system.currentSelectedGameObject != null && system.currentSelectedGameObject != selectedGameObject)
                selectedGameObject = system.currentSelectedGameObject;
            else if (selectedGameObject != null && system.currentSelectedGameObject == null)
                system.SetSelectedGameObject(selectedGameObject);

            var nothingSelected = EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null;
            var inactiveButtonSelected = EventSystem.current != null &&
                                         EventSystem.current.currentSelectedGameObject != null && !EventSystem.current
                                             .currentSelectedGameObject.activeInHierarchy;

            if (!Input.GetKeyDown(KeyCode.Tab) || system.currentSelectedGameObject == null) return;
            var next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            var inputField = system.currentSelectedGameObject.GetComponent<InputField>();

            if (next != null)
            {
                var inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));

                system.SetSelectedGameObject(next.gameObject);
            }
            else
            {
                next = FindTopUISelectable();
                system.SetSelectedGameObject(next.gameObject);
            }
        }
#endif

        public Selectable FindTopUISelectable()
        {
            var currentTop = Selectable.allSelectablesArray[0];
            double currentTopXaxis = currentTop.transform.position.x;

            foreach (var s in Selectable.allSelectablesArray)
                if (s.transform.position.x > currentTopXaxis)
                {
                    currentTop = s;
                    currentTopXaxis = s.transform.position.x;
                }

            return currentTop;
        }

        private void ConfigureUIForDevAuthLogin()
        {
            loginTypeDropdown.value = loginTypeDropdown.options.FindIndex(option => option.text == "Dev Auth");

            if (!string.IsNullOrEmpty(IdGlobalCache)) idInputField.InputField.text = IdGlobalCache;

            if (!string.IsNullOrEmpty(TokenGlobalCache)) tokenInputField.InputField.text = TokenGlobalCache;

            idInputField.gameObject.SetActive(true);
            tokenInputField.gameObject.SetActive(true);
            idText.gameObject.SetActive(true);
            tokenText.gameObject.SetActive(true);

            loginTypeDropdown.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnDown = idInputField.InputFieldButton
            };

            loginButton.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = tokenInputField.InputFieldButton,
                selectOnDown = logoutButton,
                selectOnLeft = logoutButton
            };
        }

        private void ConfigureUIForAccountPortalLogin()
        {
            loginTypeDropdown.value = loginTypeDropdown.options.FindIndex(option => option.text == "Account Portal");

            idInputField.gameObject.SetActive(false);
            tokenInputField.gameObject.SetActive(false);
            idText.gameObject.SetActive(false);
            tokenText.gameObject.SetActive(false);

            loginTypeDropdown.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnDown = loginButton
            };

            loginButton.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = loginTypeDropdown,
                selectOnDown = logoutButton,
                selectOnLeft = logoutButton
            };
        }

        private void ConfigureUIForPersistentLogin()
        {
            loginTypeDropdown.value = loginTypeDropdown.options.FindIndex(option => option.text == "PersistentAuth");

            idInputField.gameObject.SetActive(false);
            tokenInputField.gameObject.SetActive(false);
            idText.gameObject.SetActive(false);
            tokenText.gameObject.SetActive(false);

            loginTypeDropdown.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnDown = loginButton
            };

            loginButton.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = loginTypeDropdown,
                selectOnDown = logoutButton,
                selectOnLeft = logoutButton
            };
        }

        private void ConfigureUIForLogin()
        {
            OnLogout?.Invoke();

            loginTypeDropdown.gameObject.SetActive(true);

            loginButton.enabled = true;
            loginButton.gameObject.SetActive(true);
            logoutButton.gameObject.SetActive(false);

            if (loginType != LoginCredentialType.AccountPortal)
            {
                if (loginType == LoginCredentialType.PersistentAuth)
                    ConfigureUIForPersistentLogin();
                else
                    ConfigureUIForDevAuthLogin();
            }
            else
            {
                ConfigureUIForAccountPortalLogin();
            }
        }

        private void ConfigureUIForLogout()
        {
            loginTypeDropdown.gameObject.SetActive(false);

            loginButton.gameObject.SetActive(false);
            logoutButton.gameObject.SetActive(true);

            idText.gameObject.SetActive(false);
            tokenText.gameObject.SetActive(false);
            idInputField.gameObject.SetActive(false);
            tokenInputField.gameObject.SetActive(false);

            OnLogin?.Invoke();
        }

        /*public void OnLogoutButtonClick()
        {
            EOSManager.Instance.StartLogout(EOSManager.Instance.GetLocalUserId(), data =>
            {
                if (data.ResultCode != Result.Success) return;
                print("Logout Successful. [" + data.ResultCode + "]");
                ConfigureUIForLogin();
            });
        }*/

        private bool SelectedLoginTypeRequiresUsername()
        {
            return loginType == LoginCredentialType.Developer;
        }

        private bool SelectedLoginTypeRequiresPassword()
        {
            return loginType == LoginCredentialType.Developer;
        }

        public void OnLoginButtonClick()
        {
            var usernameAsString = idInputField.InputField.text.Trim();
            var passwordAsString = tokenInputField.InputField.text.Trim();

            if (SelectedLoginTypeRequiresUsername() && usernameAsString.Length <= 0)
            {
                print("Username is missing.");
                return;
            }

            if (SelectedLoginTypeRequiresPassword() && passwordAsString.Length <= 0)
            {
                print("Password is missing.");
                return;
            }

            loginButton.enabled = false;
            print("Attempting to login...");

            if (loginType == LoginCredentialType.PersistentAuth)
                EOSManager.Instance.StartPersistantLogin(callbackInfo =>
                {
                    if (callbackInfo.ResultCode != Result.Success)
                    {
                        print("Failed to login with Persistent token [" + callbackInfo.ResultCode + "]");
                        ConfigureUIForDevAuthLogin();
                    }
                    else
                    {
                        StartLoginWithLoginTypeAndTokenCallback(callbackInfo);
                    }
                });
            else
                EOSManager.Instance.StartLoginWithLoginTypeAndToken(loginType,
                    usernameAsString,
                    passwordAsString,
                    StartLoginWithLoginTypeAndTokenCallback);
        }

        private void StartConnectLoginWithLoginCallbackInfo(LoginCallbackInfo loginCallbackInfo)
        {
            EOSManager.Instance.StartConnectLoginWithEpicAccount(loginCallbackInfo.LocalUserId,
                connectLoginCallbackInfo =>
                {
                    if (connectLoginCallbackInfo.ResultCode == Result.Success)
                    {
                        print("Connect Login Successful. [" + loginCallbackInfo.ResultCode + "]");
                        ConfigureUIForLogout();
                    }
                    else if (connectLoginCallbackInfo.ResultCode == Result.InvalidUser)
                    {
                        EOSManager.Instance.CreateConnectUserWithContinuanceToken(
                            connectLoginCallbackInfo.ContinuanceToken, createUserCallbackInfo =>
                            {
                                print("Creating new connect user");
                                EOSManager.Instance.StartConnectLoginWithEpicAccount(loginCallbackInfo.LocalUserId,
                                    retryConnectLoginCallbackInfo =>
                                    {
                                        if (retryConnectLoginCallbackInfo.ResultCode == Result.Success)
                                            ConfigureUIForLogout();
                                    });
                            });
                    }
                });
        }

        private void StartLoginWithLoginTypeAndTokenCallback(LoginCallbackInfo loginCallbackInfo)
        {
            if (loginCallbackInfo.ResultCode == Result.AuthMFARequired)
            {
                // collect MFA
                // do something to give the MFA to the SDK
                print("MFA Authentication not supported in sample. [" + loginCallbackInfo.ResultCode + "]");
            }
            else if (loginCallbackInfo.ResultCode == Result.AuthPinGrantCode)
            {
                Debug.LogError("------------PIN GRANT------------");
                Debug.LogError("External account is not connected to an Epic Account. Use link below");
                Debug.LogError($"URL: {loginCallbackInfo.PinGrantInfo.VerificationURI}");
                Debug.LogError($"CODE: {loginCallbackInfo.PinGrantInfo.UserCode}");
                Debug.LogError("---------------------------------");
            }
            else if (loginCallbackInfo.ResultCode == Result.Success)
            {
                StartConnectLoginWithLoginCallbackInfo(loginCallbackInfo);
            }
            else if (loginCallbackInfo.ResultCode == Result.InvalidUser)
            {
                print("Trying Auth link with external account: " + loginCallbackInfo.ContinuanceToken);
                EOSManager.Instance.AuthLinkExternalAccountWithContinuanceToken(loginCallbackInfo.ContinuanceToken,
                    LinkAccountFlags.NoFlags,
                    linkAccountCallbackInfo => { StartConnectLoginWithLoginCallbackInfo(loginCallbackInfo); });
            }
            else
            {
                print("Error logging in. [" + loginCallbackInfo.ResultCode + "]");
                ConfigureUIForLogin();
            }
        }

        public void OnApplicationQuit()
        {
            if (loginType == LoginCredentialType.Developer || loginType == LoginCredentialType.AccountPortal)
                EOSManager.Instance.StartLogout(EOSManager.Instance.GetLocalUserId(), data =>
                {
                    if (data.ResultCode != Result.Success) return;
                    print("Logout Successful. [" + data.ResultCode + "]");
                    ConfigureUIForLogin();
                });
        }
    }
}