using UnityEngine.Audio;
using UnityEngine;

namespace LSH_Lib
{
	[System.Serializable]
	public class Sound
	{
		public string name;
		public AudioClip clip;
		[Range(0f, 1f)]
		public float volume;
		[Range(-3.0f, 3.0f)]
		public float pitch;
		[HideInInspector]
		public AudioSource source;
		public bool loop;
	}
}
