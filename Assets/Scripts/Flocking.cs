using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public int groupeID;


    public Vector3 baseRotation;

    [Range(0, 10)]
    public float maxSpeed = 1f;

    [Range(.1f, .5f)]
    public float maxForce = .03f;

    [Range(1, 10)]
    public float neighborhoodRadius = 3f;

    [Range(0, 3)]
    public float separationAmount = 1f;

    [Range(0, 3)]
    public float cohesionAmount = 1f;

    [Range(0, 3)]
    public float alignmentAmount = 1f;

    public Vector2 acceleration;                                                                                // <-- VEC
    public Vector2 velocity;                                                                                    // <-- VEC

    private Vector2 Position {
        get { return gameObject.transform.position; }
        set { gameObject.transform.position = value; }
    }

    // Start is called before the first frame update
    void Start() {
        float angle = Random.Range(0, 2 * Mathf.PI);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);                             // <-- VEC
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));                                                 // <-- VEC
    }

    // Update is called once per frame
    void Update() {
        var orcaColliders = Physics2D.OverlapCircleAll(Position, neighborhoodRadius);                               // <-- VEC
        var orcas = orcaColliders.Select(o => o.GetComponent<Flocking>()).ToList();
        orcas.Remove(this);

        Flock(orcas);
        UpdateVelocity();
        UpdatePosition();
        UpdateRotation();
        WrapAround();
    }

    private void Flock(IEnumerable<Flocking> orcas) {
        var alignment = Alignment(orcas);
        var separation = Separation(orcas);
        var cohesion = Cohesion(orcas);

        acceleration = alignmentAmount * alignment + cohesionAmount * cohesion + separationAmount * separation;
    }

    public void UpdateVelocity() {
        velocity += acceleration;
        velocity = LimitMagnitude(velocity, maxSpeed);
    }

    private void UpdatePosition() {
        Position += velocity * Time.deltaTime;
    }

    private void UpdateRotation() {
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
    }

    private Vector2 Alignment(IEnumerable<Flocking> orcas) {
        var velocity = Vector2.zero;
        if (!orcas.Any()) return velocity;

        foreach (var orca in orcas)
        {
            velocity += orca.velocity;
        }
        velocity /= orcas.Count();

        var steer = Steer(velocity.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Cohesion(IEnumerable<Flocking> orcas) {
        if (!orcas.Any()) return Vector2.zero;

        var sumPositions = Vector2.zero;
        foreach (var orca in orcas)
        {
            sumPositions += orca.Position;
        }
        var average = sumPositions / orcas.Count();
        var direction = average - Position;

        var steer = Steer(direction.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Separation(IEnumerable<Flocking> orcas) {
        var direction = Vector2.zero;
        orcas = orcas.Where(o => DistanceTo(o) <= neighborhoodRadius / 2);
        if (!orcas.Any()) return direction;

        foreach (var orca in orcas) {
            var difference = Position - orca.Position;
            direction += difference.normalized / difference.magnitude;
        }
        direction /= orcas.Count();

        var steer = Steer(direction.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Steer(Vector2 desired) {
        var steer = desired - velocity;
        steer = LimitMagnitude(steer, maxForce);

        return steer;
    }

    private float DistanceTo(Flocking orca) {
        return Vector3.Distance(orca.transform.position, Position);
    }

    private Vector2 LimitMagnitude(Vector2 baseVector, float maxMagnitude) {
        if (baseVector.sqrMagnitude > maxMagnitude * maxMagnitude)
        {
            baseVector = baseVector.normalized * maxMagnitude;
        }
        return baseVector;
    }

    private void WrapAround() {
        if (Position.x < 0) Position = new Vector2(1000, Position.y);
        if (Position.y < 0) Position = new Vector2(Position.x, 1000);
        if (Position.x > 1000) Position = new Vector2(0, Position.y);
        if (Position.y > 1000) Position = new Vector2(Position.x, 0);
    }
}
