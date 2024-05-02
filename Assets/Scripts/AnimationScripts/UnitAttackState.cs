using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    Unit unit;
    private float _timer = 0;

    public float stopAttackingDistance;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        //attackController.SetAttackMaterial();
        unit = animator.GetComponent<Unit>();
        stopAttackingDistance = unit.stopAttackDistance;

        agent.stoppingDistance = unit.attackRange * 0.9f; // nav mesh stop distance tätä pitää säätää vielä, ehkä toteutetaan muulla tavalla.
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == true)
        {
            animator.SetBool("isAttacking", false);
        }
        // check if we have target and if we didnt give another move command
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            _timer += Time.deltaTime;
            // voi olla buginen myöhemmmin animaatioiden kanssa
            LookAtTarget();

            // moving to enemy
            agent.SetDestination(attackController.targetToAttack.position);
            agent.stoppingDistance = unit.attackRange - 0.2f;
            
            // actually perform attack
            if (_timer >= unit.attackCooldown)
            {

                float damage = unit.damage;
                var bullet = Instantiate(unit.bullet, unit.barrel.transform.position, Quaternion.Euler(0, 0, 0));
                bullet.GetComponentInChildren<UnitBullet>().target = attackController.targetToAttack.GetComponent<Collider>();
                var dam = attackController.targetToAttack.GetComponent<IDamageable>();
                if (dam != null)
                {
                    dam.Damage(damage);
                }

                _timer = 0;
            }

            // should unit still attack
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {

                animator.SetBool("isAttacking", false); // move back to follow state
            }


        }
        if (attackController.targetToAttack == null)
        {
            
           
                animator.SetBool("isAttacking", false); // move back to follow state 
         


                
        }

    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.stoppingDistance = 0.5f;
    }
}
