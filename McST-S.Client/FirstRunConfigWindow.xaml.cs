using System.Text.RegularExpressions;
using System.Windows;
using McST_S.Client.Services;

namespace McST_S.Client
{
    public partial class FirstRunConfigWindow : Window
    {
        private readonly ConfigService _configService;
        private bool _isInitialized = false;

        public FirstRunConfigWindow(ConfigService configService)
        {
            InitializeComponent();
            _configService = configService;
            _isInitialized = true;

            // 初始验证
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            // 确保UI元素已初始化
            if (!_isInitialized || ServerUrlTextBox == null || VersionTextBox == null ||
                SaveButton == null || StatusTextBlock == null)
                return;

            var serverUrl = ServerUrlTextBox.Text.Trim();
            var version = VersionTextBox.Text.Trim();

            // 验证服务端地址
            bool isServerUrlValid = !string.IsNullOrWhiteSpace(serverUrl) &&
                                   Uri.TryCreate(serverUrl, UriKind.Absolute, out var uriResult) &&
                                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            // 验证版本号格式 (x.x.x.x)
            bool isVersionValid = !string.IsNullOrWhiteSpace(version) &&
                                 Regex.IsMatch(version, @"^\d+(\.\d+){3}$");

            // 更新UI状态
            Dispatcher.Invoke(() =>
            {
                SaveButton.IsEnabled = isServerUrlValid && isVersionValid;

                if (!isServerUrlValid && !string.IsNullOrWhiteSpace(serverUrl))
                {
                    StatusTextBlock.Text = "请输入有效的HTTP/HTTPS地址";
                }
                else if (!isVersionValid && !string.IsNullOrWhiteSpace(version))
                {
                    StatusTextBlock.Text = "版本号格式应为 x.x.x.x";
                }
                else
                {
                    StatusTextBlock.Text = "";
                }
            });
        }

        private void ServerUrlTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void VersionTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 保存配置
            _configService.Config.ServerUrl = ServerUrlTextBox.Text.Trim();
            _configService.Config.CurrentVersion = VersionTextBox.Text.Trim();
            _configService.SaveConfig();

            // 验证配置
            if (_configService.ValidateConfig())
            {
                MessageBox.Show("配置已保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                // 直接关闭窗口，而不是设置 DialogResult
                this.Close();
            }
            else
            {
                MessageBox.Show("配置验证失败，请检查输入的值。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}