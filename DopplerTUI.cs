using Terminal.Gui;

namespace Doppler
{
    public static class DopplerTUI
    {
        private static readonly string[] sourceArray = [
                "_ New", "_ Singles", "_ Comps", "A", "B", "C", "#"
            ];

        public static void Run()
        {
            Application.Init();
            Toplevel top = Application.Top;

            // === Menu Bar ===
            MenuBar menu = new([
                new("_File", new MenuItem[] {
                    new("_Open Library...", "Open Doppler library folder",
                        () => MessageBox.Query("Open", "Library opened.", "OK")),
                    new("_Exit", "Exit Doppler", () => Application.RequestStop())
                }),
                new("_Tools", new MenuItem[] {
                    new("_Scan", "Scan music library",
                        () => MessageBox.Query("Scan", "Scanning complete.", "OK")),
                    new("_Validate", "Check for missing tracks",
                        () => MessageBox.Query("Validate", "Validation complete.", "OK"))
                }),
                new("_Reports", new MenuItem[] {
                    new("_Summary", "Show summary report",
                        () => MessageBox.Query("Summary", "Albums: 127\nSingles: 384", "OK"))
                }),
                new("_Help", new MenuItem[] {
                    new("_About", "", () => MessageBox.Query("About", "Doppler TUI\nInspired by ZIM", "OK"))
                })
            ]);
            top.Add(menu);

            // === Main Window ===
            Window win = new("Doppler Library Manager")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            // Left Pane
            FrameView leftPane = new("Folders")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(40),
                Height = Dim.Fill()
            };
            ListView folderList = new(sourceArray);
            leftPane.Add(folderList);

            // Right Pane
            FrameView rightPane = new("Track Information")
            {
                X = Pos.Right(leftPane),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            TextView info = new()
            {
                ReadOnly = true,
                Text = "Artist: Flamin’ Groovies\nAlbum: Shake Some Action\nTrack: 03\nTitle: You Tore Me Down\nYear: 1976"
            };
            rightPane.Add(info);

            win.Add(leftPane, rightPane);
            top.Add(win);

            // === Status Bar ===
            StatusBar status = new([
                new(Key.F1, "~F1~ Help", () =>
                    MessageBox.Query("Help", "Use arrow keys to move\nEnter to select\nF9 to quit", "OK")),
                new(Key.F3, "~F3~ Edit", () =>
                    MessageBox.Query("Edit", "Editing not implemented.", "OK")),
                new(Key.F5, "~F5~ Refresh", () =>
                    MessageBox.Query("Refresh", "Screen refreshed.", "OK")),
                new(Key.F9, "~F9~ Quit", () => Application.RequestStop())
            ]);
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
