// ==========================================
// Файл: ViewModel/PersonViewModel.cs
// Назначение: ViewModel для сотрудников. Данные хранятся в БД (EDM-модель CompanyEntities).
//             Содержит:
//               ListPerson     — коллекция сотрудников для отображения
//               SelectedPerson — выбранный в списке сотрудник
//               GetPersons()   — загрузка сотрудников вместе с должностью (Include)
//               AddPerson      — добавление: context.Persons.Add() + SaveChanges()
//               EditPerson     — редактирование: context.Persons.Find(Id) + SaveChanges()
//               DeletePerson   — удаление: context.Persons.Remove() + SaveChanges()
// ==========================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Data.Entity;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Helper;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Model;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        // ---- SelectedPerson ----

        private Person _selectedPerson;

        /// <summary>Сотрудник, выбранный в ListView</summary>
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        /// <summary>Коллекция сотрудников</summary>
        public ObservableCollection<Person> ListPerson { get; set; }

        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPerson = GetPersons();
        }

        #region GetPersons

        /// <summary>
        /// Загрузка сотрудников из базы данных.
        /// Include("Role") подключает связанную сущность Role, поэтому
        /// в окне WindowEmployee доступно свойство Role.NameRole.
        /// </summary>
        public ObservableCollection<Person> GetPersons()
        {
            ListPerson.Clear();
            try
            {
                using (var context = new CompanyEntities())
                {
                    var query = from per in context.Persons.Include("Role")
                                orderby per.Id
                                select per;

                    foreach (Person p in query)
                        ListPerson.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("\nОшибка загрузки данных!\n" + ex.Message, "Предупреждение");
            }
            return ListPerson;
        }

        #endregion

        #region AddPerson

        private RelayCommand _addPerson;

        /// <summary>Добавление сотрудника в БД</summary>
        public RelayCommand AddPerson
        {
            get
            {
                return _addPerson ?? (_addPerson = new RelayCommand(obj =>
                {
                    Person newPerson = new Person
                    {
                        Birthday = DateTime.Now.Date
                    };
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title       = "Новый сотрудник",
                        DataContext = newPerson
                    };
                    wnPerson.ShowDialog();

                    if (wnPerson.DialogResult == true)
                    {
                        if (string.IsNullOrWhiteSpace(newPerson.LastName) ||
                            string.IsNullOrWhiteSpace(newPerson.FirstName))
                        {
                            MessageBox.Show("Необходимо указать фамилию и имя сотрудника.", "Предупреждение");
                            return;
                        }

                        using (var context = new CompanyEntities())
                        {
                            try
                            {
                                context.Persons.Add(newPerson);
                                context.SaveChanges();
                                ListPerson.Clear();
                                ListPerson = GetPersons();
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

        #region EditPerson

        private RelayCommand _editPerson;

        /// <summary>Редактирование данных по сотруднику</summary>
        public RelayCommand EditPerson
        {
            get
            {
                return _editPerson ?? (_editPerson = new RelayCommand(obj =>
                {
                    Person editPerson = SelectedPerson.ShallowCopy();
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title       = "Редактирование данных сотрудника",
                        DataContext = editPerson
                    };
                    wnPerson.ShowDialog();

                    if (wnPerson.DialogResult == true &&
                        !string.IsNullOrWhiteSpace(editPerson.LastName) &&
                        !string.IsNullOrWhiteSpace(editPerson.FirstName))
                    {
                        using (var context = new CompanyEntities())
                        {
                            // поиск редактируемого сотрудника в контексте по Id
                            Person person = context.Persons.Find(editPerson.Id);
                            if (person != null)
                            {
                                if (person.RoleId    != editPerson.RoleId)    person.RoleId    = editPerson.RoleId;
                                if (person.FirstName != editPerson.FirstName) person.FirstName = editPerson.FirstName;
                                if (person.LastName  != editPerson.LastName)  person.LastName  = editPerson.LastName;
                                if (person.Birthday  != editPerson.Birthday)  person.Birthday  = editPerson.Birthday;
                                try
                                {
                                    context.SaveChanges();
                                    ListPerson.Clear();
                                    ListPerson = GetPersons();
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
                        ListPerson.Clear();
                        ListPerson = GetPersons();
                    }
                }, (obj) => SelectedPerson != null && ListPerson.Count > 0));
            }
        }

        #endregion

        #region DeletePerson

        private RelayCommand _deletePerson;

        /// <summary>Удаление данных по сотруднику</summary>
        public RelayCommand DeletePerson
        {
            get
            {
                return _deletePerson ?? (_deletePerson = new RelayCommand(obj =>
                {
                    Person delPerson = SelectedPerson;
                    using (var context = new CompanyEntities())
                    {
                        // поиск удаляемого сотрудника в контексте по Id
                        Person person = context.Persons.Find(delPerson.Id);
                        if (person != null)
                        {
                            MessageBoxResult result = MessageBox.Show(
                                "Удалить данные по сотруднику: \nФамилия: " + person.LastName +
                                "\nИмя: " + person.FirstName,
                                "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                            if (result == MessageBoxResult.OK)
                            {
                                try
                                {
                                    context.Persons.Remove(person);
                                    context.SaveChanges();
                                    ListPerson.Remove(delPerson);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\nОшибка удаления данных!\n" + ex.Message, "Предупреждение");
                                }
                            }
                        }
                    }
                }, (obj) => SelectedPerson != null && ListPerson.Count > 0));
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
