﻿#pragma checksum "..\..\..\..\Services\UserControl\IPBox.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D25885ACE41238F5C89D547AA1FA560A31D6F216"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace IPBox {
    
    
    /// <summary>
    /// IPTextBox
    /// </summary>
    public partial class IPTextBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 49 "..\..\..\..\Services\UserControl\IPBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FirstSegment;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Services\UserControl\IPBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SecondSegment;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Services\UserControl\IPBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ThirdSegment;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Services\UserControl\IPBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LastSegment;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NamenschildDesktop;component/services/usercontrol/ipbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Services\UserControl\IPBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.FirstSegment = ((System.Windows.Controls.TextBox)(target));
            
            #line 50 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.FirstSegment.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxBase_OnTextChanged);
            
            #line default
            #line hidden
            
            #line 50 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.FirstSegment.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UIElement_OnPreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 50 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.FirstSegment.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.DataObject_OnPasting));
            
            #line default
            #line hidden
            return;
            case 2:
            this.SecondSegment = ((System.Windows.Controls.TextBox)(target));
            
            #line 55 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.SecondSegment.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxBase_OnTextChanged);
            
            #line default
            #line hidden
            
            #line 55 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.SecondSegment.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UIElement_OnPreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 55 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.SecondSegment.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.DataObject_OnPasting));
            
            #line default
            #line hidden
            return;
            case 3:
            this.ThirdSegment = ((System.Windows.Controls.TextBox)(target));
            
            #line 60 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.ThirdSegment.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxBase_OnTextChanged);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.ThirdSegment.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UIElement_OnPreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.ThirdSegment.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.DataObject_OnPasting));
            
            #line default
            #line hidden
            return;
            case 4:
            this.LastSegment = ((System.Windows.Controls.TextBox)(target));
            
            #line 65 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.LastSegment.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxBase_OnTextChanged);
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.LastSegment.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UIElement_OnPreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\..\Services\UserControl\IPBox.xaml"
            this.LastSegment.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.DataObject_OnPasting));
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

