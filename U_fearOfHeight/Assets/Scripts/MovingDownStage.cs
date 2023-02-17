using QuickVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingDownStage : QuickStageBase
{
	[Header("Platform Movement Setting")]
	public GameObject userPlatform;
	public GameObject therapistPlatform;
	public GameObject linkPlatform;
	public float height;
	public float duration;

	[Tooltip("if 0 it will be set as height/duration")]
	public float speed;
	[Tooltip("0-90 degrees")]
	public float angle;

	[Header("Therapist Speaker Setting")]
	public AudioSource audioSource;
	public List<AudioClip> audioClips;
	public bool volumeBasedOnDistance;
	public AudioLoudnessDetection audioDetector;
	public float loudnessSensibility = 200;
	public float threshold = 0.1f;
	public float minTimeOfSilence = 10f;

	private int _currentClipIndex = 0;
	private Vector3 _targetPosition;
	private float distanceToTarget;
	private float _timeSinceSilence = 0;

	public override void Init()
	{
		base.Init();

		_maxTimeOut = duration + 60f;

		if (speed == 0f)
		{
			speed = height / duration;
		}

		linkPlatform.transform.Rotate(Vector3.forward, angle);
		linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) / 2;

		var c = height / Mathf.Sin(Mathf.Deg2Rad * angle);
		var z_tanslation = Mathf.Sqrt(c * c - height * height);
		_targetPosition = new Vector3(therapistPlatform.transform.position.x, therapistPlatform.transform.position.y - height, therapistPlatform.transform.position.z + z_tanslation);

		audioSource.clip = audioClips[_currentClipIndex];
		audioSource.Play();
		_currentClipIndex++;
	}

	protected override void Update()
	{
		base.Update();

		var step = speed * Time.deltaTime;
		therapistPlatform.transform.position = Vector3.MoveTowards(therapistPlatform.transform.position, _targetPosition, step);

		var scale = Mathf.Abs(therapistPlatform.transform.position.y) / Mathf.Sin(Mathf.Deg2Rad * angle);
		linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) / 2;
		linkPlatform.transform.localScale = new Vector3(scale / 10, linkPlatform.transform.localScale.y, linkPlatform.transform.localScale.z);

		if (volumeBasedOnDistance)
		{
			distanceToTarget = Vector3.Distance(audioSource.gameObject.transform.position, _vrManager.GetAnimatorTarget().gameObject.transform.position);
			if (distanceToTarget < 1) { distanceToTarget = 0; }
			audioSource.volume = Mathf.Clamp(1.0f - (distanceToTarget / 40f), 0.0f, 1.0f); ;
		}

		float loudness = audioDetector.GetLoudnessFromMicrophone() * loudnessSensibility;
		if (loudness >= threshold)
		{
			//Debug.Log("Player is Speaking");
			_timeSinceSilence = 0;
			return;
		}
		else if (loudness < threshold && !audioSource.isPlaying)
		{
			//Debug.Log("Player is NOT Speaking");
			_timeSinceSilence += Time.deltaTime;
		}

		if (_timeSinceSilence >= minTimeOfSilence && !audioSource.isPlaying) 
		{
			//Debug.Log("Player do NOT speak for 10 sec");

			_timeSinceSilence = 0;
			audioSource.clip = audioClips[_currentClipIndex];
			audioSource.Play();
			_currentClipIndex++;
		}

		/*if (InputManager.GetButtonDown("NextInstruction") && audioClips.Count > _currentClipIndex && !audioSource.isPlaying)
		{
			audioSource.clip = audioClips[_currentClipIndex];
			audioSource.Play();
			_currentClipIndex++;
		}

		if (InputManager.GetButtonDown("CurrentInstruction") && audioClips.Count > _currentClipIndex && !audioSource.isPlaying)
		{
			audioSource.clip = audioClips[_currentClipIndex - 1];
			audioSource.Play();
		}*/

		if (audioClips.Count <= _currentClipIndex && !audioSource.isPlaying)
		{
			//this.Finish();
			_currentClipIndex = 0;
		}
	}
}