using QuickVR;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MovingDownStage : QuickStageBase
{
	[Header("Platform Movement Setting")]
	public GameObject userPlatform;
	public GameObject therapistPlatform;
	public GameObject linkPlatform;

	[Tooltip("Therapist platform should go down by this quantity")]
	public float height = 10;
	[Tooltip("It is _maxTimeOut - 60f")]
	public float duration = 0;
	[Tooltip("It is height/duration")]
	public float speed = 0f;
	[Tooltip("0-90 degrees")]
	public float angle = 45;

	[Header("Therapist Speaker Setting")]
	public AudioSource audioSource;
	public List<AudioClip> audioClips;
	public bool volumeBasedOnDistance;
	public AudioLoudnessDetection audioDetector;
	public float loudnessSensibility = 200;
	public float threshold = 0.1f;
	public float minTimeOfSilence = 10f;
	public float minWaitingTime = 30f;

	[Header("Therapist LookAt player")]
	public GameObject therapistHead;
	public TextMeshProUGUI textMeshProUGUI;

	private int _currentClipIndex = 0;
	private float distanceToTarget = 0;
	private float _timeSinceSilence = 0;
	private bool _isReachedTarget = false;
	private Vector3 _targetPosition = Vector3.zero;

	public override void Init()
	{
		base.Init();
		duration = _maxTimeOut - 60f;

		linkPlatform.transform.Rotate(Vector3.forward, angle);
		linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) / 2;
		var scale = Mathf.Abs(therapistPlatform.transform.position.y) / Mathf.Sin(Mathf.Deg2Rad * angle);
		linkPlatform.transform.localScale = new Vector3(scale/10f, linkPlatform.transform.localScale.y, linkPlatform.transform.localScale.z);

		var c = height / Mathf.Sin(Mathf.Deg2Rad * angle);
		var z_tanslation = Mathf.Sqrt(c * c - height * height);
		_targetPosition = new Vector3(therapistPlatform.transform.position.x, -height, z_tanslation);  

		audioSource.clip = audioClips[_currentClipIndex];
		audioSource.Play();
		_currentClipIndex++;

		StartCoroutine(MovePlatform(therapistPlatform.transform.position, _targetPosition, duration));
	}

	protected override void Update()
	{
		base.Update();

		//if (volumeBasedOnDistance)
		//{
		//	distanceToTarget = Vector3.Distance(audioSource.gameObject.transform.position, _vrManager.GetAnimatorTarget().gameObject.transform.position);
		//	if (distanceToTarget < 1) { distanceToTarget = 0; }
		//	audioSource.volume = Mathf.Clamp(1.0f - (distanceToTarget / 40f), 0.0f, 1.0f); ;
		//}

		if (audioSource.isPlaying || _isReachedTarget) return;

		float loudness = audioDetector.GetLoudnessFromMicrophone() * loudnessSensibility;
		if (loudness >= threshold)
		{
			_timeSinceSilence = 0;
		}
		else if (loudness < threshold)
		{
			_timeSinceSilence += Time.deltaTime;
		}
	
		if (_timeSinceSilence >= minTimeOfSilence || InputManager.GetButtonDown("NextAudio"))
		{
			_timeSinceSilence = 0;
			audioSource.clip = audioClips[_currentClipIndex];
			audioSource.Play();
			_currentClipIndex++;
		}

		// To be deleted when therapist script is completed
		if (audioClips.Count <= _currentClipIndex)
		{
			_currentClipIndex = 0;
		}
	}

	IEnumerator MovePlatform(Vector3 startPosition, Vector3 targetPosition, float time)
	{
		yield return new WaitForSeconds(minWaitingTime);

		float elapsedTime = 0, scale = 0;
		while (elapsedTime < time)
		{
			var curr = therapistPlatform.transform.position;
			therapistPlatform.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / time));

			//Debug.Log("Current Step: " + (Vector3.Distance(curr, therapistPlatform.transform.position).ToString("F8")));
			//textMeshProUGUI.text = Vector3.Distance(curr, therapistPlatform.transform.position).ToString();
			
			scale = Mathf.Abs(therapistPlatform.transform.position.y) / Mathf.Sin(Mathf.Deg2Rad * angle);
			linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) / 2;
			linkPlatform.transform.localScale = new Vector3(scale / 10, linkPlatform.transform.localScale.y, linkPlatform.transform.localScale.z);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		therapistPlatform.transform.position = _targetPosition;
		scale = Mathf.Abs(therapistPlatform.transform.position.y) / Mathf.Sin(Mathf.Deg2Rad * angle);
		linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) / 2;
		linkPlatform.transform.localScale = new Vector3(scale / 10, linkPlatform.transform.localScale.y, linkPlatform.transform.localScale.z);
		_isReachedTarget = true;
	}

	private void LateUpdate()
	{
		therapistHead.transform.LookAt(_vrManager.GetAnimatorTarget().gameObject.GetComponentInChildren<Camera>().transform);
	}
}