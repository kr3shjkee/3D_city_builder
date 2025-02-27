using Core.MVP.Presenters;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Presenters
{
    public class PlayerPresenter : IPresenter, IFixedTickable
    {
        private readonly PlayerView _view;
        private readonly string _grounded = "Grounded";
        private readonly string _moveSpeed = "MoveSpeed";
        
        private float _speed = 3f;

        public PlayerPresenter(PlayerView view)
        {
            _view = view;
        }
        
        public void Enable()
        {
            
        }

        public void Disable()
        {
            
        }

        public void FixedTick()
        {
            _view.Animator.SetBool(_grounded, true);
            
            Vector3 direction = Vector3.right * _view.Joystick.Vertical + Vector3.back * _view.Joystick.Horizontal;

            _view.Rigidbody.velocity = direction * _speed;
            
            if (_view.Joystick.Horizontal != 0 || _view.Joystick.Vertical != 0) 
                _view.transform.rotation = Quaternion.LookRotation(_view.Rigidbody.velocity);
            
            if (_view.Joystick.Vertical != 0)
                _view.Animator.SetFloat(_moveSpeed, 1f);
            else
                _view.Animator.SetFloat(_moveSpeed, 0);
        }
    }
}