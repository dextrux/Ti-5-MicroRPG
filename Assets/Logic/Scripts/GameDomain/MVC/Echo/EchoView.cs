using Logic.Scripts.Turns;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoView : MonoBehaviour,IEchoAction {
        //public AbilityView AbilityToCast { get; private set; }
        //[field: SerializeField] public Transform CastPosition { get; private set; }

        //public void Execute(IAbilityController abilityController) {
        //    abilityController.CreateAbility(CastPosition, AbilityToCast);
        //    Destroy(this, 1f);
        //}

        /*public void SetAbilityToCast(AbilityView ability) {
            AbilityToCast = ability;
        }*/

        public void Execute() { }
    }
}