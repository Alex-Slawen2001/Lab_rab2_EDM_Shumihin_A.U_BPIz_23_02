// ==========================================
// Файл: View/WindowNewEmployee.xaml.cs
// Назначение: Code-behind диалога добавления/редактирования сотрудника.
//             В конструкторе заполняет CbRole списком должностей из БД (RoleViewModel).
//             Window_Loaded — для нового сотрудника (RoleId = 0) выбирает первую должность.
// ==========================================

using System.Windows;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.ViewModel;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View
{
    /// <summary>
    /// Логика взаимодействия для WindowNewEmployee.xaml
    /// </summary>
    public partial class WindowNewEmployee : Window
    {
        public WindowNewEmployee()
        {
            InitializeComponent();

            RoleViewModel vmRole = new RoleViewModel();
            CbRole.ItemsSource = vmRole.ListRole;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // если должность ещё не выбрана — выбираем первую (RoleId обновится через привязку)
            if (CbRole.SelectedIndex < 0 && CbRole.Items.Count > 0)
                CbRole.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
