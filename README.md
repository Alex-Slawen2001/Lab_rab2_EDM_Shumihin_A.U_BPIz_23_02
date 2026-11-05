# Лабораторная работа 2 — Разработка приложения, взаимодействующего с моделью данных EDM (C#, WPF, Entity Framework 6)

Выполнил: Шумихин А.Ю., БПИз-23-02

Данные хранятся в локальной базе данных **CompanyEntities** (SQL Server Express LocalDB).
Доступ к ним — через модель **ADO.NET EDM** (Entity Framework 6, подход **Code First**):
база данных и таблицы строятся по классам `Role` и `Person`.
Проект — WPF-приложение на **.NET Framework 4.8** (шаблон «Модель ADO.NET EDM» работает только с .NET Framework).

## Структура проекта

```
Lab_rab2_EDM_Shumihin_A.U_BPIz_23_02/
├── App.config                      ← строка подключения CompanyEntities + настройки Entity Framework
├── App.xaml / App.xaml.cs          ← культура ru-RU (формат дат)
├── CompanyEntities.cs              ← модель EDM: DbSet<Role> Roles, DbSet<Person> Persons
│                                     + инициализатор БД с начальными данными (Seed)
├── MainWindow.xaml(.cs)            ← меню «Сотрудники» / «Должности»
├── Model/
│   ├── Role.cs                     ← Id, NameRole + коллекция Persons (связь с Person)
│   └── Person.cs                   ← Id, RoleId, FirstName, LastName, Birthday + свойство Role
├── ViewModel/
│   ├── RoleViewModel.cs            ← GetRoles(), AddRole, EditRole, DeleteRole
│   └── PersonViewModel.cs          ← GetPersons() c Include("Role"), AddPerson, EditPerson, DeletePerson
├── View/                           ← окна списков и диалоги добавления/редактирования
└── Helper/RelayCommand.cs
```

## Выполнение по пунктам методички

1. **Модель данных.** Класс `PersonDpo` удалён. В `Role` оставлены `Id`, `NameRole` и добавлена коллекция
   `Persons`; в `Person` оставлены данные сотрудника и добавлено свойство `Role` (связь с `Role`).
2. **Модель ADO.NET EDM** с именем `CompanyEntities`, технология Code First
   (Проект → Добавить новый элемент → Данные → Модель ADO.NET EDM → «Пустая модель Code First»).
   В сгенерированный класс добавлены сущности `Roles` и `Persons`; строка подключения — в `App.config`.
3. **Локальная БД.** При первом обращении к данным создаётся БД `CompanyEntities` в SQL Server Express LocalDB
   с таблицами `Roles` и `People` (имя `People` Entity Framework образует от класса `Person`)
   и заполняется начальными данными.
4. **Должности** (`RoleViewModel`): загрузка `GetRoles()` LINQ-запросом; добавление — `context.Roles.Add` +
   `SaveChanges()`; редактирование — `context.Roles.Find(Id)`, изменение, `SaveChanges()`;
   удаление — `context.Roles.Remove` + `SaveChanges()` с предупреждением.
5. **Сотрудники** (`PersonViewModel`): загрузка `GetPersons()` с `Include("Role")`; добавление, редактирование,
   удаление — аналогично должностям.
6. **XAML.** В `WindowEmployee` должность выводится через `Role.NameRole`; в `WindowNewEmployee` у ComboBox
   заданы `DisplayMemberPath`, `SelectedValue` и `SelectedValuePath`.

Дополнительно: должность, назначенную сотрудникам, удалить нельзя (выводится предупреждение);
пустые фамилия / имя / должность не сохраняются.

## Запуск и проверка

1. Нужны Visual Studio с рабочей нагрузкой «Разработка классических приложений .NET» (.NET Framework 4.8)
   и компонент **SQL Server Express LocalDB** (ставится через Visual Studio Installer, обычно уже установлен).
2. Открыть `Lab_rab2_EDM_Shumihin_A.U_BPIz_23_02.slnx`; пакет `EntityFramework` скачается автоматически.
3. F5 → «Должности»: список загружается из БД (рисунок 6 методички); проверить добавление «Офис-менеджер»,
   переименование «Менеджер» → «Менеджер по продажам», удаление «Офис-менеджер» (рисунки 7–14).
4. «Сотрудники»: загрузка, добавление, редактирование, удаление (рисунки 15–23).
5. Саму БД можно посмотреть: Вид → Обозреватель объектов SQL Server → (localdb)\MSSQLLocalDB →
   Базы данных → CompanyEntities → Таблицы (рисунки 3–5).
6. Чтобы начать с исходных данных — удалить БД `CompanyEntities` там же и снова запустить программу.
