   M S B A M L   `   `   `  ���� S  OWpf_AeroSphere_test_task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Wpf_AeroSphere_test_taskWpf_AeroSphere_test_task  )    #Wpf_AeroSphere_test_task.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:Wpf_AeroSphere_test_task     -     � Title$  File Manager AeroSphere��$	��450��$	��800�� � 	Resources    BigFont_TextBlock_Style �� Property �� Value  Button_Arrows_Style  TextBlock_Low_Font_info��%3  &	        &	 Y     &	 �     �� .���  � ��$ Times New Roman)���  � ��$
 Bold"���  � ��$ 18(��� .���  � �� �	#FFFFDBA1��  � ��$ 0q���  � ��$ 18(���  � �� �#BF8930�� .���  � ��
 �������  � ��$ 12(���  � ��$ Light"�.���  � RowDefinitions �� $��25���� .+�� $��0�����	#FFFFDBA1.+�]� ��. .+��� $
��Left=� �� Orientation$ 
Horizontal=�.+��� -   #��[  � ToolTip$= 7Вернуться на директорию назад��.��🡄�� -   #��[ 
����  �
�������$3 -Вернуться к выбору диска��$��Black"�.��⬆�� $��Right=�$��Right=�$	��150��$��2q�.��Search: 🔎� $��5,0,5,0q� � ColumnDefinitions �� $
��auto ��� $��* �.+�� $��0�����#BF8930.+��� #��[  ����  �� EndPoint$ 0.5,1&�	 � MappingMode$	 RelativeToBoundingBox=�
 �� 
StartPoint$
 0.5,0&�.���  � Color$ 	#FFFF8B00�� � Offset$ 0.414Q�� $ 	#FFFFEFA1��$ 0.873Q�� $ 	#FFFFEFA1��$ 0.438Q�Path: 📁 �� 
�������.�❯� $��1��.+��� -   $��txt_box_Path��$��Stretch=�#��[  $��Italic$� �� 
IsReadOnly . .��💻MyComputer� -    �Name$ Grid_drives��$��1��
������� �� $��* ��� $
��Auto ��� $
��Auto �.+�� $��0��.+��� -   $��group_box_drives��$��All available Drives��#��[  .���� -   $��list_view_disks�� �� SelectionMode$ Single=�
����������� .R��� $ Vertical=�$��5q�.+��� $ 
Horizontal=�.+��� $
��Left=�$��50��$��50������ *Img+�� #��[  $��0,0,5,0q����� *VolumeLabel+�� #��[  $��Light"����� *Name+ �� Mode$ OneWay=�� $
��Left=�
��� � �
�������$��10��$��80�� � Minimum$ 0Q� J� RenderTransformOrigin$ 0.5,0.5&� � Maximum �� *	TotalSize+$ OneWay=� � Value �� *AvailableFreeSpace+$ OneWay=�z�V� .~��� �� ��  �� Angle$	 180Q�T� �� $ 
Horizontal=�.+��� 2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve#��[ ���� *AvailableFreeSpace+Gb is free from ��  preserve#��[ ���� *	TotalSize+ Gb�� $��1��$g aВы можете регулировать размеры левого и правого окна�� �� ShowsPreview .  $��5��$��Center=�$��Stretch=����#A66800� $��2��
�������.+�&   System.Windows.Controls.DataGrid  -     Name$ data_grid_disks_meta_data��$y�	Collapsed=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	Attribute��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+$ OneWay=�  $ Value�� .  �� *Value+$ OneWay=�� -	   $ Grid_files_and_folders��$��1��
�������$y�	Collapsed=� �� $��* ��� $
��Auto ��� $
��Auto �.+�� $��0��.+��� -
   $��group_box_files��$��Files and Folders��#��[  .���� -    ��Name$ list_view_files��
�������#��[  ���� .R��� $ 
Horizontal=�.+��� ���� *Ico+�� ���� *Name+�� $��1��$g aВы можете регулировать размеры левого и правого окна�� .  $��5��$��Center=�$��Stretch=����#A66800� $��2��
�������$	��400��.+�  -   $ data_grid_files_meta_data��$y�	Collapsed=�   $ 	Attribute�� .  �� *Name+$ OneWay=�  $ Value�� .  �� *Value+$ OneWay=�