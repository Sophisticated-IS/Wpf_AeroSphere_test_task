# Wpf_AeroSphere_test_task
Реализовать файловый менеджер, отображающий файловую систему, на языке C#.
Графический интерфейс приложения должен быть написан на WPF и быть интуитивно понятным.
Файловый менеджер обладает следующими особенностями:

* Состоит из одной области("рабочая область"), в которой отображается содержимое текущей директории (или папки)

* При двойном нажатии на элемент списка содержимого:

а) если это файл, то приложение пытается его открыть в Windows;

б) если это папка, то в рабочую область загружается уже содержимое данной папки

* При одинарном нажатии на элемент списка содержимого, в правой стороне рабочей

области должна появляться панель, в которой отображается дополнительная информация:

а) если это файл, то его метаданные(размер, дата создания, ...);

б) если это папка, то размер и количество файлов в ней

Усложнения (Усложнения можно делать или не делать, на свой выбор)

1) Реализовать в верхней части рабочей зоны путь в иерархии папок. При нажатии на папку

в иерархии пути, происходит переход в данную папку

2) Реализовать функцию поиска по имени. Добавлется область (или окно на свое усмотрение),

в которой при вводе текста появляется список релеватных файлов в данной папке.

При нажатии на файл приложение пытается его открыть в Windows.
# Wpf_AeroSphere_test_task О проекте
1).Net Framework 4.7.2
2)Доп библиотеки не используются
3)Задание реаллизовано со всеми усложнениями

# Wpf_AeroSphere_test_task Особенности
1) Для интерфейса есть подсказки (Tooltips)
2) В начале пользователь находится в корневой директории "MyComputer", в которой можно выбрать нужный диск и перейти к файлам
3) Чтобы перейти к выбору приводу, нужно нажать стрелочку вверх в левом верхнем углу
4) Есть контекстное меню в папке с файлами, чтобы можно было обновить папку вручную.
# Wpf_AeroSphere_test_task Рассмотренные Доп ситуации
1)Если флешка или другое переносимое ЗУ было удалено или подключено (выход к выбору диска)
2)Если файл или папка были удалены (выход к первой существующей папке на обратном пути)
3)Если у пользователя нет доступа к папке (вывод сообщения об этом)
