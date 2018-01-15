using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour {
    //
    public Transform Target;
    Transform MyTransform;

    public float    MaxDistance = 20,
                    MinDistance = 2,
                    SpeedRotation = 50,
                    Speed = 5,
                    TimeCast = 2,
                    AttackDistance = 4;

    public int Damage = 1;
    public bool Couldown = false;

    public int Health = 10;
    public int scoreValue = 10;
    Rigidbody EnemyBody;

	// Use this for initialization
	void Start () {
        EnemyBody = GetComponent<Rigidbody>();

        MyTransform = transform;
        //AI for Enemy
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //если дистанция меньше то
		if (Vector3.Distance(MyTransform.position, Target.position) < MaxDistance)
        {
            Vector3 rot = Target.position - MyTransform.position;
            //поворот противника в сторону игрока
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.LookRotation(rot), SpeedRotation * Time.deltaTime);

            //движение противнкиа к игроку поле видимости
            if (Vector3.Distance(MyTransform.position, Target.position) > MinDistance)
            {
                MyTransform.position += MyTransform.forward * Speed * Time.deltaTime;
            }

            if (Vector3.Distance(MyTransform.position, Target.position) < AttackDistance)
            {
                if (!Couldown)
                {
                    Couldown = true;
                    StartCoroutine(Attack());
                }
            }
        }
	}

    //Enemy will die
    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if ( Health <= 0)
        {
            EnemyBody.constraints = RigidbodyConstraints.None;
            GetComponent<MeshRenderer>().material.color = Color.red;
            StartCoroutine(TimeToDestroy());
        }
    }


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(TimeCast);

        if (Vector3.Distance(MyTransform.position, Target.position) < AttackDistance)
        {
            Target.GetComponent<Player>().TakeDamage(Damage);
        }

        Couldown = false;
    }

    IEnumerator TimeToDestroy()
    {
        Target = null;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        ScoreScript.score += scoreValue;
    }

}
