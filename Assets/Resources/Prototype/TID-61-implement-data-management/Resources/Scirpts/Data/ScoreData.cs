namespace KSH_Lib
{
	public abstract class ScoreData
	{
		public ulong[] RoleScores;
		public abstract void AddData( in ScoreData data );
	}

	public class ExorcistScoreData : ScoreData
	{
		ExorcistScoreData()
		{
			RoleScores = new ulong[(int)ExorcistType.Count];
		}

		public ulong ClenseScore;
		public ulong JusticeScore;
		public ulong HolyWarScore;
		public ulong JudgeScore;

		public override void AddData( in ScoreData data )
		{
			if ( !(data is ExorcistScoreData) )
			{
				return;
			}
			ExorcistScoreData exData = data as ExorcistScoreData;

			ClenseScore += exData.ClenseScore;
			JusticeScore += exData.JusticeScore;
			HolyWarScore += exData.HolyWarScore;
			JudgeScore += exData.JudgeScore;

			for ( int i = 0; i < (int)ExorcistType.Count; ++i )
			{
				RoleScores[i] += exData.RoleScores[i];
			}
		}
	}
	public class DollScoreData : ScoreData
	{
		DollScoreData()
		{
			RoleScores = new ulong[(int)DollType.Count];
		}

		public ulong CoOpScore;
		public ulong SurviveScore;
		public ulong MissionScore;
		public ulong TauntScore;
		public override void AddData( in ScoreData data )
		{
			if ( !(data is DollScoreData) )
			{
				return;
			}
			DollScoreData dollData = data as DollScoreData;

			CoOpScore += dollData.CoOpScore;
			SurviveScore += dollData.SurviveScore;
			MissionScore += dollData.MissionScore;
			TauntScore += dollData.TauntScore;

			for ( int i = 0; i < (int)DollType.Count; ++i )
			{
				RoleScores[i] += dollData.RoleScores[i];
			}
		}
	}
}