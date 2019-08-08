using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScript : MonoBehaviour
{

	public bool isRewinding = false;

	List<PointInTime> pointsInTime;
	// Start is called before the first frame update
	void Start()
	{
		pointsInTime = new List<PointInTime>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			StartRewind();
		if (Input.GetKeyUp(KeyCode.Return))
			StopRewind();
	}

	void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[0];
			transform.position = pointInTime.positions;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveAt(0);
		}
		else
		{
			StopRewind();
		}
	}

	void Record()
	{
		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
	}

	void StartRewind()
	{
		isRewinding = true;
	}

	void StopRewind()
	{
		isRewinding = false;
	}
}
