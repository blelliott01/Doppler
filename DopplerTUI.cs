using Terminal.Gui;
using System;

namespace Doppler
{
    public static class DopplerTUI
    {
        public static void Run()
        {
            Application.Init();
            var top = Application.Top;

            // === Menu Bar ===
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Open Library...", "Open Doppler library folder",
                        () => MessageBox.Query("Open", "Library opened.", "OK")),
                    new MenuItem("_Exit", "Exit Doppler", () => Application.RequestStop())
                }),
                new MenuBarItem("_Tools", new MenuItem[] {
                    new MenuItem("_Scan", "Scan music library",
                        () => MessageBox.Query("Scan", "Scanning complete.", "OK")),
                    new MenuItem("_Validate", "Check for missing tracks",
                        () => MessageBox.Query("Validate", "Validation complete.", "OK"))
                }),
                new MenuBarItem("_Reports", new MenuItem[] {
                    new MenuItem("_Summary", "Show summary report",
                        () => MessageBox.Query("Summary", "Albums: 127\nSingles: 384", "OK"))
                }),
                new MenuBarItem("_Help", new MenuItem[] {
                    new MenuItem("_About", "", () => MessageBox.Query("About", "Doppler TUI\nInspired by ZIM", "OK"))
                })
            });
            top.Add(menu);

            // === Main Window ===
            var win = new Window("Doppler Library Manager")
            {
                X = 0, Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            // Left Pane
            var leftPane = new FrameView("Folders")
            {
                X = 0, Y = 0,
                Width = Dim.Percent(40),
                Height = Dim.Fill()
            };
            var folderList = new ListView(new string[] {
                "_ New", "_ Singles", "_ Comps", "A", "B", "C", "#"
            });
            leftPane.Add(folderList);

            // Right Pane
            var rightPane = new FrameView("Track Information")
            {
                X = Pos.Right(leftPane),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var info = new TextView()
            {
                ReadOnly = true,
                Text = "Artist: Flamin’ Groovies\nAlbum: Shake Some Action\nTrack: 03\nTitle: You Tore Me Down\nYear: 1976"
            };
            rightPane.Add(info);

            win.Add(leftPane, rightPane);
            top.Add(win);

            // === Status Bar ===
            var status = new StatusBar(new StatusItem[] {
                new StatusItem(Key.F1, "~F1~ Help", () =>
                    MessageBox.Query("Help", "Use arrow keys to move\nEnter to select\nF9 to quit", "OK")),
                new StatusItem(Key.F3, "~F3~ Edit", () =>
                    MessageBox.Query("Edit", "Editing not implemented.", "OK")),
                new StatusItem(Key.F5, "~F5~ Refresh", () =>
                    MessageBox.Query("Refresh", "Screen refreshed.", "OK")),
                new StatusItem(Key.F9, "~F9~ Quit", () => Application.RequestStop())
            });
            top.Add(status);

            Application.Run();

            // Proper cleanup
            Application.Shutdown();
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("Exited Doppler TUI cleanly.\n");
        }
    }
}
