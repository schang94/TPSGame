using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        private readonly int hashDie = Animator.StringToHash("Die");
        private readonly int hashResurrect = Animator.StringToHash("Resurrect");

        public bool Dead { get; private set; } = false;
        public float maxHp = 100f;
        private float hp;
        public Image hpBar;
        
        private void OnEnable()
        {
            hp = maxHp;
            hpBar.fillAmount = hp;
            
        }
        public void TakeDamage(float damage)
        {
            hp = Mathf.Max(hp -  damage, 0);
            hpBar.fillAmount = hp / maxHp;
            if (hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Dead = true;
            GetComponent<Animator>().SetTrigger(hashDie);
            GetComponent<Animator>().SetBool(hashResurrect, false);
            GetComponent<Mover>().Stop();

            var target = GetComponent<CombatTarget>();
            if (target != null)
            {
                GameManager.Instance.KillUpdate();
                GetComponent<Fighter>().Cancel();
            }

            StartCoroutine(Resurrect());
        }

        IEnumerator Resurrect()
        {
            yield return new WaitForSeconds(3f);
            hp = maxHp;
            hpBar.fillAmount = hp / maxHp;
            GetComponent<Animator>().SetBool(hashResurrect, true);
            Dead = false;
        }
    }
}

