// ==========================================
// Файл: MainWindow.xaml.cs
// Назначение: Code-behind главного окна.
//             Обработчики меню открывают окна сотрудников и должностей.
// ==========================================

using System.Windows;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Меню "Сотрудники" — открывает окно со списком и командами
        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            WindowEmployee wEmployee = new WindowEmployee();
            wEmployee.Show();
        }

        // Меню "Должности" — открывает окно со списком и командами
        private void Role_OnClick(object sender, RoutedEventArgs e)
        {
            WindowRole wRole = new WindowRole();
            wRole.Show();
        }
    }
}
