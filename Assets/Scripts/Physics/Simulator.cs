using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] BoolData simulate; 
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps; 
	[SerializeField] List<Force> forces;

	public float fixedDeltaTime => 1.0f / fixedFPS.value;
	public List<Body> bodies { get; set; } = new List<Body>(); 

	private float timeAccumulator = 0; 
	Camera activeCamera;

	BroadPhase broadPhase = new BVH(); // set to BBH 

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		// get fps 
		fps.value = (1.0f / Time.deltaTime).ToString("F2");

		if (!simulate.value) return; 

		// add delta time to time accumulator 
		timeAccumulator += Time.deltaTime; 

		forces.ForEach(force => force.ApplyForce(bodies));

		Vector2 screenSize = GetScreenSize(); 

		// integrate physics simulation with fixed delta time 
		while (timeAccumulator >= fixedDeltaTime)
        {
			// construct broad-phase tree
			broadPhase.Build(new AABB(Vector2.zero, screenSize), bodies);
			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);
			Collision.CreateNarrowPhaseContacts(contacts); 

			bodies.ForEach(body => body.shape.color = Color.cyan);

			//Collision.CreateContacts(bodies, out var contacts);
			Collision.SeperateContacts(contacts); 
			Collision.ApplyImpulses(contacts); 

            bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(-screenSize * 0.5f, screenSize * 0.5f);

				body.shape.GetAABB(body.position).Draw(Color.green);
			});
			timeAccumulator -= fixedDeltaTime;
		}

        // reset body acceleration 
        bodies.ForEach(body => body.acceleration = Vector2.zero);
		broadPhase.Draw();
	}

    public Body GetScreenToBody(Vector3 mousePosition)
    {
		//
		Body body = null;

		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit2D raycastHit = Physics2D.GetRayIntersection(ray); 

		if (raycastHit.collider)
        {
			body = raycastHit.collider.gameObject.GetComponent<Body>(); 
        }

		return body; 
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector2 world = activeCamera.ScreenToWorldPoint(screen);
		return world;//new Vector3(world.x, world.y, 0);
	}

	public Vector2 GetScreenSize()
    {
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
    }

	public void Clear()
    {
		bodies.ForEach(body => Destroy(body.gameObject)); // destroys bodies game objects 
		bodies.Clear(); // removes bodies from list
    }
}
