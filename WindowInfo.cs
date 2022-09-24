using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace aBasics {

    /// <summary>
    /// Informationen und Werkzeuge zu einem Fenster
    /// </summary>
    public class WindowInfo {

        /// <summary>
        /// Gets the window rect.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="lpRect">The lp rect.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// Sets the window pos.
        /// hWnd = Handle des Fensters, welches positioniert werden soll.
        /// hWndInsertAfter = Hinter welchem Fensterhandle das neue Fester positioniert werden soll. Möglich sind auch die folgenden Werte:
        /// Const HWND_BOTTOM = 1
        /// Const HWND_NOTOPMOST = -2
        /// Const HWND_TOP = 0
        /// Const HWND_TOPMOST = -1
        /// x = Die X-Koordinate der neuen Fensterposition (entspricht Left-Eigenschaft) in Pixeln.
        /// y = Die Y-Koordinate der neuen Fensterposition (entspricht Top-Eigenschaft) in Pixeln.
        /// cx = Die Breite der neuen Fensterposition (entspricht Width-Eigenschaft) in Pixeln.
        /// cy = Die Höhe der neuen Fensterposition (entspricht Height-Eigenschaft) in Pixeln.
        /// wFlags =  Steuert, welche Aktion durchgeführt werden soll. Mögliche Werte sind eine Kombination der folgenden Flags:
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hWndInsertAfter">The h WND insert after.</param>
        /// <param name="X">The X.</param>
        /// <param name="Y">The Y.</param>
        /// <param name="W">The W.</param>
        /// <param name="H">The H.</param>
        /// <param name="uFlags">The u flags.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// SWP_NOSIZE verhindert, dass das Fenster eine neue Größe bekommt. cx und cy sind dann irrelevant und können auf 0 gesetzt werden.
        /// SWP_NOMOVE verhindert, dass das Fenster verschoben wird. x und y sind dann irrelevant und können auf 0 gesetzt werden.
        /// SWP_NOZORDER verhindert, dass die Z-Order-Position verändert wird.
        /// SWP_NOREDRAW verhindert, dass irgendetwas automatisch neu gezeichnet wird. Das betrifft sowohl das Fenster selbst, aber auch alle verdeckten Fenster werden nicht invalidiert.
        /// SWP_NOACTIVATE verhindert, dass das Fenster den Fokus erhält.
        /// SWP_FRAMECHANGED wird benutzt um Änderungen der SetWindowLong-Funktion anzuwenden. Sendet eine WM_NCCALCSIZE-Nachricht an das Fenster.
        /// SWP_SHOWWINDOW sorgt dafür, dass das Fenster sichtbar wird. Entspricht .Show() bzw. den Ändern der Visible-Eigenschaft.
        /// SWP_HIDEWINDOW sorgt dafür, dass das Fenster unsichtbar wird. Entspricht .Hide() bzw. den Ändern der Visible-Eigenschaft.
        /// SWP_NOCOPYBITS verwirft den kompletten dargestellten Fensterinhalt und sorgt so für ein vollständiges Neuzeichnen.
        /// SWP_NOOWNERZORDER verschiebt nicht das besitzende Fenster in der Z-Order.
        /// SWP_NOSENDCHANGING verhindert, dass das Fenster die WM_WINDOWPOSCHANGING-Nachricht erhält
        /// SWP_DEFERERASE verhindert, dass das Fenster die WM_SYNCPAINT-Nachricht erhält
        /// SWP_ASYNCWINDOWPOS verhindert das der aufrufende Thread durch den Thread, der das Fenster verarbeitet, blockiert werden kann.
        /// </summary>
        public static readonly uint
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;

        ///// <summary>Position</summary>
        //private RECT Layout;

        /// <summary>Window-Title</summary>
        public string Title;

        /// <summary>Position</summary>
        public int Left;

        /// <summary>Position</summary>
        public int Top;

        /// <summary>Position</summary>
        public int Right;

        /// <summary>Position</summary>
        public int Bottom;

        /// <summary>Position</summary>
        public int Width { get { return Right - Left; } }

        /// <summary>Position</summary>
        public int Height { get { return Bottom - Top; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowInfo" /> class.
        /// </summary>
        public WindowInfo() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowInfo"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public WindowInfo(Process p) {
            RECT Layout;
            if ( !GetWindowRect(p.MainWindowHandle, out Layout) ) throw new Exception("GetWindowRect - false");

            Title   = p.MainWindowTitle;
            Left    = Layout.left;
            Top     = Layout.top;
            Right   = Layout.right;
            Bottom  = Layout.bottom;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() {
            string str = Title + " [" + Top + " / " + Right + " / " + Bottom + " / " + Left + " ]";
            return str;
        }

        /// <summary>
        /// Gets all main windows.
        /// </summary>
        /// <returns></returns>
        public static List<WindowInfo> GetAllMainWindows() {
            List<WindowInfo> lst = new List<WindowInfo>();

            foreach ( Process p in Process.GetProcesses() ) {
                if ( p.MainWindowHandle != null ) {
                    if ( String.IsNullOrEmpty(p.MainWindowTitle) ) continue;

                    WindowInfo inf = new WindowInfo(p);
                    lst.Add(inf);
                }
            }
            return lst;
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        public void SetPosition(){
            IntPtr hWnd = GetHandle(Title);
            SetPosition(hWnd);
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        public void SetPosition(IntPtr hWnd){
            SetWindowPos(hWnd, (IntPtr)0, this.Left, this.Top, this.Width, this.Height, NOZORDER);
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        private IntPtr GetHandle(string title) {
            foreach ( Process p in Process.GetProcesses() ) {
                if ( p.MainWindowHandle != null ) {
                    if ( String.IsNullOrEmpty(p.MainWindowTitle) ) continue;
                    if ( p.MainWindowTitle == title ) return p.MainWindowHandle;
                }
            }
            return new IntPtr(0);
        }
    }
}