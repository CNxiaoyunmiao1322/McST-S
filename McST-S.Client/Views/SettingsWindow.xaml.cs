using System.Windows;
using McST_S.Client.Services;

namespace McST_S.Client.Views
{
    public partial class SettingsWindow : Window
    {
        private readonly ConfigService _configService;

        public SettingsWindow(ConfigService configService)
        {
            InitializeComponent();
            _configService = configService;

            // 设置数据上下文
            this.DataContext = _configService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 验证输入
            if (!_configService.ValidateConfig())
            {
                MessageBox.Show("配置无效，请检查服务端地址格式。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 保存配置
            _configService.SaveConfig();

            MessageBox.Show("设置已保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // 重新加载配置，丢弃未保存的更改
            _configService.LoadConfig();
            this.DialogResult = false;
            this.Close();
        }
    }
}