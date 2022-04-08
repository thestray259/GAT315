using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] FloatData fixedFPS;
	[SerializeField] StringData fps; 
	[SerializeField] List<Force> forces;

	public float fixedDeltaTime => 1.0f / fixedFPS.value;
	public List<Body> bodies { get; set; } = new List<Body>(); 

	private float timeAccumulator = 0; 
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		// get fps 
		fps.value = (1.0f / Time.deltaTime).ToString("F2");
		// add delta time to time accumulator 
		timeAccumulator += Time.deltaTime; 

		forces.ForEach(force => force.ApplyForce(bodies));

		// integrate physics simulation with fixed delta time 
		//while (timeAccumulator >= fixedDeltaTime) // makes the frame rate hella slow 
        {
			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
			});
			timeAccumulator -= fixedDeltaTime;
		}

		// reset body acceleration 
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
