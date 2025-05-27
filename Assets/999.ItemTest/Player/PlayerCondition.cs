//using System;
//using UnityEngine;

//// 데미지를 받을 때 필요한 인터페이스 작성
//// Player, Monster에 모두 사용 가능
//public interface IDamageable
//{
//    void TakePhysicalDamage(int damage);
//}

//// UI를 참조할 수 있는 PlayerCondition
//// 외부에서 능력치 변경 기능은 이곳을 통해서 호출. 내부적으로 UI 업데이트 수행.
//public class PlayerCondition : MonoBehaviour, IDamageable
//{
//    public UICondition uiCondition;

//    Condition health { get { return uiCondition.health; } }
//    Condition hunger { get { return uiCondition.hunger; } }
//    Condition stamina { get { return uiCondition.stamina; } }

//    public float noHungerHealthDecay; // hunger가 0일때 사용할 값 (value > 0)

//    public event Action OnTakeDamage;

//    // Update is called once per frame
//    void Update()
//    {
//        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
//        stamina.Add(stamina.passiveValue * Time.deltaTime);

//        if (hunger.curValue == 0f)
//        {
//            health.Subtract(noHungerHealthDecay * Time.deltaTime);
//        }

//        if (health.curValue == 0f)
//        {
//            Die();
//        }
//    }

//    public void Heal(float amount)
//    {
//        health.Add(amount);
//    }

//    public void Eat(float amount)
//    {
//        hunger.Add(amount);
//    }

//    public void Die()
//    {
//        Debug.Log("죽었다!");
//    }

//    // 데미지 받을 때 필요한 로직 작성 (health 감소, 데미지 Action 호출)
//    public void TakePhysicalDamage(int damage)
//    {
//        health.Subtract(damage);
//        OnTakeDamage?.Invoke();
//    }

//    public bool UseStamina(float amount)
//    {
//        if (stamina.curValue - amount < 0f)
//        {
//            return false;
//        }

//        stamina.Subtract(amount);
//        return true;
//    }
//}