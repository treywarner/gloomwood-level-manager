   M S B A M L   `   `   `  ����R  NGloomwood Level Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Gloomwood_Level_ManagerGloomwood_Level_Manager  5   	   N JMdXaml, Version=1.21.0.0, Culture=neutral, PublicKeyToken=9f8c7afb435b7edc/$clr-namespace:MdXaml;assembly=MdXamlMdXaml 5   	   (    "Gloomwood_Level_Manager.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:Gloomwood_Level_Manager     5   	   6mdxam$clr-namespace:MdXaml;assembly=MdXaml     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Stretch=�6    �� Orientation$ 
Horizontal=�63   .+�5      �� -   5        $��Download Maps��5      $
��Left=�6J   
�������
������ 6x   �� 5      -   5        $��
Create Map��5      $
��Left=�6E   
�������
������ 6s   �� 5      -   5        $��Settings��5      $
��Left=�6E   
�������
������ 6s   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      �� -   5        $��levelListBox��5      $��2��6*    � ContextMenu 5      �� 5       .@�5!      m� -   5        $��Delete��5!   #   63   5"      ��5$      �� 5%      .R�5&      �� $ 
Horizontal=�6%   $��10q�6>   .+�5'      �� $��32��6=   $��32��6H   $��Center=�6T   ��6$   �� *Photo+6T   �� 5(      $��10,0,0,0q�6)   .+�5)   "   �� $
��Bold"�6C   ��6,   �� *Title+6C   �� 5*   "   $
��Wrap=�6I   ��6,   �� *Description+6I   5+      5,      5-      50      � 52      $��1��6    53      �� 54      $��* �6$   �� 55      $
��Auto �6$   .+�58      !  MdXaml.MarkdownScrollViewer  2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve6m   -   5          Name$ readmeTextBlock��58   B   $��10,0,0,0q�6[   $��1��6m   �� 5:      -   5         ��Name$ 	LoadPanel��5:      $ 
Horizontal=�60   $��0,0,0,10q�6I   $��1��6[   $y�	Collapsed=�6h   .+�5;      �� -   5         ��Name$ loadFileComboBox��5;      $��	10,10,0,0q�69   	 �� SelectedIndex$	 0��6L   .@�5<      �� $��Save 0��6'   �� 5=      $��Save 1��6'   �� 5>      $��Save 2��6'   �� 5?      $��Save 3��6'   �� 5@      $��Save 4��6'   �� 5A      $��Save 5��6'   �� 5B      $��Save 6��6'   �� 5C      $��Save 7��6'   �� 5D      $��Save 8��6'   �� 5E      $��Save 9��6'   5G      �� 5J      -	   5        $
��Load��5J      $��	10,10,0,0q�6E   $��1��6X   �� 5K      -
   5        $��Load and Launch��5K      $��	10,10,0,0q�6V   $��1��6i   5L      5M      5N      5O      5P      