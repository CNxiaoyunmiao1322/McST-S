using System.Windows;
using McST_S.Client.Services;

namespace McST_S.Client
{
    public partial class App : Application
    {
        private ConfigService _configService = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _configService = new ConfigService();

            // 检查是否需要配置
            if (NeedConfiguration())
            {
                // 显示配置窗口
                ShowConfigWindow();
            }
            else
            {
                // 配置有效，直接启动主窗口
                StartMainApplication();
            }
        }

        private bool NeedConfiguration()
        {
            // 如果没有配置文件，则需要配置
            if (!_configService.ConfigExists())
            {
                return true;
            }

            // 加载配置并验证
            _configService.LoadConfig();
            return !_configService.ValidateConfig();
        }

        private void ShowConfigWindow()
        {
            var configWindow = new FirstRunConfigWindow(_configService);
            configWindow.Closed += (s, args) =>
            {
                // 检查配置是否有效
                if (_configService.ValidateConfig())
                {
                    // 配置成功，启动主应用程序
                    StartMainApplication();
                }
                else
                {
                    // 用户取消配置或配置无效，退出程序
                    MessageBox.Show("必须完成初始配置才能使用 McST-S。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Shutdown();
                }
            };

            configWindow.Show();
        }

        private void StartMainApplication()
        {
            // 创建并显示主窗口
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // 设置主窗口，这样当主窗口关闭时应用程序会退出
            this.MainWindow = mainWindow;
        }
    }
}