using QuickVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingDownStage : QuickStageBase
{
	public GameObject targetGB;
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
		_maxTimeOut = duration + 60f;

		if (speed == 0f)
		{
			speed = height / duration;
		}

		var c = height / Mathf.Sin(Mathf.Deg2Rad*angle);
		var z_tanslation = Mathf.Sqrt(c*c - height * height);
		_targetPosition = new Vector3(targetGB.transform.position.x, targetGB.transform.position.y - height, targetGB.transform.position.z + z_tanslation);
	}

	protected override void Update()
	{
		base.Update();

		var step = speed * Time.deltaTime;
		targetGB.transform.position = Vector3.MoveTowards(targetGB.transform.position, _targetPosition, step);
	}
}
