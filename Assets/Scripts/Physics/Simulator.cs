using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	//public Vector2 gravity = new Vector2(0, -9.8f); 
	[SerializeField] List<Force> forces; 

	public List<Body> bodies { get; set; } = new List<Body>(); 
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		forces.ForEach(force => force.ApplyForce(bodies));

		bodies.ForEach(body =>
		{
			//body.Step(Time.deltaTime);
			Integrator.SemiImplicitEuler(body, Time.deltaTime); 
		});

		bodies.ForEach(body =>
		{
			body.acceleration = Vector2.zero; 
		});
	}

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
}
