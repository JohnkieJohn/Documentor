   M S B A M L   `   `   `  ����E  ADocumentor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Documentor.ViewsDocumentor.Views  5      O,clr-namespace:Documentor.ViewModels.DocumentDocumentor.ViewModels.Document  64        Documentor.Views.Templates   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Documentor.Views     5      Adocument,clr-namespace:Documentor.ViewModels.Document     64   $��Stretch=�5      $��Stretch=�6%     N� 
Background  �	#FFD1D0D9 N� Title$ Gestionnaire de templates��5
      .��5      � .+�5   
   �� $��Center=�6   $	��Top=�61   $��20, 150, 0, 0q�6I   $(��"Bahnschrift SemiBold SemiCondensed)�6`   $��20(�6�   -+Bienvenue sur le gestionnaire de templates.6�   �� 5   
   $��Center=�6   $	��Top=�61   $��20, 200, 0, 0q�6I   $��Bahnschrift SemiCondensed)�6`   $��15(�6�   31Ici vous pouvez créer des templates de document,6�   �� 5   
   $��Center=�6   $	��Top=�61   $��20, 220, 0, 0q�6I   $��Bahnschrift SemiCondensed)�6`   $��15(�6�   53ou encore modifier un de vos templates enregistré.6�   �� 5   
   $
��Left=�6   $	��Top=�6.   $��265,460,0,0q�6F   $	��180��6[    �� DisplayMemberPath$
 Name��6�   ��6g   �� *DocumentsList+ �� Mode$ OneWay=� �� SelectedItem 6�   �� *SelectedDocument+$ TwoWay=��� 5   
   $��Charger le template��6   $
��Left=�6/   $	��Top=�6J      
NavButtons#��[  6b   $	��180��6�   $��265,500,0,0q�6�   ��6�   �� *DetailsView+�� 5   
   
���   �$��10q�6%   $"��Enregistrer nouveau template��6:   $
��Left=�6a   $	��Top=�6|   #��[  6�   $	��180��6�   $��520,500,0,0q�6�   ��6�   �� *NewDocument+6�   �� 5   
   $
��Left=�6   $	��Top=�6-   $	��180��6E   $��520,460,0,0q�6�   ��6Q   �� *TextFieldValue+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   $
��Left=�6   $	��Top=�6.   $��520,420,0,0q�6F   $	��180��6[    �� SelectedIndex 6g   �� *ComboBoxSelectedIndex+$ TwoWay=�$ PropertyChanged=�5!      5"      