# МЭСК (MESC)

Современная WinForms платформа подготовки к экзаменам для 10-12 классов.

## Функции
- Авторизация и регистрация с JSON-хранилищем.
- Dashboard с sidebar, поиском и фильтрацией по классам.
- Раздел материалов (теория/конспекты/практика/рекомендации).
- Открытие Google Drive ссылок через браузер.
- Загрузка файлов (PDF/DOCX/TXT/PPTX).
- Сохранение загруженных файлов в `Data/uploads.json` (история загрузок).
- Светлая/тёмная тема.

## Структура
- `MESC/Forms` — формы приложения.
- `MESC/Models` — модели предметной области.
- `MESC/Managers` — бизнес-логика.
- `MESC/Data` — JSON-данные.
- `MESC/Docs` — UML/ERD/блок-схема.

## Запуск
1. Открыть `MESC.sln` в Visual Studio 2019+.
2. Запустить проект `MESC`.

## Где настраивать Google Drive ссылки
- Добавляйте/редактируйте любое количество ссылок в `MESC/Data/drive_links.json`.
- Раздел `Google Drive` в приложении автоматически читает этот файл.

## UML диаграмма классов
```mermaid
classDiagram
class User {+Username +Email +Password}
class Subject {+Name +Description +Grade}
class MaterialItem {+Title +Subject +Grade +Theory +Notes +Practice +Recommendations +UsefulLinks}
class DriveLink {+Title +Description +Url}
class UserManager {+GetAllUsers() +Register(User) +Login(string,string)}
class MaterialManager {+GetAll() +GetByGrade(int) +Search(string,int) +Recommend(int)}
class UploadManager {+UploadedFiles +AddFile(string)}
class ThemeManager {+Toggle(Form) +Apply(Control)}
UserManager --> User
MaterialManager --> MaterialItem
```
