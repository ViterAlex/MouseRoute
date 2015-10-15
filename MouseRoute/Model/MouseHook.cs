#region Copyright
/// <copyright>
/// Copyright (c) 2011 Ramunas Geciauskas, http://geciauskas.com
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// </copyright>
/// <author>Ramunas Geciauskas</author>
/// <summary>Contains a MouseHook class for setting up low level Windows mouse hooks.</summary>
#endregion

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static MouseRoute.Model.NativeMethods;

namespace RamGecTools {
	/// <summary>
	/// Class for intercepting low level Windows mouse hooks.
	/// </summary>
	public class MouseHook {
		/// <summary>
		/// Internal callback processing function
		/// </summary>
		private MouseHookHandler hookHandler;

		/// <summary>
		/// Function to be called when defined even occurs
		/// </summary>
		/// <param name="sender">MSLLHOOKSTRUCT mouse structure</param>
		internal delegate void MouseHookCallback(MSLLHOOKSTRUCT sender);

        #region Events
        internal event MouseHookCallback LeftButtonDown;
        internal event MouseHookCallback LeftButtonUp;
        internal event MouseHookCallback RightButtonDown;
        internal event MouseHookCallback RightButtonUp;
        internal event MouseHookCallback MouseMove;
        internal event MouseHookCallback MouseWheel;
        internal event MouseHookCallback DoubleClick;
        internal event MouseHookCallback MiddleButtonDown;
        internal event MouseHookCallback MiddleButtonUp;
		#endregion
		/// <summary>
		/// Mouse hooked or not
		/// </summary>
		public bool IsHooked { get; set; }
		/// <summary>
		/// Low level mouse hook's ID
		/// </summary>
		private IntPtr hookID = IntPtr.Zero;

		/// <summary>
		/// Install low level mouse hook
		/// </summary>
		/// <param name="mouseHookCallbackFunc">Callback function</param>
		public void Install() {
			hookHandler = HookFunc;
			hookID = SetHook(hookHandler);
			IsHooked = true;
		}

		/// <summary>
		/// Remove low level mouse hook
		/// </summary>
		public void Uninstall() {
			if (hookID == IntPtr.Zero)
				return;

			UnhookWindowsHookEx(hookID);
			hookID = IntPtr.Zero;
			IsHooked = false;
		}

		/// <summary>
		/// Destructor. Unhook current hook
		/// </summary>
		~MouseHook() {
			Uninstall();
		}

		/// <summary>
		/// Sets hook and assigns its ID for tracking
		/// </summary>
		/// <param name="proc">Internal callback function</param>
		/// <returns>Hook ID</returns>
		private IntPtr SetHook(MouseHookHandler proc) {
			//using (ProcessModule module = Process.GetCurrentProcess().MainModule)
			//return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(module.ModuleName), 0);
			return SetWindowsHookEx(WH_MOUSE_LL, proc, IntPtr.Zero, 0);
		}

		/// <summary>
		/// Callback function
		/// </summary>
		private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam) {
			// parse system messages
			if (nCode >= 0) {
				if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
					if (LeftButtonDown != null)
						LeftButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
					if (LeftButtonUp != null)
						LeftButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
					if (RightButtonDown != null)
						RightButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
					if (RightButtonUp != null)
						RightButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
					if (MouseMove != null)
						MouseMove((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
					if (MouseWheel != null)
						MouseWheel((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_LBUTTONDBLCLK == (MouseMessages)wParam)
					if (DoubleClick != null)
						DoubleClick((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
					if (MiddleButtonDown != null)
						MiddleButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
				if (MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
					if (MiddleButtonUp != null)
						MiddleButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
			}
			return CallNextHookEx(hookID, nCode, wParam, lParam);
		}
	}
}
