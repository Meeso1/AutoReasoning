using System;
using Eto.Forms;
using Eto.Drawing;

namespace AutoReasoning;

public partial class MainForm : Form
{
	public MainForm()
	{
		Title = "My Eto Form";
		MinimumSize = new Size(200, 200);

		// Create a drawable canvas for custom drawing
		var canvas = new Drawable
		{
			Size = new Size(200, 200),
			BackgroundColor = Colors.White
		};

		// Handle the Paint event to draw on the canvas
		canvas.Paint += (sender, e) => 
		{
			e.Graphics.FillEllipse(Colors.SkyBlue, 50, 50, 100, 100);
			using (var pen = new Pen(Colors.DarkBlue, 2))
			{
				e.Graphics.DrawEllipse(pen, 50, 50, 100, 100);
			}
		};

		Content = new StackLayout
		{
			Padding = 10,
			Spacing = 10, // Add spacing between elements
			Items =
			{
				"Hello World!",
				canvas,
				// add more controls here
			}
		};

		// create a few commands that can be used for the menu and toolbar
		var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
		clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

		var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
		quitCommand.Executed += (sender, e) => Application.Instance.Quit();

		var aboutCommand = new Command { MenuText = "About..." };
		aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

		// create menu
		Menu = new MenuBar
		{
			Items =
			{
				// File submenu
				new SubMenuItem { Text = "&File", Items = { clickMe } },
				// new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
				// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
			},
			ApplicationItems =
			{
				// application (OS X) or file menu (others)
				new ButtonMenuItem { Text = "&Preferences..." },
			},
			QuitItem = quitCommand,
			AboutItem = aboutCommand
		};

		// create toolbar			
		ToolBar = new ToolBar { Items = { clickMe } };
	}
}
