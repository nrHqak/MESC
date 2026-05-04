# Блок-схема логики проекта «МЭСК»

```mermaid
flowchart TD
    A([Старт приложения]) --> B[Program.cs: инициализация WinForms]
    B --> C[Проверка/создание папки Data]
    C --> D[Открыть LoginForm]

    D --> E{Действие пользователя}
    E -->|Регистрация| R1[Открыть RegisterForm]
    R1 --> R2[Валидация: пустые поля / длина пароля]
    R2 -->|Ошибка| R3[MessageBox с ошибкой]
    R2 -->|ОК| R4[UserManager.Register -> users.json]
    R4 --> R5{Пользователь уже существует?}
    R5 -->|Да| R3
    R5 -->|Нет| R6[Успешная регистрация]
    R6 --> D

    E -->|Вход| L1[Валидация логина/пароля]
    L1 -->|Ошибка| L2[MessageBox: заполните поля]
    L1 -->|ОК| L3[UserManager.Login -> users.json]
    L3 --> L4{Данные верны?}
    L4 -->|Нет| L5[MessageBox: неверные данные]
    L4 -->|Да| M0[Открыть MainForm]

    M0 --> M1[Инициализация менеджеров: Material/Drive/Upload]
    M1 --> M2[Построение UI: Sidebar + TopBar + Content]
    M2 --> M3[Показ Home]

    M3 --> NAV{Навигация Sidebar}
    NAV -->|Главная| H1[Hero + быстрый переход в материалы]
    NAV -->|Предметы| S1[Список предметов]
    S1 --> S2[Выбор предмета]
    S2 --> MAT
    NAV -->|Материалы| MAT[Материалы по фильтрам]
    MAT --> MAT1[Фильтр по классу 10/11/12]
    MAT1 --> MAT2[Поиск по title/subject]
    MAT2 --> MAT3[Отобразить список]
    MAT3 --> MAT4{Двойной клик по материалу}
    MAT4 -->|Локальный путь существует| MAT5[Process.Start(filePath)]
    MAT4 -->|HTTP/HTTPS| MAT6[Открыть ссылку в браузере]
    MAT4 -->|Иное| MAT7[Показать детали в MessageBox]

    NAV -->|Google Drive| G1[DriveLinkManager.GetAll -> drive_links.json]
    G1 --> G2[Показ карточек ссылок]
    G2 --> G3[Кнопка "Открыть" -> браузер]

    NAV -->|Загрузка файлов| U1[OpenFileDialog: pdf/docx/txt/pptx]
    U1 --> U2{Файл выбран?}
    U2 -->|Нет| NAV
    U2 -->|Да| U3[UploadManager.AddFile: проверка пути/расширения]
    U3 --> U4{Валидный файл?}
    U4 -->|Нет| U5[MessageBox: ошибка загрузки]
    U4 -->|Да| U6[Выбор предмета + ввод названия]
    U6 --> U7[UploadStorageManager.Add -> uploads.json]
    U7 --> U8[MaterialManager.AddMaterial -> materials.json]
    U8 --> U9[Обновить таблицу загрузок]

    NAV -->|Помощь| HP[Показ инструкций по использованию]
    NAV -->|О программе| AB[Показ информации о версии]

    M2 --> T1[Кнопка Light/Dark]
    T1 --> T2[ThemeManager.Toggle + рекурсивный Apply]

    M0 --> X{Закрытие MainForm}
    X --> Y([Конец])
```
