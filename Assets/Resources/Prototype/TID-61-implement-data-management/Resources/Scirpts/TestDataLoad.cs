using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib;
using KSH_Lib.Data;
namespace KSH_Lib.Test
{
	public class TestDataLoad : MonoBehaviour
	{
		/*--- Public Fields ---*/

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/

		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{

		}
		void Update()
		{
			if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
			{
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
            {
				Debug.Log( "Try Save csv File" );

			}
			if(Input.GetKeyDown(KeyCode.Alpha3))
            {
				Debug.Log( "Try Save to json File" );
            }

		}


		/*--- Public Methods ---*/
		//public bool GetRoleDataFromCSV(string csvPath, out List<RoleData> roleDatas )
  //      {
		//	roleDatas = default;
		//	List<Dictionary<string, object>> data = Util.CSVReader.Read( csvPath );
		//	if(data == null)
  //          {
		//		return false;
  //          }

		//	for ( int i = 0; i < data.Count; ++i )
		//	{
		//		string type = data[i]["RoleType"].ToString();
		//		string name = data[i]["RoleName"].ToString();
		//		float moveSpeed = float.Parse( data[i]["MoveSpeed"].ToString() );
		//		float interactionSpeed = float.Parse( data[i]["InteractionSpeed"].ToString() );
		//		float projectileSpeed = float.Parse( data[i]["ProjectileSpeed"].ToString() );

		//		if ( type == "E" )
		//		{
		//			float attackSpeed = float.Parse( data[i]["AttackSpeed"].ToString() );
		//			float attackPower = float.Parse( data[i]["AttackPower"].ToString() );
		//			roleDatas.Add( new ExorcistData( name, moveSpeed, interactionSpeed, projectileSpeed,  attackPower, attackSpeed ) );
		//		}
		//		else if ( type == "D" )
		//		{
		//			int dollHP = int.Parse( data[i]["DollHP"].ToString() );
		//			int devilHP = int.Parse( data[i]["DevilHP"].ToString() );
		//			roleDatas.Add( new DollData( name, moveSpeed, interactionSpeed, projectileSpeed, dollHP, devilHP ) );
		//		}
		//	}
		//	return true;
		//}

		//public void SaveRoleDataToCSV(string csvPath, List<RoleData> roleDatas)
  //      {
		//	using ( var writer = new Util.CSVWriter( "Assets/Resources/Prototype/TID-61-implement-data-management/Resources/Datas/test.csv" ) )
		//	{
		//		List<string> colums = new List<string>() { "Type", "Name", "Speed", "Interaction", "Projectile", "AttackSpeed", "AttackPower", "DollHP", "DevilHP" };

		//		writer.WriteRow( colums );
		//		colums.Clear();

		//		foreach ( var data in roleDatas )
		//		{
		//			colums.Add( data.Type.ToString() );
		//			colums.Add( data.RoleName );
		//			colums.Add( data.MoveSpeed.ToString() );
		//			colums.Add( data.InteractionSpeed.ToString() );
		//			colums.Add( data.ProjectileSpeed.ToString() );

		//			if ( data.Type == RoleData.RoleType.Exorcist )
		//			{
		//				ExorcistData exorcist = data as ExorcistData;
		//				colums.Add( exorcist.AttackSpeed.ToString() );
		//				colums.Add( exorcist.AttackPower.ToString() );
		//			}
		//			else
		//			{
		//				DollData doll = data as DollData;
		//				colums.Add( "" );
		//				colums.Add( "" );
		//				colums.Add( doll.DollHP.ToString() );
		//				colums.Add( doll.DevilHP.ToString() );
		//			}
		//			writer.WriteRow( colums );
		//			colums.Clear();
		//		}
		//	}
		//}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}