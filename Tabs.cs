using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Windows.Forms;

namespace MSEdgeTabsOutput
{
    public class GetTabs
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

        private struct Windowplacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public static List<string> GetAndChangeTabUrl()
        {
            Process[] procsEdge = System.Diagnostics.Process.GetProcessesByName("msedge");
            List<string> tabs = new List<string>();
            foreach (Process proc in procsEdge)
            {
                //string name = proc.ProcessName;
                Windowplacement placement = new Windowplacement();
                GetWindowPlacement(proc.MainWindowHandle, ref placement);

                // Check if window is minimized
                if (placement.showCmd == 2)
                {
                    //the window is hidden so we restore it
                    //ShowWindow(proc.MainWindowHandle.ToInt32(), 9);
                }

                //Switch Edge tab to the first one
                //SetForegroundWindow(proc.MainWindowHandle);
                //SendKeys.SendWait("^1");

                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;

                string TabUrl = string.Empty;
                string TabTitle = string.Empty;
                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
               
                Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                foreach (AutomationElement tabitem in root.FindAll(TreeScope.Subtree, condTabItem))
                {
                    var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                    TabUrl = (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty);
                    TabTitle = tabitem.Current.Name;
                    //tabs.Add("URL: " + TabUrl + " ----Title: " + TabTitle);
                    tabs.Add(TabTitle);
                    //SetForegroundWindow(proc.MainWindowHandle);
                    //SendKeys.SendWait("^{TAB}"); // change focus to next tab
                }
            }
            return tabs;
        }
    }
}