using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Util;
using TMPro;
using LSH_Lib;

namespace KSH_Lib.UI
{
	public class AccountUI_Manager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		[Header("Next Scene")]
		[SerializeField]
		string NextSceneName;

		[Header("UI Init")]
		public GameObject IdFieldObj;
		public GameObject PasswordFieldObj;
		public GameObject NicknameFieldObj;
		public GameObject LoginButtonObj;
		public GameObject RegisterButtonObj;
		public GameObject RedRegisterButtonObj;
		public GameObject BackButtonObj;
		public GameObject MessageObj;
		public GameObject LoadingPanelObj;

		[Header("PopUp Time")]
		public float FadeInTime = 0.2f;
		public float WaitTime = 0.5f;
		public float FadeOutTime = 0.2f;

		[Header("Error Message Setting")]
		[SerializeField]
		string idEmptyMsg = "No Id Input";
		[SerializeField]
		string pwEmptyMsg = "No Password Input";
		[SerializeField]
		string nicknameEmptyMsg = "No NickName Input";

		/*--- Private Fields ---*/
		enum UI_State { Login, Register}
		UI_State curState;

		string id;
		string password;
		string nickname;

		TMP_InputField idField;
		TMP_InputField passwordField;
		TMP_InputField nicknameField;

		CanvasGroup errMsgCanvasGroup;
		TextMeshProUGUI errMsgText;
		//UIEffector uiEffect;


		/*--- MonoBehaviour Callbacks ---*/
		private void Awake()
		{
			idField = IdFieldObj.GetComponent<TMP_InputField>();
			passwordField = PasswordFieldObj.GetComponent<TMP_InputField>();
			nicknameField = NicknameFieldObj.GetComponent<TMP_InputField>();
			errMsgCanvasGroup = MessageObj.GetComponent<CanvasGroup>();
			errMsgText = MessageObj.GetComponent<TextMeshProUGUI>();
			if ( idField == null || passwordField == null || nicknameField == null )
			{
				Debug.LogError( "AccountService.Awake(): null Refrence" );
			}
		}
		private void Start()
		{
			ActiveLoginScreen();
			LoadingPanelObj.SetActive( false );
			//uiEffect = new UIEffector(errMsgCanvasGroup);

			//AudioManager.instance.Play("LoginBGM");
		}


        /*--- Public Methods ---*/
        public void ActiveLoginScreen()
		{
			IdFieldObj.SetActive( true );
			PasswordFieldObj.SetActive( true );
			NicknameFieldObj.SetActive( false );
			LoginButtonObj.SetActive( true );
			RegisterButtonObj.SetActive( true );
			RedRegisterButtonObj.SetActive(false);
			BackButtonObj.SetActive( false );

			errMsgCanvasGroup.alpha = 0.0f;

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
			RegisterButtonObj.SetActive( false );
			RedRegisterButtonObj.SetActive(true);
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
			LoadingPanelObj.SetActive( true );
			AccountService.Instance.Login( CheckLoginField, InitLoginForm, HandleResponse );
        }
		public void OnRegisterClicked()
        {
			if (curState == UI_State.Login)
            {
				ActiveRegisterScreen();
            }
			else if(curState == UI_State.Register)
			{
				LoadingPanelObj.SetActive( true );
				AccountService.Instance.Register( CheckRegisterField, InitRegisterForm, HandleResponse );
            }

        }
		public void OnBackClicked()
        {
			ActiveLoginScreen();
        }

		public void OnSkipLoginButtonClicked()
		{
			DataManager.Instance.SetLocalAccount( -1, "Test", "Test" );
			StartCoroutine( ChangeScene() );
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
					PopUpMessage( nicknameEmptyMsg);
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
				PopUpMessage( idEmptyMsg );
                return false;
            }
            else if ( password == "" )
            {
                Debug.Log( "AccountService.CheckField: No password Input" );
				PopUpMessage( pwEmptyMsg );
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

			PopUpMessage( data.message);

			if(data.result == "OK")
            {
				if(data.order == "register")
				{
					ActiveLoginScreen();
				}
				else if( data.order == "login")
                {
					DataManager.Instance.SetLocalAccount(data.index, data.id, data.nickname);
					StartCoroutine( ChangeScene() );
                }
            }
		}

		void PopUpMessage(in string msg)
		{
			LoadingPanelObj.SetActive( false );
			errMsgText.text = msg;
			//StartCoroutine(uiEffect.PopUp(FadeInTime, WaitTime, FadeOutTime));
			StartCoroutine( UIEffector.PopUp( errMsgCanvasGroup, FadeInTime, WaitTime, FadeOutTime ) );
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


		IEnumerator ChangeScene()
        {
			yield return new WaitForSeconds( 0.5f );
			GameManager.Instance.LoadScene( NextSceneName );
		}
	}
}
