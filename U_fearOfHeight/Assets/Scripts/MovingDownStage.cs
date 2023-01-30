using QuickVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingDownStage : QuickStageBase
{
	public GameObject targetGB;
	public float heightToReach;
	public float duration; 

	private Vector3 _targetPosition;
	private float speed;

	public override void Init() 
	{
		base.Init();
		speed = heightToReach / duration;
		_targetPosition = new Vector3(targetGB.transform.position.x, -heightToReach, targetGB.transform.position.z);
	}

	protected override void Update()
	{
		base.Update();

		var step = speed * Time.deltaTime;
		targetGB.transform.position = Vector3.MoveTowards(targetGB.transform.position, _targetPosition, step);
	}
}
