using Infralution.Controls.VirtualTree;

namespace MiniNavigator_UI
{
    partial class NavigatorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding1 = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            this.colMain = new Infralution.Controls.VirtualTree.Column();
            this.NavigatorSplitContainer = new System.Windows.Forms.SplitContainer();
            this.NavigatorVirtualTree = new Infralution.Controls.VirtualTree.VirtualTree();
            this.navObjectDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rowBindingNavObjectDTO = new Infralution.Controls.VirtualTree.ObjectRowBinding();
            this.NavigatorDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).BeginInit();
            this.NavigatorSplitContainer.Panel1.SuspendLayout();
            this.NavigatorSplitContainer.Panel2.SuspendLayout();
            this.NavigatorSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navObjectDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // colMain
            // 
            this.colMain.AutoSizePolicy = Infralution.Controls.VirtualTree.ColumnAutoSizePolicy.AutoIncrease;
            this.colMain.Caption = "Объекты";
            this.colMain.DataField = "__MAIN__";
            this.colMain.Hidable = false;
            this.colMain.Name = "colMain";
            this.colMain.Width = 362;
            // 
            // NavigatorSplitContainer
            // 
            this.NavigatorSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.NavigatorSplitContainer.Name = "NavigatorSplitContainer";
            this.NavigatorSplitContainer.SplitterMoved += NavigatorSplitContainer_SplitterMoved;
            // 
            // NavigatorSplitContainer.Panel1
            // 
            this.NavigatorSplitContainer.Panel1.Controls.Add(this.NavigatorVirtualTree);
            // 
            // NavigatorSplitContainer.Panel2
            // 
            this.NavigatorSplitContainer.Panel2.Controls.Add(this.NavigatorDataGridView);
            this.NavigatorSplitContainer.Size = new System.Drawing.Size(785, 458);
            this.NavigatorSplitContainer.SplitterDistance = 255;
            this.NavigatorSplitContainer.TabIndex = 1;
            // 
            // NavigatorVirtualTree
            // 
            this.NavigatorVirtualTree.Columns.Add(this.colMain);
            this.NavigatorVirtualTree.DataSource = this.navObjectDTOBindingSource;
            this.NavigatorVirtualTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorVirtualTree.Location = new System.Drawing.Point(0, 0);
            this.NavigatorVirtualTree.Name = "NavigatorVirtualTree";
            this.NavigatorVirtualTree.RowBindings.Add(this.rowBindingNavObjectDTO);
            this.NavigatorVirtualTree.Size = new System.Drawing.Size(255, 458);
            this.NavigatorVirtualTree.TabIndex = 0;
            // 
            // navObjectDTOBindingSource
            // 
            this.navObjectDTOBindingSource.DataSource = typeof(MiniNavigator_UI.DTO.NavObjectDTO);
            // 
            // rowBindingNavObjectDTO
            // 
            objectCellBinding1.Column = this.colMain;
            this.rowBindingNavObjectDTO.CellBindings.Add(objectCellBinding1);
            this.rowBindingNavObjectDTO.Name = "rowBindingNavObjectDTO";
            this.rowBindingNavObjectDTO.TypedListName = "NavObjectDTO";
            // 
            // NavigatorDataGridView
            // 
            this.NavigatorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NavigatorDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorDataGridView.Location = new System.Drawing.Point(0, 0);
            this.NavigatorDataGridView.Name = "NavigatorDataGridView";
            this.NavigatorDataGridView.Size = new System.Drawing.Size(526, 458);
            this.NavigatorDataGridView.TabIndex = 0;
            // 
            // NavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 458);
            this.Controls.Add(this.NavigatorSplitContainer);
            this.Name = "NavigatorForm";
            this.Text = "MiniNavigator";
            this.NavigatorSplitContainer.Panel1.ResumeLayout(false);
            this.NavigatorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).EndInit();
            this.NavigatorSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navObjectDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        private void NavigatorSplitContainer_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            this.colMain.Width = this.NavigatorSplitContainer.Panel1.Width;
        }

        #endregion

        private Infralution.Controls.VirtualTree.VirtualTree NavigatorVirtualTree;
        private System.Windows.Forms.BindingSource navObjectDTOBindingSource;
        private Infralution.Controls.VirtualTree.Column colMain;
        private Infralution.Controls.VirtualTree.ObjectRowBinding rowBindingNavObjectDTO;
        private System.Windows.Forms.SplitContainer NavigatorSplitContainer;
        private System.Windows.Forms.DataGridView NavigatorDataGridView;
    }

}

