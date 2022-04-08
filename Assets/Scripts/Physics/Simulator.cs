using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	//public Vector2 gravity = new Vector2(0, -9.8f); 
	[SerializeField] List<Force> forces;
	[SerializeField] FloatData fixedFPS;
	[SerializeField] StringData fps; 

	public float fixedDeltaTime { get => 1 / fixedFPS.value; }
	public List<Body> bodies { get; set; } = new List<Body>(); 
	private float timeAccumulator; 
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		Debug.Log(1.0f / Time.deltaTime);

		fps.value = (Time.frameCount / fixedDeltaTime).ToString("F1");

		timeAccumulator += Time.deltaTime; 
		forces.ForEach(force => force.ApplyForce(bodies));

		//while (timeAccumulator > fixedDeltaTime) // makes the frame rate hella slow 
        {
			bodies.ForEach(body =>
			{
				//body.Step(Time.deltaTime);
				Integrator.SemiImplicitEuler(body, Time.deltaTime);
			});

			timeAccumulator = timeAccumulator - fixedDeltaTime;

			bodies.ForEach(body =>
			{
				body.acceleration = Vector2.zero; 
			});
		}

	}

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
}
