/* https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?view=netframework-4.7.2
   This code example uses the NotifyIcon class to display an icon for an application in the notification area.
   It demonstrates setting the Icon, ContextMenu, Text, and Visible properties and handling the DoubleClick event.
   A ContextMenu with an Exit item on it is assigned to the NotifyIcon.ContextMenu property,
    which allows the user to close the application.
   When the DoubleClick event occurs, the application form is activated by calling the Form.Activate method.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : System.Windows.Forms.Form
{
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private readonly System.Windows.Forms.ContextMenu contextMenu1;
    private readonly System.Windows.Forms.MenuItem menuItem1;
    private System.ComponentModel.IContainer components;
    
    [STAThread]
    static void Main() 
    {
        Application.Run(new Form1());
    }

    public Form1()
    {
        this.components = new System.ComponentModel.Container();
        this.contextMenu1 = new System.Windows.Forms.ContextMenu();
        this.menuItem1 = new System.Windows.Forms.MenuItem();

        // Initialize contextMenu1
        this.contextMenu1.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] {this.menuItem1});

        // Initialize menuItem1
        this.menuItem1.Index = 0;
        this.menuItem1.Text = "E&xit";
        this.menuItem1.Click += new System.EventHandler(this.MenuItem1_Click);

        // Create the NotifyIcon.
        notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

        notifyIcon1.Visible = true;

        // Set the icon that will appear in the systray.
        notifyIcon1.Icon = new Icon("appicon.ico");

        // Sets the menu appears when the systray icon is right clicked.
        notifyIcon1.ContextMenu = this.contextMenu1;

        // Set the text that displays in a tooltip
        // when the mouse hovers over the systray icon.
        notifyIcon1.Text = "Form1 NotifyIcon Icon";
        notifyIcon1.BalloonTipTitle = "Form1 notifyIcon1 Balloon Tip Title";

        // Handle the DoubleClick event to activate the form.
        notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1_DoubleClick);

        // Pay attention to Form1 minimize
        this.Resize += new System.EventHandler(this.Form1_Resize);

        /* You get a form, whether or not it is wanted.
         * It insists on being initially visible
         * unless minimized and hidden there.
         */
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
        this.Visible = false;
    }

    protected override void Dispose( bool disposing )
    {
        // Clean up any components being used.
        if( disposing )
            if (components != null)
                components.Dispose();            

        base.Dispose( disposing );
    }

    private void NotifyIcon1_DoubleClick(object Sender, EventArgs e) 
    {
        // Show the form when the user double clicks on the notify icon.
        if (0 == this.Text.Length)
	    {
            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Form";
            this.Visible = false;
            this.ShowInTaskbar = true;  // provokes Resize event
            this.Visible = true;
        }

        // Set the WindowState to normal if the form is minimized.
        if (this.WindowState == FormWindowState.Minimized)
            this.WindowState = FormWindowState.Normal;
        else if (this.Visible)
            this.Hide();
        else this.Show();

        // Activate the form.
        if (this.Visible && this.WindowState == FormWindowState.Normal)
            this.Activate();
    }

    private void MenuItem1_Click(object Sender, EventArgs e) {
        // Close the form, which closes the application.
        this.Close();
    }

// Use the Resize event to check the Forms.WindowState property.
    private void Form1_Resize(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized && this.Visible)
        {
            // First event is invalid
            if (0 == notifyIcon1.BalloonTipText.Length)
                notifyIcon1.BalloonTipText = "Form minimized.";
            else
              notifyIcon1.ShowBalloonTip(5000);
        }
    }
}
