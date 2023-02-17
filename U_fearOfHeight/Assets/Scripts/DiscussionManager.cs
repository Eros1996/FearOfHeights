using QuickVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiscussionManager : MonoBehaviour
{
	public QuickStageBase currentStage;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private int _currentClipIndex = 0;
	private float distance = 0;
    //private AudioSource[] audioSources;
    //private bool _isPlaying = true;
    //private float _timeSinceLastAudio = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(audioClips[_currentClipIndex]);
        _currentClipIndex++;

        //audioSources = GameObject.FindObjectsOfType<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		//distance = Vector3.Distance();

		if (InputManager.GetButtonDown("NextInstruction") && audioClips.Count > _currentClipIndex && !audioSource.isPlaying)
		{
			audioSource.PlayOneShot(audioClips[_currentClipIndex]);
			_currentClipIndex++;
		}

		if (InputManager.GetButtonDown("CurrentInstruction") && audioClips.Count > _currentClipIndex && !audioSource.isPlaying)
		{
			audioSource.PlayOneShot(audioClips[_currentClipIndex-1]);
		}

		if (audioClips.Count <= _currentClipIndex && !audioSource.isPlaying) 
		{
			currentStage.Finish();
		}

		//foreach (AudioSource _audioSource in audioSources)
		//{
		//	if (_audioSource.isPlaying)
		//	{
		//		_isPlaying = true;
		//	}
		//}

		//if (_isPlaying)
		//{
		//	_timeSinceLastAudio = 0f;
		//	_isPlaying = false;
		//}
		//else
		//{
		//	_timeSinceLastAudio += Time.deltaTime;
		//}

		//if(_timeSinceLastAudio > 5f && audioClips.Count > _currentClipIndex)
		//{
		//	audioSource.PlayOneShot(audioClips[_currentClipIndex]);
		//	_currentClipIndex++;
		//	_isPlaying = false;
		//}
	}
}
