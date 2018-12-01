using System;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerEventHandler(Player sender);

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public float Water;
    public float WaterUsage;
    public Slider WaterMeter;

    private Transform _transform;
    private float _timePassed;
    private float _startWater;

    private void Awake()
    {
        _transform = transform;
        _timePassed = 0;
        _startWater = Water;
    }

    void Start () {

	}

	void Update () {
        if (Horizontal != 0 || Vertical != 0)
        {
            _transform.position += Time.deltaTime * MoveSpeed * (Vector3.right * Horizontal + Vector3.forward * Vertical);
            _timePassed += Time.deltaTime;

            var usedWater = WaterUsage * _timePassed;
            WaterMeter.value = (_startWater - usedWater) / _startWater;

            if(usedWater >= _startWater)
            {
                Die();
            }
        }
	}

    private void Die()
    {
        throw new NotImplementedException();
    }

    private float Horizontal
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    private float Vertical
    {
        get { return Input.GetAxis("Vertical"); }
    }
}
