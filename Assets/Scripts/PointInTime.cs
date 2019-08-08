
using UnityEngine;

public class PointInTime
{
	public Vector3 positions;
	public Quaternion rotation;

	public PointInTime(Vector3 _position, Quaternion _rotation)
	{
		positions = _position;
		rotation = _rotation;
	}
  
}
