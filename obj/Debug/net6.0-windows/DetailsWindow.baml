   M S B A M L   `   `   `  ����E  ADocumentor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Documentor
Documentor  5   	       Documentor.DetailsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Documentor     5   	   ] YXceed.Wpf.Toolkit.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          �Name$  detailsWindow��5
   	    � Title 5	   	   �� *Name+.��5      �  � ColumnDefinitions 5   
   �� 5      $��0.05* �6   �� 5      $��0.35* �6   �� 5      $��0.35* �6   �� 5      $��0.25* �6   .+�5   
    � -   5          �Name$ ActionFrame��5      $��0��5        � NavigationUIVisibility$ Hidden=�5       � 5   
   -   5        $ DocumentFrame��5      $ Hidden=�5      $��1��5      $��2��5       � 5   
   -   5        $ PropertiesFrame��5      $��3��5      $ Hidden=�5      �� 5   
   $��précédent��6   $��1��5       $��Bottom=�5!      $��Right=�5"      $��10(�5#         
NavButtons#��[  5$      �� 5%   
   $��suivant��6   $��2��5&      $��Bottom=�5'      $
��Left=�5(      $��10(�5)      #��[  5*      �� 5+   
   $	����6   $��Segoe MDL2 Assets)�5,      $��0��5-      $��Bottom=�5.      $��Center=�5/      $��0,0,0,50q�50      
���  ��$��	ExtraBold"�52      #��[  53      54      55      