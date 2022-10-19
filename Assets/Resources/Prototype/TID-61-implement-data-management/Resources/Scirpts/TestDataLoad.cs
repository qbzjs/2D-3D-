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
		List<RoleData> roleDatas;

		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{

		}
		void Update()
		{
			if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
			{
				Debug.Log( "Try Load csv File" );
				List<Dictionary<string, object>> data = Util.CSVReader.Read( "Prototype/TID-61-implement-data-management/Resources/Datas/ChracterStatus" );
				for(int i = 0; i < data.Count; ++i )
                {
					//RoleType	RoleName	MoveSpeed	InteractionSpeed	ProjectileSpeed	AttackSpeed	AttackPower	DollHP	DevilHP
					//print( $"RollType: {data[i]["RollType"]}" );
					//print( $"RollName: {data[i]["RollName"]}" );
					//print( $"MoveSpeed: {data[i]["MoveSpeed"]}" );
					//print( $"InteractionSpeed: {data[i]["InteractionSpeed"]}" );
					//print( $"ProjectileSpeed: {data[i]["ProjectileSpeed"]}" );
					//print( $"AttackSpeed: {data[i]["AttackSpeed"]}" );
					//print( $"RoleAttackPowerlType: {data[i]["AttackPower"]}" );
					//print( $"DollHP: {data[i]["DollHP"]}" );
					//print( $"DevilHP: {data[i]["DevilHP"]}" );

					string name = data[i]["RoleName"].ToString();
					float moveSpeed = (float)data[i]["MoveSpeed"];
					float iteractionSpeed = (float)data[i]["InteractionSpeed"];
					float projectileSpeed = (float)data[i]["ProjectileSpeed"];
					float attackSpeed = (float)data[i]["AttackSpeed"];
					float attackPower = (float)data[i]["AttackPower"];

					if (data[i]["RoleType"].ToString() == "E")
					{
						//roleDatas.Add( new ExorcistData())
					}


                }
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
            {
				Debug.Log( "Try Save csv File" );
				using ( var writer = new Util.CSVWriter( "Assets/Resources/Prototype/TID-61-implement-data-management/Resources/Datas/test.csv" ) )
				{
					List<string> columns = new List<string>() { "Name", "Level", "Hp", "Exp", "Str", "Dex", "Con", "Int" };// making Index Row
					writer.WriteRow( columns );
					columns.Clear();

					columns.Add( "Bbulle" ); // Name
					columns.Add( "99" ); // Level
					columns.Add( "999" ); // Hp
					columns.Add( "5000" ); // Exp
					columns.Add( "99" ); // Str
					columns.Add( "50" ); // Dex
					columns.Add( "80" ); // Con
					columns.Add( "40" ); // Int
					writer.WriteRow( columns );
					columns.Clear();

					columns.Add( "Kukai" ); // Name
					columns.Add( "50" ); // Level
					columns.Add( "666" ); // Hp
					columns.Add( "3500" ); // Exp
					columns.Add( "66" ); // Str
					columns.Add( "66" ); // Dex
					columns.Add( "44" ); // Con
					columns.Add( "22" ); // Int
					writer.WriteRow( columns );
					columns.Clear();
				}
			}
			if(Input.GetKeyDown(KeyCode.Alpha3))
            {
				Debug.Log( "Try Save to json File" );
            }

		}


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}