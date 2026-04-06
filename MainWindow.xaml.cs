using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace QueryBuilder;

public partial class MainWindow : Window
{
    // ── Snackbar timer ──
    private readonly DispatcherTimer _snackTimer = new() { Interval = TimeSpan.FromSeconds(3) };

    public MainWindow()
    {
        InitializeComponent();
        _snackTimer.Tick += (_, _) => HideSnackbar();

        var iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico");
        if (System.IO.File.Exists(iconPath))
            Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri(iconPath));
    }

    // ══════════════════════════════════════════════════════════════
    //  TITLE BAR
    // ══════════════════════════════════════════════════════════════

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
            ToggleMaximize();
        else
            DragMove();
    }

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void MaximizeBtn_Click(object sender, RoutedEventArgs e) =>
        ToggleMaximize();

    private void CloseBtn_Click(object sender, RoutedEventArgs e) =>
        Close();

    private void ToggleMaximize() =>
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    // ══════════════════════════════════════════════════════════════
    //  INPUT CHANGED – live line counter
    // ══════════════════════════════════════════════════════════════

    private void InputBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        var lines = GetNonEmptyLines();
        LineCountText.Text = $"{lines.Length} line{(lines.Length == 1 ? "" : "s")}";
    }

    // ══════════════════════════════════════════════════════════════
    //  CORE LOGIC – Generate Query
    // ══════════════════════════════════════════════════════════════

    private void GenerateBtn_Click(object sender, RoutedEventArgs e)
    {
        var lines = GetNonEmptyLines();

        if (lines.Length == 0)
        {
            ShowSnackbar("⚠  Please enter at least one line.", isWarning: true);
            return;
        }

        // Build: Subject: "line1" OR Subject: "line2" OR ...
        string joined = string.Join(" OR ", lines.Select(l => $"Subject: \"{l}\""));

        OutputBox.Text = joined;

        // Update stats
        StatLines.Text      = lines.Length.ToString();
        StatConditions.Text = lines.Length.ToString();
        StatChars.Text      = joined.Length.ToString();

        ShowSnackbar($"✅  Query generated — {lines.Length} line(s) merged!");
    }

    // ══════════════════════════════════════════════════════════════
    //  COPY TO CLIPBOARD
    // ══════════════════════════════════════════════════════════════

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(OutputBox.Text))
        {
            ShowSnackbar("⚠  Nothing to copy — generate a query first.", isWarning: true);
            return;
        }

        Clipboard.SetText(OutputBox.Text);
        ShowSnackbar("📋  Copied to clipboard!");
    }

    // ══════════════════════════════════════════════════════════════
    //  CLEAR
    // ══════════════════════════════════════════════════════════════

    private void ClearBtn_Click(object sender, RoutedEventArgs e)
    {
        InputBox.Text  = string.Empty;
        OutputBox.Text = string.Empty;
        StatLines.Text = StatConditions.Text = StatChars.Text = "0";
        ShowSnackbar("🗑  Cleared!");
    }

    // ══════════════════════════════════════════════════════════════
    //  SNACKBAR  (animated show / hide)
    // ══════════════════════════════════════════════════════════════

    private void ShowSnackbar(string message, bool isWarning = false)
    {
        _snackTimer.Stop();

        SnackbarText.Text = message;

        // Colour the snackbar based on type
        var colour = isWarning
            ? (Color)FindResource("WarningAmber")
            : (Color)FindResource("AccentPurple");

        SnackbarContainer.Background = new SolidColorBrush(colour) { Opacity = 0.92 };
        SnackbarContainer.Visibility = Visibility.Visible;

        // Fade-in + slide-up
        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        var slideUp = new ThicknessAnimation(
            new Thickness(0, 0, 0, -10),
            new Thickness(0, 0, 0, 16),
            TimeSpan.FromMilliseconds(300))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        SnackbarContainer.BeginAnimation(OpacityProperty, fadeIn);
        SnackbarContainer.BeginAnimation(MarginProperty, slideUp);

        _snackTimer.Start();
    }

    private void HideSnackbar()
    {
        _snackTimer.Stop();

        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };
        fadeOut.Completed += (_, _) => SnackbarContainer.Visibility = Visibility.Collapsed;

        SnackbarContainer.BeginAnimation(OpacityProperty, fadeOut);
    }

    // ══════════════════════════════════════════════════════════════
    //  HELPERS
    // ══════════════════════════════════════════════════════════════

    private string[] GetNonEmptyLines() =>
        InputBox.Text
            .Split('\n', '\r')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();
}
