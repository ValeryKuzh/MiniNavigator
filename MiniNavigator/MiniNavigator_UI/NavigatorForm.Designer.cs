using Infralution.Controls.VirtualTree;

namespace MiniNavigator_UI
{
    partial class NavigatorForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding1 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding2 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding3 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding4 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding5 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            this.NavigatorVirtualTree = new Infralution.Controls.VirtualTree.VirtualTree();
            this.Дерево = new Infralution.Controls.VirtualTree.ObjectRowBinding();
            this.colMain = new Infralution.Controls.VirtualTree.Column();
            this.rowBindingNavObjectDTO = new Infralution.Controls.VirtualTree.ObjectRowBinding();
            this.colName = new Infralution.Controls.VirtualTree.Column();
            this.colType = new Infralution.Controls.VirtualTree.Column();
            this.colChildren = new Infralution.Controls.VirtualTree.Column();
            this.navObjectDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navObjectDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // NavigatorVirtualTree
            // 
            this.NavigatorVirtualTree.Columns.Add(this.colMain);
            this.NavigatorVirtualTree.Columns.Add(this.colName);
            this.NavigatorVirtualTree.Columns.Add(this.colType);
            this.NavigatorVirtualTree.Columns.Add(this.colChildren);
            this.NavigatorVirtualTree.DataSource = this.navObjectDTOBindingSource;
            this.NavigatorVirtualTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorVirtualTree.Location = new System.Drawing.Point(0, 0);
            this.NavigatorVirtualTree.Name = "NavigatorVirtualTree";
            this.NavigatorVirtualTree.RowBindings.Add(this.Дерево);
            this.NavigatorVirtualTree.RowBindings.Add(this.rowBindingNavObjectDTO);
            this.NavigatorVirtualTree.Size = new System.Drawing.Size(785, 458);
            this.NavigatorVirtualTree.TabIndex = 0;
            // 
            // Дерево
            // 
            objectCellBinding1.Column = this.colMain;
            this.Дерево.CellBindings.Add(objectCellBinding1);
            this.Дерево.ChildProperty = "this";
            this.Дерево.Name = "Дерево";
            this.Дерево.TypeName = "System.Windows.Forms.BindingSource";
            // 
            // colMain
            // 
            this.colMain.AutoSizePolicy = Infralution.Controls.VirtualTree.ColumnAutoSizePolicy.AutoIncrease;
            this.colMain.Caption = "Item";
            this.colMain.DataField = "__MAIN__";
            this.colMain.Hidable = false;
            this.colMain.Movable = false;
            this.colMain.Name = "colMain";
            this.colMain.Width = 362;
            // 
            // rowBindingNavObjectDTO
            // 
            objectCellBinding2.Column = this.colMain;
            objectCellBinding2.Field = "Name";
            objectCellBinding3.Column = this.colName;
            objectCellBinding3.Field = "Name";
            objectCellBinding4.Column = this.colType;
            objectCellBinding4.Field = "Type";
            objectCellBinding5.Column = this.colChildren;
            objectCellBinding5.Field = "Children";
            this.rowBindingNavObjectDTO.CellBindings.Add(objectCellBinding2);
            this.rowBindingNavObjectDTO.CellBindings.Add(objectCellBinding3);
            this.rowBindingNavObjectDTO.CellBindings.Add(objectCellBinding4);
            this.rowBindingNavObjectDTO.CellBindings.Add(objectCellBinding5);
            this.rowBindingNavObjectDTO.Name = "rowBindingNavObjectDTO";
            this.rowBindingNavObjectDTO.TypedListName = "NavObjectDTO";
            // 
            // colName
            // 
            this.colName.Caption = "Name";
            this.colName.DataField = "Name";
            this.colName.Name = "colName";
            // 
            // colType
            // 
            this.colType.Caption = "Type";
            this.colType.DataField = "Type";
            this.colType.Name = "colType";
            // 
            // colChildren
            // 
            this.colChildren.Caption = "Children";
            this.colChildren.DataField = "Children";
            this.colChildren.Name = "colChildren";
            // 
            // navObjectDTOBindingSource
            // 
            this.navObjectDTOBindingSource.DataSource = typeof(MiniNavigator_UI.DTO.NavObjectDTO);
            // 
            // NavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 458);
            this.Controls.Add(this.NavigatorVirtualTree);
            this.Name = "NavigatorForm";
            this.Text = "MiniNavigator";
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navObjectDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infralution.Controls.VirtualTree.VirtualTree NavigatorVirtualTree;
        private System.Windows.Forms.BindingSource navObjectDTOBindingSource;
        private Column colMain;
        private Column colName;
        private Column colType;
        private Column colChildren;
        private ObjectRowBinding Дерево;
        private ObjectRowBinding rowBindingNavObjectDTO;
    }
}

