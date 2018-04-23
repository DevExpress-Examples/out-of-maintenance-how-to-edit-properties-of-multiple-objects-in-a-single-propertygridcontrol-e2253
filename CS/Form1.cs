using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraVerticalGrid.Events;

namespace CutomProperties {
    public partial class Form1 : Form {
        List<PropertyDescriptor> propertyStore = new List<PropertyDescriptor>();
        public Form1() {
            InitializeComponent();
            propertyStore.Add(new CustomPropertyDescriptor(this, "Form's Size", "Size"));
            propertyStore.Add(new CustomPropertyDescriptor(this, "Form's Title", "Text"));
            propertyStore.Add(new CustomPropertyDescriptor(this.propertyGridControl1, "PropertyGridControl's RecordWidth", "RecordWidth"));
            propertyStore.Add(new CustomPropertyDescriptor(this.button1, "Button's Visibility", "Visible"));
            this.propertyGridControl1.SelectedObject = propertyStore;
        }

        void propertyGridControl1_CustomPropertyDescriptors(object sender, CustomPropertyDescriptorsEventArgs e) {
            if(e.Source == propertyStore) {
                PropertyDescriptorCollection rootProperties = new PropertyDescriptorCollection(null);
                foreach(PropertyDescriptor pd in propertyStore) {
                    rootProperties.Add(pd);
                }
                e.Properties = rootProperties;
            }
        }
    }
    class CustomPropertyDescriptor : PropertyDescriptor {
        string name;
        PropertyDescriptor sourcePropertyDescriptor;
        object source;

        public CustomPropertyDescriptor(object source, string name, string targetPath) : base(name, null) {
            this.name = name;
            this.source = source;
            this.sourcePropertyDescriptor = TypeDescriptor.GetProperties(source)[targetPath];
            if(SourcePropertyDescriptor == null)
                throw new Exception("Can't bind to the source with the " + targetPath + " property");
        }

        public override string Name { get { return name; } }
        public override Type ComponentType { get { return SourcePropertyDescriptor.ComponentType; } }
        public override bool IsReadOnly { get { return SourcePropertyDescriptor.IsReadOnly; } }
        public override Type PropertyType { get { return SourcePropertyDescriptor.PropertyType; } }

        PropertyDescriptor SourcePropertyDescriptor { get { return sourcePropertyDescriptor; } }
        object Source { get { return source; } }

        public override object GetValue(object component) {
            return SourcePropertyDescriptor.GetValue(Source);
        }
        public override bool CanResetValue(object component) {
            return SourcePropertyDescriptor.CanResetValue(Source);
        }
        public override void ResetValue(object component) {
            SourcePropertyDescriptor.ResetValue(Source);
        }
        public override void SetValue(object component, object value) {
            SourcePropertyDescriptor.SetValue(Source, value);
        }
        public override bool ShouldSerializeValue(object component) {
            return SourcePropertyDescriptor.ShouldSerializeValue(Source);
        }
    }
}
