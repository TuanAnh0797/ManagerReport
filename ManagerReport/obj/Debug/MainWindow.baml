   M S B A M L   `   `   `  ����H  DManagerReport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ManagerReportManagerReport  5   	   I)clr-namespace:ManagerReport.MyUserControlManagerReport.MyUserControl  5	   	       ManagerReport.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ManagerReport     5   	   _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   9	Mycontrol)clr-namespace:ManagerReport.MyUserControl  5	   	   -   5        $��Wd_MainWindow��5   	      MaterialDesignBody#���   5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	   
�������  MaterialDesignFont#���  5   	    � Title$ 
MainWindow��5   	   $	��450��6   $	��800��6)    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	    � 	Resources 5       �� 
TargetType �� Property �� Value0   *ManagerReport.StringToImageSourceConverter   StringToImageSourceConverter��%�   '��       &	 s     �� 5   
   " ��6   .�5      ��  � ��
 �   �6;   �� 5       � ��$ 20(�6)   66   5        5   
   6T   5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $
��auto �6   6,   �� 5       6   .+�5"   
   2   ,ManagerReport.MyUserControl.MyControlBarxaml  6'   � 5#   
   $��1��6   $��0,0,0,0q�6   	 5$      �� 5%      $
��auto �6    60   �� 5&      6"   .+�5(      � $��0��6   
�������$��0,1,0,0q�68   .+�5)      �� $
��auto��6   $
��Left=�6+   $��
40,10,0,10q�6F   
 �� Orientation$
 
Horizontal=�6Z   .+�5*      �� $��Hãy đọc QRCode��6    $��Center=�66   $��0,0,10,0q�6Q   6e   �� 5+      -   5        $��
txb_QRCode��5+   ;   $��0��6   $��20(�6-    �� TextAlignment$ Center=�6M   $	��350��6e   $��Center=�6q   6�   �� 5,      $��	20,0,10,0q�6    $,��&Công đoạn cần nhập dữ liệu��63   $��Center=�6U   6s   �� 5-      -   5        $��cmb_line��5-      $��25(�6/   $
��Bold"�6=   $
��auto��6O   $	��250��6]    " MaterialDesignOutlinedComboBox#��[ 6i   6�   �� 5.      $��	20,0,20,0q�6    $��Tháng nhâp dữ liệu��63   $��Center=�6M   6k   �� 5/      -   5        $��	cmb_month��5/      $��25(�60   $
��Bold"�6>   $
��auto��6P   $	��100��6^   #��[ 6j   6�   �� 50      $��	20,0,20,0q�6    $#�� Năm cần nhâp dữ liệu��63   $��Center=�6P   6n   �� 51      -   5        $��cmb_year��51       $��25(�60   $
��Bold"�6>   $
��auto��6P   $	��100��6^    �� TextAlignment$ Center=�6j   $��Center=�6�   $��Center=�6�   56      57      � 59      $��1��6   
�������$��0,1,0,0q�63    � ColumnDefinitions 5:      �� 5;      $��* �6'   63   �� 5<      $
��auto �6'   66   �� 5=      $
��auto �6'   66   .+�5?      �� $��0��6   
���   �$��1q�6=   $��2q�6Q   ._�5@      � 	 5A      �� 5B      $
��auto �6,   6<   �� 5C      $
��auto �6,   6<   �� 5D      $��* �6,   69   .+�5F      �� 
���Gc��$ Center=�68   $��0��6P   $��Camera��6]   $��20(�6k   $��Stretch=�6y   $��Stretch=�6�   6�   �� 5H      -   5         ��Name$ myImage��5H   L   $��2��6    $��10q�6.   $��Uniform=�6:    J� RenderTransformOrigin$ 0.5,0.5&�6p   ��6]   �� 5I      5J      5K      �� 5L      $��1��6   
���   �$��1q�6=   $��2q�6Q   ._�5M      � 	 5N      �� 5O      $
��auto �6,   6<   �� 5P      $
��auto �6,   6<   �� 5Q      $��* �6,   69   .+�5T      �� $ Center=�6$   
���Gc��$��0��6O   $#��Danh sách ảnh đã chụp ��6\   $��Stretch=�6z   $��Stretch=�6�   $��20(�6�   6�   �� 5V      $��2��6'   .��5W      � .+�5X   "   �� -   5         ��Name$ 	imageList��5X   N   ��6+   �� *	listimage+��5Y   &   �� 5Z   *   " ��60   .�5[   .   ��  � ��$	 600��6I   6W   5\   +   ��5^   &   �� 5_   *   .R�5`   .   �� 
�������$��1q�6I   $��10q�6]   ._�5a   2   � 	 5b   6   �� 5c   :   $
��auto �6H   6X   �� 5d   :   $��* �6H   6U   .+�5f   6   �� $��0��6@   $ Center=�6N   $��Center=�6e   $��Center=�6�   $��20(�6�   ��6�   �� *	Nameimage+ �� Mode$ OneTime=�6�   �� 5g   6   $��1��6<   $��Uniform=�6I   $��5q�6�   $��Center=�6�   $��Center=�6�   ��6[   �� *Pathnameimage+ �� 	Converter# [ 6�   5h   3   5i   /   5j   +   5l   #   5m      5n      5p      5r      �� 5s      $��2��6   
���   �$��1q�6=   $��2q�6Q   ._�5t      � 	 5u      �� 5v      $
��auto �6,   6<   �� 5w      $
��auto �6,   6<   �� 5x      $��* �6,   69   �� 5y      $
��auto �6,   6<   .+�5{      ��  �� Padding$ 3,0,3,0q�6$   $ Center=�66   
���Gc��$/��)Danh sách bản thông số cần chụp��6a   $��Stretch=�6�   $��Stretch=�6�   $��20(�6�   6�   �� 5}      -   5        $��listimagename��5}   W   $��2��6J   $��Visible=�6l   $��Visible=�6�   ��6#   �� *listchecksheet+��5~      �� 5   "   " ��6(    �� Triggers 5�   &   �� 5�   *    �� Binding 66   �� *
StatusDone+ �� UpdateSourceTrigger$ PropertyChanged=� �� Value True6z   .a�5�   .   ��  � ��
 � � �6[   5�   +   �� 5�   *    66   �� *
StatusDone+$ PropertyChanged=�	 False6z   .a�5�   .   ��  � ��
 �  ��6Y   5�   +   5�   #   ��5�      �� 5�   "   .R�5�   &   � 	 5�   *   �� 5�   .   $
��auto �6<   6L   .+�5�   *   �� $��0��64   $ Center=�6B   $��Center=�6Y   $��Center=�6t   $��20(�6�   ��6�   �� *NameCheckSheet+5�   +   5�   '   5�   #   5�      �� 5�      $��Right=�6%   $��0,0,5,0q�6A   $��3��6R   $
 
Horizontal=�6_    5�      ��%W   '��       �� 5�   "   " ��6(   .�5�   &   ��  � ��$ 20(�6A   6N   5�   #   5�      .+�5�      �� $��Hoàn thành: ��6(   6>   �� 5�      ��6(   �� *FinishCheckSheet+6L   �� 5�      $��/��6(   63   �� 5�      ��6(   �� *ToltalCheckSheet+6M   5�      5�      5�      5�      5�      5�      5�      