using System.Windows;
using McST_S.Client.Services;
using McST_S.Client.Views;

namespace McST_S.Client
{
    public partial class MainWindow : Window
    {
        private readonly ConfigService _configService;

        public MainWindow()
        {
            InitializeComponent();

            // 初始化配置服务
            _configService = new ConfigService();
            _configService.LoadConfig();

            // 初始化UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            // 设置窗口标题显示当前版本
            this.Title = $"McST-S 启动器 - 版本 {_configService.Config.CurrentVersion}";

            // 这里可以添加更多UI初始化代码
            // 例如：加载游戏版本列表、加载账号列表、获取新闻等

            NewsTextBlock.Text = "欢迎使用 McST-S 启动器！\n\n这里将显示游戏新闻和更新信息。";
        }

        private void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // 检查更新逻辑
            MessageBox.Show("检查更新功能即将实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开设置窗口
            var settingsWindow = new SettingsWindow(_configService);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开添加账号窗口
            MessageBox.Show("添加账号功能即将实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ManageAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开账号管理窗口
            MessageBox.Show("账号管理功能即将实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            // 启动游戏逻辑
            MessageBox.Show("启动游戏功能即将实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}