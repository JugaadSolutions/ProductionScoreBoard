   M S B A M L   `   `   `  ����  PSB_OperatorApp)    #PSB_OperatorApp.Views.DashboardView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6tk-http://schemas.microsoft.com/wpf/2008/toolkit  5      
��� � �$��Stretch=�5   !   $��Stretch=�6?     � 	Resources  5	      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value��%V   '        �� 5
   
   "  6   .�5      ��  � ��$ Center=�64   5      5      .��5      �� -   5        $��BaseTab��5      
��� � �.@�5   
   �� $��Actual��6   .��5      �  � RowDefinitions 5      �� 5      $
��0.3* �6$   �� 5      $��* �6$   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Stretch=�66   $��5q�6T   .+�5      �� $��
Reference:��6   $��Right=�61   $��Right=�6M   $��20(�5      
�������$	��120��60   $��Center=�6?   �� 5      -   5        $��ActualReferenceTextBox��5      $	��200��6<   $��20(�6K   $
��Left=�6Y   $
��Left=�5      $��Center=�68   6S   �� 5      $��Unit:��6   $��Right=�6,   $��Right=�6H   $��20(�5       
�������$	��120��60   $��Center=�6?   $	��0,2q�6Z   �� 5!      -   5        $��SerialNoTextBox��5!      $	��200��65   $��20(�6D   $
��Left=�6R   $
��Left=�5"      $��Center=�68   6T   5*      �� 5,      $ 
Horizontal=�6   $��Center=�66   $��5q�6S   5/      �� 50      $��1��6   
�������$��1q�6<   $��10,5,5,5q�6P   ��51      �� 52      $��Actual��6    $��18(�61   
�������6?   .��54      � .+�57      )  #Microsoft.Windows.Controls.DataGrid  -   5        $��
ActualGrid��57   4   $��1��6&      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  	   
IsReadOnly	 . 
   CanUserReorderColumns
 .     CanUserSortColumns .  $	��100��59      $��5q�6&   
��� � �
���   �$��15(�6W   $��Center=�6}   $	��400��6�   $��1q�5:      
���������59   e   ��    Columns 5;      3  -Microsoft.Windows.Controls.DataGridTextColumn  5<   "   /  )Microsoft.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$ 	Serial No��6J   4  .Microsoft.Windows.Controls.DataGridBoundColumn   Binding 6]   �� *
SerialNo+  5=   "    . $ 	Reference��6J    6]   �� *	Reference+  5>   "    . $ Start��6J    6Y   �� *StartTimestamp+  5?   "    . $	 End��6J    6W   �� *EndTimestamp+5B      5C      5D      5E      5F      �� 5G   
   $
��Plan��6   .��5H      �  5I      �� 5J      $
��Auto �6$   �� 5K      $��* �6$   .+�5M      � $��0��6   $��Center=�6$    5N      �� 5O      �� 5P      .+�5T      �� $ 
Horizontal=�6!   $��Stretch=�6:   $��5q�6X   .+�5U      �� $��
Reference:��6    $��Right=�65   $��Right=�6Q   $��20(�5V      
�������$	��120��60   $��Center=�6?   �� 5W      -   5        $��ReferenceTextBox��5W   "   $	��250��6:   $��18(�6I   $
��Left=�6W   $
��Left=�5X      $��Center=�68   �� 5Y      $��
Operators:��6    $��Right=�65   $��Right=�6Q   $��20(�5Z      
�������$	��120��60   $��Center=�6?   �� 5[      -   5        $��OperatorTextBox��5[   "   $��60��69   $��18(�6G   $
��Left=�6U   $
��Left=�5\      $��Center=�68   �� 5]      $��Target Qty:��6    $��Right=�66   $��Right=�6R   $��20(�5^      
�������$	��120��60   $��Center=�6?   �� 5_      -   5        $��TargetQtyTextBox��5_   "   $��60��6:   $��20(�6H   $
��Left=�6V   $
��Left=�5`      $��Center=�68   5c      �� 5e      $��1��6!   $ 
Horizontal=�6.   $��Stretch=�6G   $��5q�6e   5g      �� 5i      $��2��6!   $ 
Horizontal=�6.   $��Stretch=�6G   $��5q�6e   5k      �� 5o      $��3��6!   $ 
Horizontal=�6.   $��Center=�6G   .+�5p      �� -   5        $��AddPlanButton��5p   #   $	��Add��68   $��5q�6F   $��80��6Q   $��20(�6_   $��Center=�6m   $��Center=�6�   $��Center=�5q       J� Effect 5r      3  -System.Windows.Media.Effects.DropShadowEffect  5s   "      Color$ Black��63      	Direction$	 320Q�6A      ShadowDepth$ 6Q�6Q      
BlurRadius$ 5Q�6a      Opacity$	 0.5Q�6p   5u      �� 5v      -	   5        $��ClearButton��5v   /   $��3��6!   $��Clear��6B   $��5q�6R   $��20(�6]   $��80��6l   $��Right=�6z   $��Center=�6�   $��Center=�5w       5x        5y   "   $ Black��63   $	 320Q�6A   $ 6Q�6Q   $ 5Q�6a   $	 0.5Q�6p   5{      5|      5      �� 5�      $��1��6   
�������$��1q�6<   $��10,5,5,5q�6P   ��5�      �� 5�      $
��Plan��6    $��18(�6/   
�������6=   .��5�      �  5�      �� 5�      $��* �6,   �� 5�      $
��Auto �6,   .+�5�        -
   5        $��PlanGrid��5�   '    .   .   .  
 .   .  $	��100��5�      $��5q�6&   
��� � �
���   �$��15(�6W   $��Center=�6}   $	��400��6�   $��1q�5�      
�������$ �� IsSynchronizedWithCurrentItem$
 trueY�6?   ��5�   e   ��  5�        5�   "    . $ 	Reference��6J    6]   �� *	Reference+  5�   "    . $ 
Quantity  ��6J    6^   �� *
Quantity+  5�   "    . $ 	Operators��6J    6]   �� *	Operators+5�      �� 5�      -   5        $��ActivateButton��5�   /   $��3��6!   $��Start��6E   $��5q�6U   $��15(�6`   $��80��6o   $��Center=�6}   $��Center=�5�   !   $��Center=�6<    5�        5�   "   $ Black��63   $	 320Q�6A   $ 6Q�6Q   $ 5Q�6a   $	 0.5Q�6p   5�      5�      5�      5�      5�      5�      5�      