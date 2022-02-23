using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSensor : Sensor
{
	
	[SerializeField] private float radius;
	private float radius2;

	public float Radius
	{
		get => this.radius;
		set
		{
			this.radius = value;
			this.radius2 = this.radius * this.radius;
		}
	}
	protected float Radius2 => this.radius2;

	
	protected virtual void Start()
	{
		SetRadius(radius);
	}

	
	public void SetRadius(float r)
	{
		radius = r;
		radius2 = r * r;
	}

	
	public override bool CanSee(Transform target)
	{
		Vector3 delta = target.position - transform.position;
		Debug.DrawRay(transform.position, delta, Color.red);

		if (delta.sqrMagnitude <= radius2)
		{
			Ray ray = new Ray(transform.position, delta);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				return true;
			}
			//RaycastHit2D hit = Physics2D.Raycast(transform.position, delta, radius);
			if (hit.transform == target)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
    /// This is used for debugging purposes to help visualise the sphere
    /// using a wire frame in the scene view.
    /// </summary>
    /// Author: Max Schafer
    /// Date: 2021-10-20
    /// Description Initial testing
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}