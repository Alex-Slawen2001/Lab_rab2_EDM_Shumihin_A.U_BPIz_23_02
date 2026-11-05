// ==========================================
// Файл: ViewModel/RoleViewModel.cs
// Назначение: ViewModel для должностей. Данные хранятся в БД (EDM-модель CompanyEntities).
//             Содержит:
//               ListRole     — коллекция должностей для отображения
//               SelectedRole — выбранная в списке должность
//               GetRoles()   — загрузка должностей из БД (LINQ-запрос к контексту)
//               AddRole      — добавление: context.Roles.Add() + SaveChanges()
//               EditRole     — редактирование: context.Roles.Find(Id) + SaveChanges()
//               DeleteRole   — удаление: context.Roles.Remove() + SaveChanges()
// ==========================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Helper;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Model;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        // ---- SelectedRole ----

        private Role selectedRole;

        /// <summary>Должность, выбранная в ListView</summary>
        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }

        /// <summary>Коллекция должностей сотрудников</summary>
        public ObservableCollection<Role> ListRole { get; set; }

        public RoleViewModel()
        {
            ListRole = new ObservableCollection<Role>();
            ListRole = GetRoles();
        }

        #region GetRoles

        /// <summary>
        /// Загрузка должностей из базы данных.
        /// Создаётся контекст модели, LINQ-запросом выбираются все должности.
        /// Коллекция ListRole заполняется заново (объект коллекции не меняется,
        /// поэтому привязка ListView к ListRole продолжает работать).
        /// </summary>
        public ObservableCollection<Role> GetRoles()
        {
            ListRole.Clear();
            try
            {
                using (var context = new CompanyEntities())
                {
                    var query = from role in context.Roles
                                orderby role.Id
                                select role;

                    foreach (Role r in query)
                        ListRole.Add(r);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("\nОшибка загрузки данных!\n" + ex.Message, "Предупреждение");
            }
            return ListRole;
        }

        #endregion

        #region command AddRole

        private RelayCommand _addRole;

        /// <summary>Команда добавления новой должности в БД</summary>
        public RelayCommand AddRole
        {
            get
            {
                return _addRole ?? (_addRole = new RelayCommand(obj =>
                {
                    Role newRole = new Role();
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title       = "Новая должность",
                        DataContext = newRole,
                    };
                    wnRole.ShowDialog();

                    if (wnRole.DialogResult == true)
                    {
                        if (string.IsNullOrWhiteSpace(newRole.NameRole))
                        {
                            MessageBox.Show("Наименование должности не может быть пустым.", "Предупреждение");
                            return;
                        }

                        using (var context = new CompanyEntities())
                        {
                            try
                            {
                                newRole.NameRole = newRole.NameRole.Trim();
                                // Id не задаём — его сформирует БД (IDENTITY)
                                context.Roles.Add(newRole);
                                context.SaveChanges();
                                ListRole.Clear();
                                ListRole = GetRoles();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка добавления данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
                    }
                }, (obj) => true));
            }
        }

        #endregion

        #region Command EditRole

        private RelayCommand _editRole;

        /// <summary>Команда редактирования должности</summary>
        public RelayCommand EditRole
        {
            get
            {
                return _editRole ?? (_editRole = new RelayCommand(obj =>
                {
                    Role editRole = SelectedRole.ShallowCopy();
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title       = "Редактирование должности",
                        DataContext = editRole,
                    };
                    wnRole.ShowDialog();

                    if (wnRole.DialogResult == true && !string.IsNullOrWhiteSpace(editRole.NameRole))
                    {
                        using (var context = new CompanyEntities())
                        {
                            // поиск редактируемой должности в контексте по Id
                            Role role = context.Roles.Find(editRole.Id);
                            if (role != null)
                            {
                                if (role.NameRole != editRole.NameRole)
                                    role.NameRole = editRole.NameRole.Trim();
                                try
                                {
                                    context.SaveChanges();
                                    ListRole.Clear();
                                    ListRole = GetRoles();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\nОшибка редактирования данных!\n" + ex.Message, "Предупреждение");
                                }
                            }
                        }
                    }
                    else
                    {
                        ListRole.Clear();
                        ListRole = GetRoles();
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion

        #region DeleteRole

        private RelayCommand _deleteRole;

        /// <summary>Команда удаления должности</summary>
        public RelayCommand DeleteRole
        {
            get
            {
                return _deleteRole ?? (_deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    using (var context = new CompanyEntities())
                    {
                        // поиск удаляемой должности в контексте по Id
                        Role delRole = context.Roles.Find(role.Id);
                        if (delRole != null)
                        {
                            // должность, на которую ссылаются сотрудники, удалять нельзя
                            if (context.Persons.Any(p => p.RoleId == delRole.Id))
                            {
                                MessageBox.Show("Должность «" + delRole.NameRole + "» назначена сотрудникам.\n" +
                                                "Сначала измените должность у этих сотрудников или удалите их.",
                                                "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            MessageBoxResult result = MessageBox.Show(
                                "Удалить данные по должности: " + delRole.NameRole,
                                "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                            if (result == MessageBoxResult.OK)
                            {
                                try
                                {
                                    context.Roles.Remove(delRole);
                                    context.SaveChanges();
                                    ListRole.Remove(role);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\nОшибка удаления данных!\n" + ex.Message, "Предупреждение");
                                }
                            }
                        }
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        #endregion

        // ---- INotifyPropertyChanged ----

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
