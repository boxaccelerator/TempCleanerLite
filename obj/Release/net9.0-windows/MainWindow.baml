   M S B A M L   `   `   `  ����   TempCleanerLite     TempCleanerLite.MainWindow   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  TEMP Folder Cleaner��$	��400��$	��400�� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������ .��� .+��� -   $��BackgroundCanvas�����	#F01E1E1E.+�A� -   $��Circle1��$	��200��$	��200�� J� Opacity$	 0.5Q����� -   Circle1Brush�� �� Color$
 Blue�� J� Effect -  'System.Windows.Media.Effects.BlurEffect     Radius$ 50Q�A� -   $��Circle2��$	��200��$	��200��$	 0.5Q����� -   Circle2Brush��$	 Red��   $ 50Q�A� -   $��Circle3��$	��200��$	��200��$	 0.5Q����� -   Circle3Brush��$ Green��   $ 50Q��� -   ���	#C02D2D30$��30��$	��Top=�._�� .+���  �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��TEMP Cleaner��
�������$
��Left=�$��Center=�$	��2,0q��� $��v2.2.3��
�������$��Right=�$��Center=�$	��2,0q��� $ 
Horizontal=�$��Right=�$��Center=�.+��� -	    ��Name$ MinimizeButton��$�� ��$��30�����	#80FFFF00
�������$��0q��� -
   $ CloseButton��$�� ��$��30�����	#80FF3030
�������$��0q��� $��Center=�$��Center=�$	��350��$��0,40,0,0q�.+��� -   	 ��Name$!	 CleanAllTempFoldersCheckBox��$��Clean All Temp Folders��$��10q�
��������� -   $ CleanButton��$��Clean TEMP Folders��$��Center=�$��10q�� .+�� -   
 �Name$
 OverallProgressBar�� � Minimum$ 0Q� � Maximum$	 100Q�$��20��$
��10,2q��� -    ��Name$ OverallProgressText��$��Center=�$��Center=�
�������� .+�� -   $
 CurrentFolderProgressBar��$ 0Q�$	 100Q�$��20��$
��10,2q��� -   $ CurrentFolderProgressText��$��Center=�$��Center=�
��������� -   $ InfoText��$��--- Folders, --- items��$
��10,2q�
��������� -   $ 
StatusText��$
��Left=�$
��10,2q� �� TextAlignment$
 Left=�
�������$%��Hello! Click Clean TEMP Folders��