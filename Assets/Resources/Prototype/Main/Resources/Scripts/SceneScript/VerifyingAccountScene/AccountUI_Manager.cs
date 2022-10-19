using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Util;
using TMPro;

namespace KSH_Lib.UI
{
	public class AccountUI_Manager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public GameObject IdFieldObj;
		public GameObject PasswordFieldObj;
		public GameObject NicknameFieldObj;
		public GameObject LoginButtonObj;
		public GameObject RegisterButtonObj;
		public GameObject BackButtonObj;


		/*--- Private Fields ---*/
		enum UI_State { Login, Register}
		UI_State curState;

		string id;
		string password;
		string nickname;

		TMP_InputField idField;
		TMP_InputField passwordField;
		TMP_InputField nicknameField;

		AccountService accountService;


		/*--- MonoBehaviour Callbacks ---*/
		private void Awake()
		{
			idField = IdFieldObj.GetComponent<TMP_InputField>();
			passwordField = PasswordFieldObj.GetComponent<TMP_InputField>();
			nicknameField = NicknameFieldObj.GetComponent<TMP_InputField>();
			if ( idField == null || passwordField == null || nicknameField == null )
			{
				Debug.LogError( "AccountService.Awake(): null Refrence" );
			}
		}
        private void Start()
        {
			ActiveLoginScreen();
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

			ResetInputFields();

			curState = UI_State.Login;
		}
		public void ActiveRegisterScreen()
		{
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
			passwordField = null;
			nicknameField = null;
		}

		public void OnLoginClicked()
        {
			accountService.Login( CheckField, HandleResponse );
        }
		public void OnRegisterClicked()
        {

			if(curState == UI_State.Login)
            {
				ActiveRegisterScreen();
            }
			else if(curState == UI_State.Register)
            {
				accountService.Register( CheckField, HandleResponse );
				ActiveLoginScreen();
            }

        }
		public void OnBackClicked()
        {
			ActiveLoginScreen();
        }

		/*--- Private Methods ---*/
		bool CheckField()
		{
			id = idField.text.Trim();
			password = passwordField.text.Trim();
			nickname = nicknameField.text.Trim();

			if ( id == "" )
			{
				Debug.Log( "AccountService.CheckField: No Id Input" );
				return false;
			}
			else if ( password == "" )
			{
				Debug.Log( "AccountService.CheckField: No password Input" );
				return false;
			}
			else if ( nickname == "" )
			{
				Debug.Log( "AccountService.CheckField: No nickname Input" );
				return false;
			}
			else
			{
				return true;
			}
		}


		void HandleResponse(string response)
		{
			Debug.Log( response );
			AccountService.AccountResponse data = JsonUtility.FromJson<AccountService.AccountResponse>( response );


			if(data.result == "ERROR")
            {
				Debug.Log( "Response Error" );
				return;
            }


		}

	}
}
