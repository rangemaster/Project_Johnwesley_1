﻿

#pragma checksum "C:\Users\Roel Suntjens\Documents\GitHub\Project_Johnwesley_1\Project_Johnwesley\Project_Johnwesley_Part1\Project_Johnwesley_Part1.Windows\BillPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DD6D9EC619740356FC4D5ADF4061104D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_Johnwesley_Part1
{
    partial class BillPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 78 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyDown += this._Edit_Field_tx_KeyDown;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 80 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Edit_Flyout_Continue_bn;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 81 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Close_Flyout_Continue_bn;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 70 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Add_bn_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 71 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Edit_bn_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 72 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Remove_bn_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 66 "..\..\BillPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.Bill_List_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


