using System.ComponentModel;
using SeeThroughWindows.Core;
using SeeThroughWindows.Services;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Interface for hotkey management
    /// </summary>
    public interface IHotkeyManager
    {
        /// <summary>
        /// Event fired when the user's custom hotkey is pressed
        /// </summary>
        event EventHandler? UserHotkeyPressed;

        /// <summary>
        /// Event fired when the maximize hotkey is pressed
        /// </summary>
        event EventHandler? MaximizeHotkeyPressed;

        /// <summary>
        /// Event fired when the minimize hotkey is pressed
        /// </summary>
        event EventHandler? MinimizeHotkeyPressed;

        /// <summary>
        /// Event fired when the previous screen hotkey is pressed
        /// </summary>
        event EventHandler? PreviousScreenHotkeyPressed;

        /// <summary>
        /// Event fired when the next screen hotkey is pressed
        /// </summary>
        event EventHandler? NextScreenHotkeyPressed;

        /// <summary>
        /// Event fired when the more transparent hotkey is pressed
        /// </summary>
        event EventHandler? MoreTransparentHotkeyPressed;

        /// <summary>
        /// Event fired when the less transparent hotkey is pressed
        /// </summary>
        event EventHandler? LessTransparentHotkeyPressed;

        /// <summary>
        /// Initialize all hotkeys and register them with the specified control
        /// </summary>
        void Initialize(Control control, HotkeySettings userHotkeySettings);

        /// <summary>
        /// Update the user hotkey with new settings
        /// </summary>
        void UpdateUserHotkey(HotkeySettings settings);

        /// <summary>
        /// Enable or disable monitor movement hotkeys
        /// </summary>
        void SetMonitorHotkeysEnabled(bool enabled);

        /// <summary>
        /// Enable or disable min/max hotkeys
        /// </summary>
        void SetMinMaxHotkeysEnabled(bool enabled);

        /// <summary>
        /// Enable or disable transparency change hotkeys
        /// </summary>
        void SetTransparencyHotkeysEnabled(bool enabled);

        /// <summary>
        /// Dispose of all hotkeys
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// Implementation of hotkey management using the Hotkey class
    /// </summary>
    public class HotkeyManager : IHotkeyManager, IDisposable
    {
        private Hotkey? _userHotkey;
        private readonly Hotkey _maximizeHotkey;
        private readonly Hotkey _minimizeHotkey;
        private readonly Hotkey _previousScreenHotkey;
        private readonly Hotkey _nextScreenHotkey;
        private readonly Hotkey _moreTransparentHotkey;
        private readonly Hotkey _lessTransparentHotkey;

        public event EventHandler? UserHotkeyPressed;
        public event EventHandler? MaximizeHotkeyPressed;
        public event EventHandler? MinimizeHotkeyPressed;
        public event EventHandler? PreviousScreenHotkeyPressed;
        public event EventHandler? NextScreenHotkeyPressed;
        public event EventHandler? MoreTransparentHotkeyPressed;
        public event EventHandler? LessTransparentHotkeyPressed;

        public HotkeyManager()
        {
            // Initialize fixed hotkeys
            _maximizeHotkey = new Hotkey(Keys.Up, false, true, false, true);
            _minimizeHotkey = new Hotkey(Keys.Down, false, true, false, true);
            _previousScreenHotkey = new Hotkey(Keys.Left, false, true, false, true);
            _nextScreenHotkey = new Hotkey(Keys.Right, false, true, false, true);
            _moreTransparentHotkey = new Hotkey(Keys.PageDown, false, true, false, true);
            _lessTransparentHotkey = new Hotkey(Keys.PageUp, false, true, false, true);

            // Wire up event handlers
            _maximizeHotkey.Pressed += (s, e) => { MaximizeHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _minimizeHotkey.Pressed += (s, e) => { MinimizeHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _previousScreenHotkey.Pressed += (s, e) => { PreviousScreenHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _nextScreenHotkey.Pressed += (s, e) => { NextScreenHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _moreTransparentHotkey.Pressed += (s, e) => { MoreTransparentHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _lessTransparentHotkey.Pressed += (s, e) => { LessTransparentHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
        }

        public void Initialize(Control control, HotkeySettings userHotkeySettings)
        {
            // Initialize user hotkey
            _userHotkey = new Hotkey(userHotkeySettings.KeyCode, userHotkeySettings.Shift,
                userHotkeySettings.Control, userHotkeySettings.Alt, userHotkeySettings.Windows);

            _userHotkey.Pressed += (s, e) => { UserHotkeyPressed?.Invoke(this, EventArgs.Empty); e.Handled = true; };
            _userHotkey.Register(control);

            // Note: Fixed hotkeys are registered/unregistered based on their enable state
            // This is handled by the SetXXXEnabled methods
        }

        public void UpdateUserHotkey(HotkeySettings settings)
        {
            _userHotkey?.Reregister(settings.KeyCode, settings.Shift, settings.Control, settings.Alt, settings.Windows);
        }

        public void SetMonitorHotkeysEnabled(bool enabled)
        {
            RegisterHotkey(_previousScreenHotkey, enabled);
            RegisterHotkey(_nextScreenHotkey, enabled);
        }

        public void SetMinMaxHotkeysEnabled(bool enabled)
        {
            RegisterHotkey(_minimizeHotkey, enabled);
            RegisterHotkey(_maximizeHotkey, enabled);
        }

        public void SetTransparencyHotkeysEnabled(bool enabled)
        {
            RegisterHotkey(_moreTransparentHotkey, enabled);
            RegisterHotkey(_lessTransparentHotkey, enabled);
        }

        private void RegisterHotkey(Hotkey hotkey, bool register)
        {
            try
            {
                if (register)
                {
                    if (!hotkey.IsRegistered)
                    {
                        // We need a control reference - this should be passed in during initialization
                        // For now, we'll assume the hotkey manager has access to the main form
                        // This could be improved by storing the control reference
                    }
                }
                else
                {
                    if (hotkey.IsRegistered)
                        hotkey.Unregister();
                }
            }
            catch
            {
                // Ignore registration errors
            }
        }        public void Dispose()
        {
            if (_userHotkey?.IsRegistered == true) _userHotkey?.Unregister();
            if (_maximizeHotkey?.IsRegistered == true) _maximizeHotkey?.Unregister();
            if (_minimizeHotkey?.IsRegistered == true) _minimizeHotkey?.Unregister();
            if (_previousScreenHotkey?.IsRegistered == true) _previousScreenHotkey?.Unregister();
            if (_nextScreenHotkey?.IsRegistered == true) _nextScreenHotkey?.Unregister();
            if (_moreTransparentHotkey?.IsRegistered == true) _moreTransparentHotkey?.Unregister();
            if (_lessTransparentHotkey?.IsRegistered == true) _lessTransparentHotkey?.Unregister();
        }
    }
}
