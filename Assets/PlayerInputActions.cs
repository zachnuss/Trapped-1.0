// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""2bf8cb3a-acdc-4339-a93d-925b95787000"",
            ""actions"": [
                {
                    ""name"": ""Move1"",
                    ""type"": ""Value"",
                    ""id"": ""75917092-b8b8-48b4-bfbf-9a91ddc51f83"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""ebc5a850-33ac-4854-9186-a53168d15948"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Return"",
                    ""type"": ""Button"",
                    ""id"": ""e038c58a-37ce-46fd-8335-74b3f020f307"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""d89b20c8-b54e-4984-9832-476e0c246f5f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""63a43cbf-6890-472c-a388-23d94142c2ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9bad8572-3aa0-48e3-b6b4-c687c18c8fea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b7dc5e8c-75f2-43b4-a5f9-ffdea30e1378"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57206927-5f42-47d1-9512-7ac3abe95e01"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa935b3d-0d1b-42f4-b20c-e63db2120dbb"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fce1fc17-c349-4ecf-808a-6c57b2a490ab"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f19da10-da77-4d3e-9414-a46964fa9a2e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bf6fe89-0de3-4431-afcb-23d18688531e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ea3e3ca-2c77-4ab6-b376-2e0069bc532c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0aac8fc-1933-40bb-9450-bafd9974c129"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Move1 = m_PlayerControls.FindAction("Move1", throwIfNotFound: true);
        m_PlayerControls_Pause = m_PlayerControls.FindAction("Pause", throwIfNotFound: true);
        m_PlayerControls_Return = m_PlayerControls.FindAction("Return", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_Newaction = m_PlayerControls.FindAction("New action", throwIfNotFound: true);
        m_PlayerControls_Attack = m_PlayerControls.FindAction("Attack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move1;
    private readonly InputAction m_PlayerControls_Pause;
    private readonly InputAction m_PlayerControls_Return;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_Newaction;
    private readonly InputAction m_PlayerControls_Attack;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move1 => m_Wrapper.m_PlayerControls_Move1;
        public InputAction @Pause => m_Wrapper.m_PlayerControls_Pause;
        public InputAction @Return => m_Wrapper.m_PlayerControls_Return;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @Newaction => m_Wrapper.m_PlayerControls_Newaction;
        public InputAction @Attack => m_Wrapper.m_PlayerControls_Attack;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move1.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove1;
                @Move1.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove1;
                @Move1.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove1;
                @Pause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Return.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReturn;
                @Return.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReturn;
                @Return.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReturn;
                @Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Newaction.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
                @Attack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move1.started += instance.OnMove1;
                @Move1.performed += instance.OnMove1;
                @Move1.canceled += instance.OnMove1;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Return.started += instance.OnReturn;
                @Return.performed += instance.OnReturn;
                @Return.canceled += instance.OnReturn;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IPlayerControlsActions
    {
        void OnMove1(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnReturn(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnNewaction(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
}
