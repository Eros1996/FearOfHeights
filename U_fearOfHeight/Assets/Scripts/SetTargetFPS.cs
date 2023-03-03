using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFPS : MonoBehaviour
{
    public int fps = 90;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = fps;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
