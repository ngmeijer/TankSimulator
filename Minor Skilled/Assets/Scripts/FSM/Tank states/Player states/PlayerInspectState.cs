using UnityEngine;

namespace FSM.TankStates
{
    public class PlayerInspectState : TankInspectState
    {
        [SerializeField] private GameObject _adsPostProcessVolume;

        public override void Enter()
        {
            base.Enter();

            _adsPostProcessVolume.SetActive(false);
            HandleDamageRegistrationUI(true);
        }

        public override void Exit()
        {
            base.Exit();

            _adsPostProcessVolume.SetActive(true);
            HandleDamageRegistrationUI(false);
        }

        public override void UpdateState()
        {
            GetInputValues();
        }

        private void HandleDamageRegistrationUI(bool enabled)
        {
            _componentManager.DamageComp.ShowUI(enabled);
        }
    }
}