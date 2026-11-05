// ==========================================
// Файл: Helper/RelayCommand.cs
// Назначение: Универсальная команда (реализация ICommand).
//             Используется во всех ViewModel для AddRole, EditRole, DeleteRole,
//             AddPerson, EditPerson, DeletePerson.
//
//             Конструктор принимает:
//               execute    — Action<object>      — логика команды
//               canExecute — Func<object, bool>  — условие доступности (опционально)
//
//             CanExecuteChanged подписывается на CommandManager.RequerySuggested,
//             что позволяет WPF автоматически обновлять состояние кнопок.
// ==========================================

using System;
using System.Windows.Input;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Helper
{
    public class RelayCommand : ICommand
    {
        private Action<object>      execute;
        private Func<object, bool>  canExecute;

        /// <summary>
        /// WPF вызывает это событие, чтобы перепроверить CanExecute.
        /// Подписка на CommandManager.RequerySuggested обеспечивает
        /// автоматическую активацию/деактивацию кнопок.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute    = execute;
            this.canExecute = canExecute;
        }

        /// <summary>Проверяет, доступна ли команда. Если canExecute не задан — всегда true.</summary>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>Выполняет логику команды.</summary>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
