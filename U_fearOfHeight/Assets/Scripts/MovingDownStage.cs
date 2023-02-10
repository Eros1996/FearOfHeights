using QuickVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingDownStage : QuickStageBase
{
	public DiscussionManager discussionManager;
	public GameObject userPlatform;
	public GameObject therapistPlatform;
	public GameObject linkPlatform;
	public float height;
	public float duration;

	[Tooltip("if 0 it will be set as height/duration")]
	public float speed;
	[Tooltip("0-90 degrees")]
	public float angle;

	private Vector3 _targetPosition;

	public override void Init() 
	{
		base.Init();
		discussionManager.enabled = true;
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
	}

	protected override void Update()
	{
		base.Update();

		var step = speed * Time.deltaTime;
		therapistPlatform.transform.position = Vector3.MoveTowards(therapistPlatform.transform.position, _targetPosition, step);

		var scale = Mathf.Abs(therapistPlatform.transform.position.y) / Mathf.Sin(Mathf.Deg2Rad * angle);
		linkPlatform.transform.position = (therapistPlatform.transform.position + userPlatform.transform.position) /2;
		linkPlatform.transform.localScale = new Vector3(scale/10, linkPlatform.transform.localScale.y, linkPlatform.transform.localScale.z);
	}
}
