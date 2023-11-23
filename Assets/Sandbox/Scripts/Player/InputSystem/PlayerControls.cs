//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Sandbox/Scripts/Player/InputSystem/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Ground"",
            ""id"": ""cb003142-d357-4d92-856f-72d887e150da"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""64fdc050-7243-4c51-9e7e-3028933f40c9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""357c1591-cac3-462d-baf8-2a4f2c4463b1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""dd943b1a-4931-43a3-911b-1981f5e9ff1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Value"",
                    ""id"": ""96d993d7-9e43-420f-8e20-456704531b8c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Value"",
                    ""id"": ""dc429925-c20b-4659-b4aa-3f007ed804a8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ChangeOrb"",
                    ""type"": ""Button"",
                    ""id"": ""d01bd2e2-40ad-450f-b645-d47920863bac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeOrbWeigth"",
                    ""type"": ""Button"",
                    ""id"": ""aeb18c4c-ca44-4445-9793-0a3ddfc99cc8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Change Weight"",
                    ""type"": ""Button"",
                    ""id"": ""95a600fb-d012-48af-b59f-6a09ac4203c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeOrbDirectly"",
                    ""type"": ""Button"",
                    ""id"": ""868e7e54-a3a6-401c-86b0-89329529ce91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""02dc86d5-4014-4281-ab86-e0a827192550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""d0ff34dc-7993-4162-ad55-896f7939365e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b92681bb-6d76-413f-ac6a-fefccf476e06"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f6f3105-6aac-4a2e-990d-611f3275a7f3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=1.5,y=1.5)"",
                    ""groups"": ""Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13aa705c-51ca-44a1-8a90-5d5dc81209c8"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6347c67e-e471-41ef-928d-105c1ac5ae99"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cef5dd30-f992-4e88-9a5c-284dba28bfb5"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83dc9038-51e8-4fe2-a136-9fe4b24f551c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3300e6d-43c9-4990-91af-26d8391d226a"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e41e8e4-dda4-4a62-91f1-def27b275eb0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0226e75c-372f-44dc-bb64-0d872bae81ba"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f29ae9d9-12ea-44f0-9765-6ba68dce65d8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": """",
                    ""action"": ""ChangeOrb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db0fe492-c3ca-4398-b852-44ca5aa1086d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=-1)"",
                    ""groups"": """",
                    ""action"": ""ChangeOrb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57c5c9d7-6a12-4e91-9b62-e4bbfec3fa9f"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": ""Controller"",
                    ""action"": ""ChangeOrb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e7524da-c0d0-442c-a342-1383b4ebfbdf"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=-1)"",
                    ""groups"": ""Controller"",
                    ""action"": ""ChangeOrb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""392a246b-6430-4c33-800e-26a68cc78aed"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeOrbWeigth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""670f5c97-5066-404e-81f9-fd7ec30ac5b9"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": """",
                    ""action"": ""Change Weight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2132ce0d-d342-4dd7-878f-110530e72d62"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=-1)"",
                    ""groups"": """",
                    ""action"": ""Change Weight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e5a93b1-da20-4df8-893e-fc997f9e3704"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0)"",
                    ""groups"": """",
                    ""action"": ""ChangeOrbDirectly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f5c3663-7cbb-4200-bcbc-08f9280337e2"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": """",
                    ""action"": ""ChangeOrbDirectly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe55d1d6-bc95-4666-ac55-49d11e626729"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=2)"",
                    ""groups"": """",
                    ""action"": ""ChangeOrbDirectly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""c1f3ea60-35ee-4bff-8572-971e4f488838"",
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
                    ""id"": ""eacca528-500d-4922-8a01-532d86bcffb6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b2b06bee-afa6-4b59-babd-2af4f45ee9a6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a67639fb-7f5f-4d1b-beed-c1aee53fa5ac"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1d0ab378-327c-4460-a0e3-8063d0b02653"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""75781f06-ffe2-460b-9691-0f83d87365eb"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dec88de2-1554-4cc4-a59d-a959b242fe70"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aca12a2d-c181-42f6-90db-967f10268d7a"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f6e650c-fe7e-4df7-b32d-919c1bdc5741"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""744b16eb-4bc9-4b60-87dd-7db4d9347f79"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menus"",
            ""id"": ""e8cdb09b-39fc-47fe-a38f-da64a51d6057"",
            ""actions"": [
                {
                    ""name"": ""ResumeGame"",
                    ""type"": ""Button"",
                    ""id"": ""faca21b0-46f2-4610-8b78-e91e9fade7de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""63cd940b-8580-41ee-a15b-71d0683e1fa7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""ResumeGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b954a0b-1c85-4ba1-b953-5e4ea4e1c908"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ResumeGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Ground
        m_Ground = asset.FindActionMap("Ground", throwIfNotFound: true);
        m_Ground_Move = m_Ground.FindAction("Move", throwIfNotFound: true);
        m_Ground_Look = m_Ground.FindAction("Look", throwIfNotFound: true);
        m_Ground_Jump = m_Ground.FindAction("Jump", throwIfNotFound: true);
        m_Ground_Sprint = m_Ground.FindAction("Sprint", throwIfNotFound: true);
        m_Ground_Shoot = m_Ground.FindAction("Shoot", throwIfNotFound: true);
        m_Ground_ChangeOrb = m_Ground.FindAction("ChangeOrb", throwIfNotFound: true);
        m_Ground_ChangeOrbWeigth = m_Ground.FindAction("ChangeOrbWeigth", throwIfNotFound: true);
        m_Ground_ChangeWeight = m_Ground.FindAction("Change Weight", throwIfNotFound: true);
        m_Ground_ChangeOrbDirectly = m_Ground.FindAction("ChangeOrbDirectly", throwIfNotFound: true);
        m_Ground_Interact = m_Ground.FindAction("Interact", throwIfNotFound: true);
        m_Ground_PauseMenu = m_Ground.FindAction("PauseMenu", throwIfNotFound: true);
        // Menus
        m_Menus = asset.FindActionMap("Menus", throwIfNotFound: true);
        m_Menus_ResumeGame = m_Menus.FindAction("ResumeGame", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Ground
    private readonly InputActionMap m_Ground;
    private IGroundActions m_GroundActionsCallbackInterface;
    private readonly InputAction m_Ground_Move;
    private readonly InputAction m_Ground_Look;
    private readonly InputAction m_Ground_Jump;
    private readonly InputAction m_Ground_Sprint;
    private readonly InputAction m_Ground_Shoot;
    private readonly InputAction m_Ground_ChangeOrb;
    private readonly InputAction m_Ground_ChangeOrbWeigth;
    private readonly InputAction m_Ground_ChangeWeight;
    private readonly InputAction m_Ground_ChangeOrbDirectly;
    private readonly InputAction m_Ground_Interact;
    private readonly InputAction m_Ground_PauseMenu;
    public struct GroundActions
    {
        private @PlayerControls m_Wrapper;
        public GroundActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Ground_Move;
        public InputAction @Look => m_Wrapper.m_Ground_Look;
        public InputAction @Jump => m_Wrapper.m_Ground_Jump;
        public InputAction @Sprint => m_Wrapper.m_Ground_Sprint;
        public InputAction @Shoot => m_Wrapper.m_Ground_Shoot;
        public InputAction @ChangeOrb => m_Wrapper.m_Ground_ChangeOrb;
        public InputAction @ChangeOrbWeigth => m_Wrapper.m_Ground_ChangeOrbWeigth;
        public InputAction @ChangeWeight => m_Wrapper.m_Ground_ChangeWeight;
        public InputAction @ChangeOrbDirectly => m_Wrapper.m_Ground_ChangeOrbDirectly;
        public InputAction @Interact => m_Wrapper.m_Ground_Interact;
        public InputAction @PauseMenu => m_Wrapper.m_Ground_PauseMenu;
        public InputActionMap Get() { return m_Wrapper.m_Ground; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundActions set) { return set.Get(); }
        public void SetCallbacks(IGroundActions instance)
        {
            if (m_Wrapper.m_GroundActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnLook;
                @Jump.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnSprint;
                @Shoot.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnShoot;
                @ChangeOrb.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrb;
                @ChangeOrb.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrb;
                @ChangeOrb.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrb;
                @ChangeOrbWeigth.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbWeigth;
                @ChangeOrbWeigth.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbWeigth;
                @ChangeOrbWeigth.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbWeigth;
                @ChangeWeight.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeWeight;
                @ChangeWeight.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeWeight;
                @ChangeWeight.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeWeight;
                @ChangeOrbDirectly.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbDirectly;
                @ChangeOrbDirectly.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbDirectly;
                @ChangeOrbDirectly.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnChangeOrbDirectly;
                @Interact.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnInteract;
                @PauseMenu.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnPauseMenu;
            }
            m_Wrapper.m_GroundActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @ChangeOrb.started += instance.OnChangeOrb;
                @ChangeOrb.performed += instance.OnChangeOrb;
                @ChangeOrb.canceled += instance.OnChangeOrb;
                @ChangeOrbWeigth.started += instance.OnChangeOrbWeigth;
                @ChangeOrbWeigth.performed += instance.OnChangeOrbWeigth;
                @ChangeOrbWeigth.canceled += instance.OnChangeOrbWeigth;
                @ChangeWeight.started += instance.OnChangeWeight;
                @ChangeWeight.performed += instance.OnChangeWeight;
                @ChangeWeight.canceled += instance.OnChangeWeight;
                @ChangeOrbDirectly.started += instance.OnChangeOrbDirectly;
                @ChangeOrbDirectly.performed += instance.OnChangeOrbDirectly;
                @ChangeOrbDirectly.canceled += instance.OnChangeOrbDirectly;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @PauseMenu.started += instance.OnPauseMenu;
                @PauseMenu.performed += instance.OnPauseMenu;
                @PauseMenu.canceled += instance.OnPauseMenu;
            }
        }
    }
    public GroundActions @Ground => new GroundActions(this);

    // Menus
    private readonly InputActionMap m_Menus;
    private IMenusActions m_MenusActionsCallbackInterface;
    private readonly InputAction m_Menus_ResumeGame;
    public struct MenusActions
    {
        private @PlayerControls m_Wrapper;
        public MenusActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResumeGame => m_Wrapper.m_Menus_ResumeGame;
        public InputActionMap Get() { return m_Wrapper.m_Menus; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenusActions set) { return set.Get(); }
        public void SetCallbacks(IMenusActions instance)
        {
            if (m_Wrapper.m_MenusActionsCallbackInterface != null)
            {
                @ResumeGame.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnResumeGame;
                @ResumeGame.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnResumeGame;
                @ResumeGame.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnResumeGame;
            }
            m_Wrapper.m_MenusActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ResumeGame.started += instance.OnResumeGame;
                @ResumeGame.performed += instance.OnResumeGame;
                @ResumeGame.canceled += instance.OnResumeGame;
            }
        }
    }
    public MenusActions @Menus => new MenusActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IGroundActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnChangeOrb(InputAction.CallbackContext context);
        void OnChangeOrbWeigth(InputAction.CallbackContext context);
        void OnChangeWeight(InputAction.CallbackContext context);
        void OnChangeOrbDirectly(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPauseMenu(InputAction.CallbackContext context);
    }
    public interface IMenusActions
    {
        void OnResumeGame(InputAction.CallbackContext context);
    }
}
