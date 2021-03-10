// GENERATED AUTOMATICALLY FROM 'Assets/_Project/TestControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TestControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestControls"",
    ""maps"": [
        {
            ""name"": ""Keyboard"",
            ""id"": ""238d5bab-1e91-4210-85ab-1bf15f2667a0"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""77e63b40-81b2-47d1-9002-d05328729d4a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""ef5e6d20-8cf9-4d80-b150-4fd95efda3c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""af4d7311-6d3b-4dbd-8b83-3994097fda2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MediumAttack"",
                    ""type"": ""Button"",
                    ""id"": ""6ed03262-5390-4f5e-ad79-e6de75e2364d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialAttack"",
                    ""type"": ""Button"",
                    ""id"": ""e9b8cf7a-fb5c-45ac-a6cc-f71e8e4dfed5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1c305cbf-fa58-4fac-9115-98a0f88d6438"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""62201e93-2a53-4336-822f-cbfefcef787a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0bc625c7-32b5-494c-b2b2-191ccf709865"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8d728237-9cff-4528-ab2e-27462fa7a115"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d76ef8f7-221f-40f1-afa1-2fe5f0d3ba4d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ca133c86-656a-4988-b757-2411dc5e0b7b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f62a0603-1e01-4f3d-86e4-1716ef870409"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2961771c-5f9a-4147-be22-238695ec5b74"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpecialAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef182283-e08c-4634-adae-da63ea4250d9"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MediumAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard
        m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
        m_Keyboard_Move = m_Keyboard.FindAction("Move", throwIfNotFound: true);
        m_Keyboard_Jump = m_Keyboard.FindAction("Jump", throwIfNotFound: true);
        m_Keyboard_LightAttack = m_Keyboard.FindAction("LightAttack", throwIfNotFound: true);
        m_Keyboard_MediumAttack = m_Keyboard.FindAction("MediumAttack", throwIfNotFound: true);
        m_Keyboard_SpecialAttack = m_Keyboard.FindAction("SpecialAttack", throwIfNotFound: true);
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

    // Keyboard
    private readonly InputActionMap m_Keyboard;
    private IKeyboardActions m_KeyboardActionsCallbackInterface;
    private readonly InputAction m_Keyboard_Move;
    private readonly InputAction m_Keyboard_Jump;
    private readonly InputAction m_Keyboard_LightAttack;
    private readonly InputAction m_Keyboard_MediumAttack;
    private readonly InputAction m_Keyboard_SpecialAttack;
    public struct KeyboardActions
    {
        private @TestControls m_Wrapper;
        public KeyboardActions(@TestControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Keyboard_Move;
        public InputAction @Jump => m_Wrapper.m_Keyboard_Jump;
        public InputAction @LightAttack => m_Wrapper.m_Keyboard_LightAttack;
        public InputAction @MediumAttack => m_Wrapper.m_Keyboard_MediumAttack;
        public InputAction @SpecialAttack => m_Wrapper.m_Keyboard_SpecialAttack;
        public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardActions instance)
        {
            if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnJump;
                @LightAttack.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLightAttack;
                @MediumAttack.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMediumAttack;
                @SpecialAttack.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnSpecialAttack;
            }
            m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @MediumAttack.started += instance.OnMediumAttack;
                @MediumAttack.performed += instance.OnMediumAttack;
                @MediumAttack.canceled += instance.OnMediumAttack;
                @SpecialAttack.started += instance.OnSpecialAttack;
                @SpecialAttack.performed += instance.OnSpecialAttack;
                @SpecialAttack.canceled += instance.OnSpecialAttack;
            }
        }
    }
    public KeyboardActions @Keyboard => new KeyboardActions(this);
    public interface IKeyboardActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnMediumAttack(InputAction.CallbackContext context);
        void OnSpecialAttack(InputAction.CallbackContext context);
    }
}
