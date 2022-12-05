using CodeBase.Infrastructure.Services;
using CodeBase.Services.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.UI.Buttons
{
    public class AttackButton : MonoBehaviour
    {
        private IInputService _inputService;


        private void Awake()
        {
            Graphic graphic = GetComponent<Graphic>();
            if (graphic != null)
                graphic.raycastTarget = true;

            _inputService = AllServices.Container.Single<IInputService>();
        }
    }
}
