using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Util;
using TMPro;
using UnityEngine.Networking;

namespace KSH_Lib.UI
{
	public class AccountUI_Manager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		[Header("UI Init")]
		public GameObject IdFieldObj;
		public GameObject PasswordFieldObj;
		public GameObject NicknameFieldObj;
		public GameObject LoginButtonObj;
		public GameObject RegisterButtonObj;
		public GameObject BackButtonObj;

		public GameObject ErrorMsgObj;

		[Header("PopUp Time")]
		public float FadeInTime = 0.2f;
		public float WaitTime = 0.5f;
		public float FadeOutTime = 0.2f;

		[Header("Error Message Setting")]
		[SerializeField]
		string noLoginFieldStr = "No Id Input";
		[SerializeField]
		string noPasswordFieldStr = "No Password Input";
		[SerializeField]
		string noNickNameFieldStr = "No NickName Input";

		/*--- Private Fields ---*/
		enum UI_State { Login, Register}
		UI_State curState;

		string id;
		string password;
		string nickname;

		TMP_InputField idField;
		TMP_InputField passwordField;
		TMP_InputField nicknameField;


		CanvasGroup errorMsgCanvasGroup;
		TextMeshProUGUI errorMsgText;
		UI_Effect uiEffect;

		bool isResponsed = false;

		/*--- MonoBehaviour Callbacks ---*/
		private void Awake()
		{
			idField = IdFieldObj.GetComponent<TMP_InputField>();
			passwordField = PasswordFieldObj.GetComponent<TMP_InputField>();
			nicknameField = NicknameFieldObj.GetComponent<TMP_InputField>();
			errorMsgCanvasGroup = ErrorMsgObj.GetComponent<CanvasGroup>();
			errorMsgText = ErrorMsgObj.GetComponent<TextMeshProUGUI>();
			if ( idField == null || passwordField == null || nicknameField == null )
			{
				Debug.LogError( "AccountService.Awake(): null Refrence" );
			}
		}
		private void Start()
		{
			ActiveLoginScreen();
			uiEffect = new UI_Effect(errorMsgCanvasGroup);
		}


        /*--- Public Methods ---*/
        public void ActiveLoginScreen()
		{
			IdFieldObj.SetActive( true );
			PasswordFieldObj.SetActive( true );
			NicknameFieldObj.SetActive( false );
			LoginButtonObj.SetActive( true );
			RegisterButtonObj.SetActive( true );
			BackButtonObj.SetActive( false );

			errorMsgCanvasGroup.alpha = 0.0f;

			ResetInputFields();

			curState = UI_State.Login;
		}
		public void ActiveRegisterScreen()
		{
			ResetInputFields();
			IdFieldObj.SetActive( true );
			PasswordFieldObj.SetActive( true );
			NicknameFieldObj.SetActive( true );
			LoginButtonObj.SetActive( false );
			RegisterButtonObj.SetActive( true );
			BackButtonObj.SetActive( true );

			curState = UI_State.Register;
		}

		public void ResetInputFields()
        {
			idField.text = null;
			passwordField.text = null;
			nicknameField.text = null;
		}

		public void OnLoginClicked()
        {
			AccountService.Instance.Login( CheckLoginField, InitLoginForm, HandleResponse );
        }
		public void OnRegisterClicked()
        {

			if(curState == UI_State.Login)
            {
				ActiveRegisterScreen();
            }
			else if(curState == UI_State.Register)
            {
				AccountService.Instance.Register( CheckRegisterField, InitRegisterForm, HandleResponse );
            }

        }
		public void OnBackClicked()
        {
			ActiveLoginScreen();
        }

		/*--- Private Methods ---*/
		bool CheckRegisterField()
		{
			if(CheckLoginField())
			{
				nickname = nicknameField.text.Trim();
				if (nickname == "")
				{
					Debug.Log("AccountService.CheckField: No nickname Input");
					PopUpErrorMsg(noNickNameFieldStr);
					return false;
				}
				return true;
			}
			return false;
		}
		bool CheckLoginField()
        {
            id = idField.text.Trim();
            password = passwordField.text.Trim();

            if ( id == "" )
            {
                Debug.Log( "AccountService.CheckField: No Id Input" );
                PopUpErrorMsg( noLoginFieldStr );
                return false;
            }
            else if ( password == "" )
            {
                Debug.Log( "AccountService.CheckField: No password Input" );
                PopUpErrorMsg( noPasswordFieldStr );
                return false;
            }
            else
            {
                return true;
            }
        }


		void HandleResponse(string response)
		{
			AccountService.AccountResponse data = JsonUtility.FromJson<AccountService.AccountResponse>( response );

			if(data.result == "ERROR")
            {
				PopUpErrorMsg(data.message);
				return;
            }

			if(data.order == "register")
            {
				if(data.result == "OK")
                {
					ActiveLoginScreen();
				}
            }
		}

		void PopUpErrorMsg(in string msg)
        {
			errorMsgText.text = msg;
			StartCoroutine(uiEffect.PopUp(FadeInTime, WaitTime, FadeOutTime));
		}

		WWWForm InitLoginForm()
        {
			WWWForm form = new WWWForm();
			form.AddField( "order", "login" );
			form.AddField( "id", id );
			form.AddField( "password", password );
			return form;
        }
		WWWForm InitRegisterForm()
		{
			WWWForm form = new WWWForm();
			form.AddField( "order", "register" );
			form.AddField( "id", id );
			form.AddField( "password", password );
			form.AddField( "nickname", nickname );
			return form;
        }
	}
}
