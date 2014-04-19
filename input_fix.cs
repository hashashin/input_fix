// -------------------------------------------------------------------------------------------------
// linux_input_fix.cs 0.0.1
//
// Simple KSP plugin to take linux_input_fix ingame.
// Copyright (C) 2014 Iván Atienza
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// 
// Email: mecagoenbush at gmail dot com
// Freenode: hashashin
//
// -------------------------------------------------------------------------------------------------


using System.Reflection;
using UnityEngine;

namespace input_fix
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class linux_input_fix : MonoBehaviour
    {
        private bool _lock = false;
        private string _keybind;

        private IButton _button;
        private string _tooltipon = "Unlock Input";
        private string _tooltipoff = "Lock Input";
        private string _btexture_on = "input_fix/Textures/icon_on";
        private string _btexture_off = "input_fix/Textures/icon_off";

        private const ControlTypes BLOCK_ALL_CONTROLS = ControlTypes.ALL_SHIP_CONTROLS | ControlTypes.ACTIONS_ALL | ControlTypes.EVA_INPUT | ControlTypes.TIMEWARP | ControlTypes.MISC | ControlTypes.GROUPS_ALL | ControlTypes.CUSTOM_ACTION_GROUPS;
        
        
        void Awake()
        {
            LoadSettings();
            CheckDefaults();
            GUI.SetNextControlName("input_fix");
        }
        
        void Start()
        {
            if (ToolbarManager.ToolbarAvailable)
            {
                _button = ToolbarManager.Instance.add("linux_input_fix", "toggle");
                _button.TexturePath = _btexture_off;
                _button.ToolTip = _tooltipoff;
                _button.OnClick += ((e) =>
                {
                    Toggle();
                });
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(_keybind))
            {
                Toggle();
            }
        }

        void OnDestroy()
        {
            SaveSettings();
            if (_button != null)
            {
                _button.Destroy();
            }
        }

        private void Toggle()
        {
            if (_lock == true)
            {
                _lock = false;
                InputLockManager.RemoveControlLock("input_fix");
                ScreenMessages.PostScreenMessage("INPUT UNLOCKED", 3f, ScreenMessageStyle.UPPER_CENTER);
                _button.TexturePath = _btexture_off;
                _button.ToolTip = _tooltipoff;
            }
            else
            {
                _lock = true;
                
                InputLockManager.SetControlLock(BLOCK_ALL_CONTROLS, "input_fix");
                ScreenMessages.PostScreenMessage("INPUT LOCKED", 3f, ScreenMessageStyle.UPPER_CENTER);
                _button.TexturePath = _btexture_on;
                _button.ToolTip = _tooltipon;
            }
        }

        private void LoadSettings()
        {
            KSPLog.print("[input_fix.dll] Loading Config...");
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<linux_input_fix>();
            configfile.load();

            _keybind = configfile.GetValue<string>("keybind");
        }

        private void SaveSettings()
        {
            KSPLog.print("[input_fix.dll] Saving Config...");
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<linux_input_fix>();

            configfile.SetValue("keybind", _keybind);

            configfile.save();
        }

        private void CheckDefaults()
        {
            
            if (_keybind == null)
            {
                _keybind = "i";
            }            
        }
    }
}
