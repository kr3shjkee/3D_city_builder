using System;
using System.Threading;
using Game.Animations;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class BuildingPartElement : MonoBehaviour
    {
        [SerializeField] private MoveAnimation _animation;
        
        private BuildingStatus _status = BuildingStatus.NotBuilded;
        private CancellationTokenSource _cts;

        public BuildingStatus Status => _status;

        public async void BuildAsync(Vector3 position)
        {
            _status = BuildingStatus.Builded;
            
            try
            {
                _cts = new CancellationTokenSource();
                await _animation.DoMoveAnimationAsync(position, _cts.Token);
            }
            catch (OperationCanceledException e)
            {
                transform.localPosition = position;
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void OnDestroy()
        {
            _cts?.Dispose();
            _cts = null;
        }
    }
}