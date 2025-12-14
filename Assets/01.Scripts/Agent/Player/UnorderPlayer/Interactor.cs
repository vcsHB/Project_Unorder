using System;
using Project_Unorder.AgentSystem.PlayerManage;
using UnityEngine;
namespace Project_Unorder.AgentSystem
{

    public class Interactor : MonoBehaviour, IAgentComponent
    {
        private UnorderPlayer _owner;
        [SerializeField] private DragZone _dragZone;
        private bool _isDragging;


        public void Initialize(Agent owner)
        {
            _owner = owner as UnorderPlayer;
            _owner.PlayerInput.OnMouseClickPressEvent += HandleDragStart;
            _owner.PlayerInput.OnMouseClickReleaseEvent += HandleDragEnd;
        }


        public void AfterInitialize()
        {
        }

        public void Dispose()
        {

            _owner.PlayerInput.OnMouseClickPressEvent -= HandleDragStart;
            _owner.PlayerInput.OnMouseClickReleaseEvent -= HandleDragEnd;
        }



        private void HandleDragStart()
        {
            _dragZone.HandleDragStart(_owner.transform.position);
            _isDragging = true;
        }

        void Update()
        {
            if (_isDragging)
            {
                _dragZone.SetMousePosition(Camera.main.ScreenToWorldPoint(_owner.PlayerInput.MousePosition));
            }
        }
        private void HandleDragEnd()
        {
            _isDragging = false;
            _dragZone.Release();

        }

        public void LateInitialize()
        {
        }
    }
}