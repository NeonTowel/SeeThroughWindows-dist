using SeeThroughWindows.Infrastructure;
using SeeThroughWindows.Services;

namespace SeeThroughWindows;

static class Program
{
  /// <summary>
  /// The main entry point for the application.
  /// </summary>
  [STAThread]
  static void Main()
  {
    try
    {
      // Make sure we have a single instance of this application running:
      bool ok;
      Mutex m = new Mutex(true, "MOBZystems.SeeThroughWindows", out ok);

      if (!ok)
      {
        MessageBox.Show(null, "See Through Windows is already active in the system tray!", "See Through Windows", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Exit immediately
        return;
      }

      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();

      // Set up dependency injection
      var serviceContainer = new ServiceContainer();
      ConfigureServices(serviceContainer);
      ServiceLocator.Initialize(serviceContainer);

      // Run the application
      var mainForm = ServiceLocator.Resolve<SeeThrougWindowsForm>();
      Application.Run(mainForm);

      GC.KeepAlive(m); // important!
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Application failed to start:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                      "SeeThroughWindows Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }
  }

  /// <summary>
  /// Configure dependency injection services
  /// </summary>
  private static void ConfigureServices(ServiceContainer container)
  {
    // Register services
    container.RegisterTransient<ISettingsManager, RegistrySettingsManager>();
    container.RegisterTransient<IWindowManager, WindowManager>();
    container.RegisterTransient<IHotkeyManager, HotkeyManager>();
    container.RegisterTransient<IUpdateChecker, GitHubUpdateChecker>();

    // Register application service
    container.RegisterFactory<IApplicationService>(() =>
    {
      var windowManager = container.Resolve<IWindowManager>();
      var settingsManager = container.Resolve<ISettingsManager>();
      return new ApplicationService(windowManager, settingsManager);
    });

    // Register main form
    container.RegisterFactory<SeeThrougWindowsForm>(() =>
    {
      var applicationService = container.Resolve<IApplicationService>();
      var hotkeyManager = container.Resolve<IHotkeyManager>();
      var settingsManager = container.Resolve<ISettingsManager>();
      var updateChecker = container.Resolve<IUpdateChecker>();

      return new SeeThrougWindowsForm(applicationService, hotkeyManager, settingsManager, updateChecker);
    });
  }
}
