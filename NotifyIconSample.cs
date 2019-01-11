/* https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?view=netframework-4.7.2
   This code example displays an application icon in the notification area usiing NotifyIcon.
   It demonstrates Icon, ContextMenu, Text, Visible properties and DoubleClick event handling.
   A ContextMenu Close item is assigned to the NotifyIcon.ContextMenu property,
    allowing a user to close the application.
   Another ContextMenu event handler invokes another Class' method,
     which in turn uses a main class method to change an Icon Balloon string.
   A DoubleClick event hides or activates the application form using Form.Activate method.  
   A Form size change event conditionally activates an Icon Balloon.  
 */
using System;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : System.Windows.Forms.Form
{
    public static Form1 form1; // = new Form1(); // Place this var out of the constructor
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private readonly System.Windows.Forms.ContextMenu RightClickMenu;
    private readonly System.Windows.Forms.MenuItem menuItem1;
    private System.ComponentModel.IContainer container;
    // track state change to set notifyIcon1 Balloon
    private FormWindowState PreviousWindowState;
    // access a SubClass method
    private readonly MySubclass subclass;

    [STAThread]
    static void Main() 
    {
        Application.Run(form1 = new Form1());
    }

    public Form1()
    {
        // Provide a hook for disposing notifyIcon1
        this.container = new System.ComponentModel.Container();
        this.RightClickMenu = new System.Windows.Forms.ContextMenu();
        this.menuItem1 = new System.Windows.Forms.MenuItem("E&xit");
        this.subclass = new MySubclass();

        // Initialize RightClickMenu
        this.RightClickMenu.MenuItems.Add(this.menuItem1);
        this.RightClickMenu.MenuItems.Add("Subclass?", MySubclass);

        // Initialize menuItem1
        this.menuItem1.Index = 0;
        this.menuItem1.Click += new System.EventHandler(this.MenuItem1_Close);

        // Create the NotifyIcon.
        notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.container);

        notifyIcon1.Visible = true;

        // Set the icon that will appear in the systray.
        notifyIcon1.Icon = new Icon("appicon.ico");

        // Set systray icon right-click menu
        notifyIcon1.ContextMenu = this.RightClickMenu;

        // Set text for tooltip to display
        // when hovering mouse over the systray icon.
        notifyIcon1.Text = "Form1 NotifyIcon Icon";
        notifyIcon1.BalloonTipTitle = "Form1 notifyIcon1 Balloon Tip Title";

        // DoubleClick event handler activates the form.
        notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1_DoubleClick);

        // control FormState() MessageBox
        boxing = true;

        // Form1 minimize events launch BallonTips
        this.Resize += new System.EventHandler(this.Form1_Resize);

        /* You get a form, whether wanted or not.
         * It insists on being initially visible
         * unless minimized and hidden on the taskbar.
         */
        FormState("Before Minimixe");
        PreviousWindowState = this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;

        // track Window size changes
        resize_count = 0;

/* Redundant and pointless, here:
        this.Hide();
 */

// Visible becomes true AFTER leaving - where documented?
//      FormState("Leaving Form1");
    }

    protected override void Dispose( bool disposing )
    {
        // Clean up any container being used.
        if( disposing )
            if (container != null)
                container.Dispose();            

        base.Dispose( disposing );
    }

    private bool boxing;
    private System.Windows.Forms.DialogResult button;
    internal void FormState(string current)
    {
        if (boxing)
        {
            button = System.Windows.Forms.MessageBox.Show(
                String.Format("WindowState {0}, Visible {1}, ShowInTaskbar {2}\nPress Cancel to disable these prompts",
                this.WindowState, this.Visible, this.ShowInTaskbar),
                current,
                System.Windows.Forms.MessageBoxButtons.OKCancel,
                System.Windows.Forms.MessageBoxIcon.Information);
            boxing = (button == System.Windows.Forms.DialogResult.OK);
        }
    }

    private int resize_count;
    internal void MsgBoxResize()
    {
        resize_count++;

        FormState(String.Format("Resize Count {0}", resize_count));
    }

    // Use Resize events to check state
    private void Form1_Resize(object sender, EventArgs e)
    {
        MsgBoxResize();
        // New plan
        if (PreviousWindowState != WindowState)
            if (WindowState == FormWindowState.Minimized)
            {
                if (0 == notifyIcon1.BalloonTipText.Length || notifyIcon1.BalloonTipText == this.subclass.text)
                    notifyIcon1.BalloonTipText = "Form Minimize";
                notifyIcon1.ShowBalloonTip(5000);
            }
            else
            {
                notifyIcon1.Visible = false;
                notifyIcon1.Visible = true; // trick to kill BalloonTip when Form1 is no longer minimized;
            }

        PreviousWindowState = WindowState;
    }

    private void NotifyIcon1_DoubleClick(object Sender, EventArgs e) 
    {
        // Show the form when users double clicks notify icon.
        FormState("Entering NotifyIcon1");
        if (0 == this.Text.Length)
	    {
            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Form";
            this.Visible = false;
            this.ShowInTaskbar = true;  // provokes Resize event
            this.Visible = true;
        }

        // Set WindowState to normal if Form is minimized.
        if (this.WindowState == FormWindowState.Minimized)
            this.WindowState = FormWindowState.Normal;
        // else toggle Form visibility
        else if (this.Visible)
            this.Hide();
        else this.Show();

        // Activate the form.
        if (this.Visible && this.WindowState == FormWindowState.Normal)
            this.Activate();
    }

    private void MySubclass(object Sender, EventArgs e) {
        // Invoke a subclass method
        this.subclass.SubBall();
    }

    private void MenuItem1_Close(object Sender, EventArgs e) {
        // Close the form, which closes the application.
        this.Close();
    }

    internal void Balloon(string text)
    {
        notifyIcon1.BalloonTipText = text;
        notifyIcon1.ShowBalloonTip(5000);
    }

}   // Class Form1

// practice methods among classes
public class MySubclass
{
    internal readonly string text;

    public MySubclass()
    {
        text = "Subclass";
    }

    internal void SubBall()
    {
        Form1.form1.Balloon(text);
    }
}
